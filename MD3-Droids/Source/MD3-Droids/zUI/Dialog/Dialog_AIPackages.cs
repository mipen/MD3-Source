using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class Dialog_AIPackages : Window
    {
        private Blueprint design;
        private List<AIPackageDef> packagesTemp = new List<AIPackageDef>();
        private List<AIPackageDef> availableTemp = null;

        private Vector2 availableScrollPos = default(Vector2);
        private Vector2 designScrollPos = default(Vector2);

        private AIPackageDef selAvailable = null;
        private AIPackageDef selDesign = null;

        private static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);
        private static readonly Vector2 ListViewSize = new Vector2(240f, 200f);
        private static readonly Vector2 ListButtonSize = new Vector2(40f, 30f);
        private static readonly Vector2 AcceptButtonSize = new Vector2(120f, 30f);
        private const float HorizontalMargin = 30f;
        private const float VerticalMargin = 10f;
        private const float TitleBarHeight = 30f;
        private const float EntryHeight = 30f;

        public override Vector2 InitialSize => new Vector2(580f, 350f);

        private List<AIPackageDef> AvailablePackagesForDesign
        {
            get
            {
                if (availableTemp == null)
                    availableTemp = DefDatabase<AIPackageDef>.AllDefs.Where(x => x.chassisType == design.ChassisType || x.chassisType == ChassisType.Any).ToList();
                return availableTemp;
            }
        }

        private List<AIPackageDef> PackagesTemp
        {
            get
            {
                if (packagesTemp == null)
                {
                    packagesTemp = new List<AIPackageDef>();
                    packagesTemp.AddRange(design.AIPackages);
                }
                return packagesTemp;
            }
        }

        public Dialog_AIPackages(Blueprint design)
        {
            this.design = design;

            doCloseX = true;
            absorbInputAroundWindow = true;
            forcePause = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            try
            {
                GUI.BeginGroup(inRect);

                Rect titleRect = new Rect(0f, 0f, inRect.width, TitleBarHeight);
                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = GameFont.Medium;
                Widgets.Label(titleRect, "AI Packages");
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;

                Rect availableRect = new Rect(0f, titleRect.yMax + VerticalMargin, ListViewSize.x, inRect.height - titleRect.yMax - AcceptButtonSize.y - VerticalMargin * 2);
                Widgets.DrawBoxSolid(availableRect, BoxColor);
                DrawList(availableRect, AvailablePackagesForDesign, availableScrollPos, ref selAvailable, ref selDesign, true);

                Rect chosenRect = new Rect(inRect.width - ListViewSize.x, availableRect.y, ListViewSize.x, availableRect.height);
                Widgets.DrawBoxSolid(chosenRect, BoxColor);
                DrawList(chosenRect, PackagesTemp, designScrollPos, ref selDesign, ref selAvailable, false);

                float butX = availableRect.xMax + ((chosenRect.x - availableRect.xMax) / 2) - ListButtonSize.x / 2;
                Rect addButtonRect = new Rect(butX, availableRect.center.y - VerticalMargin - ListButtonSize.y, ListButtonSize.x, ListButtonSize.y);
                if (Widgets.ButtonText(addButtonRect, "->"))
                {
                    if (selAvailable != null)
                    {
                        availableTemp.Remove(selAvailable);
                        packagesTemp.Add(selAvailable);
                        selDesign = selAvailable;
                        selAvailable = null;
                    }
                }

                Rect removeButtonRect = new Rect(addButtonRect.x, availableRect.center.y + VerticalMargin, ListButtonSize.x, ListButtonSize.y);
                if (Widgets.ButtonText(removeButtonRect, "<-"))
                {
                    if (selDesign != null)
                    {
                        packagesTemp.Remove(selDesign);
                        availableTemp.Add(selDesign);
                        selAvailable = selDesign;
                        selDesign = null;
                    }
                }

                Rect cancelButtonRect = new Rect(0f, inRect.height - AcceptButtonSize.y, AcceptButtonSize.x, AcceptButtonSize.y);
                if (Widgets.ButtonText(cancelButtonRect, "Cancel"))
                {
                    Find.WindowStack.TryRemove(this);
                }

                Rect acceptButtonRect = new Rect(inRect.width - AcceptButtonSize.x, cancelButtonRect.y, AcceptButtonSize.x, AcceptButtonSize.y);
                if (Widgets.ButtonText(acceptButtonRect, "Accept"))
                {
                    design.AIPackages.Clear();
                    design.AIPackages.AddRange(packagesTemp);
                    BlueprintUIUtil.StatDummy(design).InitialiseFromDesign();
                    RimWorld.StatsReportUtility.Reset();
                    Find.WindowStack.TryRemove(this);
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private void DrawList(Rect rect, List<AIPackageDef> packages, Vector2 scrollPos, ref AIPackageDef selPackage, ref AIPackageDef unselPackage, bool reportNoPackages)
        {
            if (packages.Count > 0)
            {
                try
                {
                    GUI.BeginGroup(rect);

                    Rect outRect = new Rect(0f, 0f, rect.width, rect.width);

                    float height = packages.Count * EntryHeight;
                    Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                    float curY = 0f;
                    bool alternate = false;
                    Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);

                    foreach (var p in packages)
                    {
                        Rect entryRect = new Rect(0f, curY, viewRect.width, EntryHeight);
                        DrawListEntry(entryRect, p, ref selPackage, ref unselPackage, alternate);
                        alternate = !alternate;
                        curY += EntryHeight;
                    }

                    Widgets.EndScrollView();
                }
                finally
                {
                    GUI.EndGroup();
                }
            }
            else if (reportNoPackages)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(rect, "No AI packages found");
                Text.Anchor = TextAnchor.UpperLeft;
            }
        }

        private void DrawListEntry(Rect entryRect, AIPackageDef p, ref AIPackageDef selPackage, ref AIPackageDef unselPackage, bool alternate)
        {
            if (IsSelected(p))
            {
                Widgets.DrawHighlightSelected(entryRect);
                if (Mouse.IsOver(entryRect))
                    TooltipHandler.TipRegion(entryRect, p.Tooltip);
            }
            else if (Mouse.IsOver(entryRect))
            {
                Widgets.DrawHighlight(entryRect);
                TooltipHandler.TipRegion(entryRect, p.Tooltip);
            }
            else if (alternate)
                Widgets.DrawAltRect(entryRect);

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(entryRect, $"   {p.LabelCap}");
            Text.Anchor = TextAnchor.UpperLeft;

            if (Widgets.ButtonInvisible(entryRect))
            {
                selPackage = p;
                unselPackage = null;
            }
        }

        private bool IsSelected(AIPackageDef d)
        {
            return selAvailable == d || selDesign == d;
        }
    }
}
