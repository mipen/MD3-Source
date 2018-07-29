using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class Dialog_SelectPart : Window
    {
        private List<DroidChassisPartDef> availableParts = new List<DroidChassisPartDef>();
        private DroidChassisPartDef basePart = null;
        private const float EntryHeight = 30f;
        private static readonly Vector2 ButtonSize = new Vector2(120f, 30f);
        private Vector2 scrollPos = default(Vector2);
        private DroidChassisPartDef selPart = null;
        private PartCustomisePack pcp = null;

        public override Vector2 InitialSize => new Vector2(350f, 370f);

        public Dialog_SelectPart(PartCustomisePack pcp, ChassisType ct)
        {
            if (pcp == null)
                Log.Error("PartCustomisePack was null");
            this.pcp = pcp;

            availableParts = (from t in DefDatabase<DroidChassisPartDef>.AllDefs
                              where (t.ChassisType == ct || t.ChassisType == ChassisType.Any) && t.ChassisPoint == pcp.ChassisPoint
                              select t).ToList();
            var list = (from t in availableParts
                        where t.BasePart == true
                        select t).ToList();
            if (list.Count > 0)
            {
                basePart = list.First();
                availableParts.Remove(basePart);
            }
            if (pcp.Part is DroidChassisPartDef)
            {
                var p = pcp.Part as DroidChassisPartDef;
                selPart = p;
            }

            absorbInputAroundWindow = true;
            forcePause = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            try
            {
                GUI.BeginGroup(inRect);

                if (availableParts.Count > 0 || basePart != null)
                {
                    //Label
                    Rect labelRect = new Rect(0f, 0f, inRect.width, EntryHeight);
                    Text.Anchor = TextAnchor.UpperCenter;
                    Text.Font = GameFont.Medium;
                    Widgets.Label(labelRect, "Select a part");
                    Text.Anchor = TextAnchor.UpperLeft;
                    Text.Font = GameFont.Small;
                    Widgets.DrawLine(new Vector2(0, labelRect.yMax), new Vector2(inRect.width, labelRect.yMax), Color.white, 1f);

                    Rect outRect = new Rect(0f, labelRect.yMax+10f, inRect.width, inRect.height - ButtonSize.y - 10f);

                    float height = 0f;
                    if (availableParts.Count > 0)
                        height += availableParts.Count * EntryHeight;
                    if (basePart != null)
                        height += EntryHeight;
                    Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                    float curY = 0f;
                    Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);
                    bool alternate = false;
                    if (basePart != null)
                    {
                        Rect rect = new Rect(0f, curY, viewRect.width, EntryHeight);
                        DrawEntry(basePart, rect, alternate, "<Base>");
                        curY += EntryHeight;
                        alternate = true;
                    }
                    if (availableParts.Count > 0)
                    {
                        foreach (var p in availableParts)
                        {
                            Rect rect = new Rect(0f, curY, viewRect.width, EntryHeight);
                            DrawEntry(p, rect, alternate);
                            alternate = !alternate;
                            curY += EntryHeight;
                        }
                    }
                    Widgets.EndScrollView();

                    Rect cancelButRect = new Rect(0f, inRect.height - ButtonSize.y, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(cancelButRect, "Cancel"))
                    {
                        Find.WindowStack.TryRemove(this);
                    }

                    Rect acceptButRect = new Rect(inRect.width - ButtonSize.x, cancelButRect.y, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(acceptButRect, "Accept"))
                    {
                        Log.Message("got here");
                        if (selPart != null)
                        {
                            Log.Message($"selPart not null {selPart.defName}");
                            pcp.Part = selPart;
                            Find.WindowStack.TryRemove(this);
                        }
                        else
                        {
                            Messages.Message(new Message("Select a part", DefDatabase<MessageTypeDef>.GetNamed("RejectInput")));
                        }
                    }
                }
                else
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(inRect, "No parts found");
                    Text.Anchor = TextAnchor.UpperLeft;

                    Rect butRect = new Rect(inRect.width / 2 - ButtonSize.x / 2, inRect.height - ButtonSize.y, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(butRect, "Close"))
                    {
                        Find.WindowStack.TryRemove(this);
                    }
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private void DrawEntry(DroidChassisPartDef d, Rect rect, bool alternate, string label = "")
        {
            string labelToUse = label.NullOrEmpty() ? d.label : label;

            if (d == selPart)
            {
                Widgets.DrawHighlightSelected(rect);
                if (Mouse.IsOver(rect))
                    TooltipHandler.TipRegion(rect, d.GetTooltip());
            }
            else if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
                TooltipHandler.TipRegion(rect, d.GetTooltip());
            }
            else if (alternate)
                Widgets.DrawAltRect(rect);

            if (Widgets.ButtonInvisible(rect))
            {
                selPart = d;
            }

            Text.Anchor = TextAnchor.MiddleLeft;
            if (d.color != null)
                GUI.color = d.color.GetColor();
            Widgets.Label(rect, labelToUse);
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
        }
    }
}
