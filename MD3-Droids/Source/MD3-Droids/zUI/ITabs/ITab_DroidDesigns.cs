using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class ITab_DroidDesigns : ITab
    {
        private static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);

        private const float SectionMargin = 10f;
        private const float DesignEntryHeight = 30f;
        private const float DesignEntryMargin = 0f;
        private const float EntryHeightWithMargin = DesignEntryHeight + DesignEntryMargin;
        private Vector2 scrollPos = default(Vector2);
        private DroidDesign selDesign = null;

        private static readonly Vector2 CreateButtonSize = new Vector2(150f, 29f);
        public static readonly Vector2 SmallSize = new Vector2(260f, 600f);
        public static readonly Vector2 LargeSize = new Vector2(1310f, 600f);

        public ITab_DroidDesigns()
        {
            size = SmallSize;
            labelKey = "Designs";//TODO:: implement proper label key
        }

        protected override void FillTab()
        {
            if (selDesign == null)
                size = SmallSize;
            else
                size = LargeSize;

            Rect baseRect = new Rect(0f, 20f, size.x, size.y - 20f);
            Rect mainRect = baseRect.ContractedBy(10f);

            GUI.BeginGroup(mainRect);

            //Designs list area
            Rect designsListRect = new Rect(0f, 0f, 240f, mainRect.height - CreateButtonSize.y - SectionMargin);
            Widgets.DrawBoxSolid(designsListRect, BoxColor);
            DrawDesignList(designsListRect);
            //Create button
            Rect createButtonRect = new Rect(designsListRect.xMax - CreateButtonSize.x, designsListRect.yMax + SectionMargin, CreateButtonSize.x, CreateButtonSize.y);
            if (Widgets.ButtonText(createButtonRect, "Create New"))
            {
                Func<List<FloatMenuOption>> chassisTypeOptionsMaker = delegate
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    //list.Add(new FloatMenuOption("Small", delegate
                    //{
                    //TODO:: implement this
                    //}));
                    list.Add(new FloatMenuOption("Medium", delegate
                    {
                        Find.WindowStack.Add(new Dialog_EditDesign(new DroidDesign(ChassisType.Medium), true));
                    }));
                    //list.Add(new FloatMenuOption("Large", delegate
                    //{
                    //TODO:: implement this
                    //}));
                    return list;
                };
                Find.WindowStack.Add(new FloatMenu(chassisTypeOptionsMaker()));
            }

            if (selDesign != null)
            {
                //Droid display area
                Rect droidDisplayRect = new Rect(designsListRect.xMax + SectionMargin, 0f, 420f, designsListRect.height);
                Widgets.DrawBoxSolid(droidDisplayRect, BoxColor);
                DroidDesignUIHandler.DrawPartSelector(droidDisplayRect, selDesign, false);

                //Design label
                Rect designLabelRect = new Rect(droidDisplayRect.x, droidDisplayRect.yMax + SectionMargin, droidDisplayRect.width, CreateButtonSize.y);
                if (selDesign != null)
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Text.Font = GameFont.Medium;
                    Widgets.Label(designLabelRect, selDesign.Label);
                    Text.Anchor = TextAnchor.UpperLeft;
                    Text.Font = GameFont.Small;
                }

                //Parts list area
                Rect partsRect = new Rect(droidDisplayRect.xMax + SectionMargin, 0f, 240f, 260f);
                Widgets.DrawBoxSolid(partsRect, BoxColor);
                DroidDesignUIHandler.DrawPartsList(partsRect, selDesign);

                float rectHeight = (mainRect.height - partsRect.height - (SectionMargin * 2)) / 2;
                //AI packages area
                Rect aiPackagesRect = new Rect(partsRect.x, partsRect.yMax + SectionMargin, partsRect.width, rectHeight);
                Widgets.DrawBoxSolid(aiPackagesRect, BoxColor);
                DroidDesignUIHandler.DrawAIList(aiPackagesRect, selDesign, false);

                //Skills list area
                Rect skillsRect = new Rect(partsRect.x, aiPackagesRect.yMax + SectionMargin, partsRect.width, rectHeight);
                Widgets.DrawBoxSolid(skillsRect, BoxColor);
                DrawSkillsList(skillsRect);

                //Stats display area
                Rect statsRect = new Rect(partsRect.xMax + SectionMargin, 0f, mainRect.width - partsRect.xMax, mainRect.height - skillsRect.height - SectionMargin);
                Widgets.DrawBoxSolid(statsRect, BoxColor);
                DrawStatsList(statsRect);

                Rect materialsCostRect = new Rect(statsRect.x, skillsRect.y, statsRect.width, skillsRect.height);
                Widgets.DrawBoxSolid(materialsCostRect, BoxColor);
                DrawMaterialsCostList(materialsCostRect);
            }

            GUI.EndGroup();
        }

        private void DrawDesignList(Rect mainRect)
        {
            if (DroidManager.Instance.Designs.Count == 0)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(mainRect, "No designs");
                Text.Anchor = TextAnchor.UpperLeft;
            }
            else
            {
                try
                {
                    GUI.BeginGroup(mainRect);

                    Rect outRect = new Rect(0f, 0f, mainRect.width, mainRect.height);

                    float height = DroidManager.Instance.Designs.Count * EntryHeightWithMargin;
                    Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                    float curY = 0f;
                    bool alternate = false;
                    Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);

                    foreach (var design in DroidManager.Instance.Designs)
                    {
                        Rect entryRect = new Rect(0f, curY, viewRect.width, DesignEntryHeight);
                        DrawDesignEntry(entryRect, design, alternate);
                        curY += EntryHeightWithMargin;
                        alternate = !alternate;
                    }
                    Widgets.EndScrollView();
                }
                catch (Exception e)
                {
                    Log.Error("Exception drawing the scroll box: \n" + e.ToString() + "\n" + e.StackTrace);
                }
                finally
                {
                    GUI.EndGroup();
                }
            }
        }

        private void DrawDesignEntry(Rect rect, DroidDesign design, bool alternate)
        {
            try
            {
                GUI.BeginGroup(rect);
                Rect entryRect = new Rect(0f, 0f, rect.width, rect.height - DesignEntryMargin);

                if (Widgets.ButtonInvisible(entryRect))
                {
                    selDesign = design;
                }

                if (selDesign != null && selDesign == design)
                    Widgets.DrawHighlightSelected(entryRect);
                else if (Mouse.IsOver(entryRect))
                    Widgets.DrawHighlight(entryRect);
                else if (alternate)
                    Widgets.DrawAltRect(entryRect);

                Rect textRect = new Rect(SectionMargin, 0, entryRect.width - SectionMargin, entryRect.height);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(textRect, design.Label);
                Text.Anchor = TextAnchor.UpperLeft;

                string text = design.ChassisType == ChassisType.Small ? "Small Chassis   " : design.ChassisType == ChassisType.Medium ? "Medium Chassis   " : design.ChassisType == ChassisType.Large ? "Large Chassis   " : "Undefined droid chassis   ";
                Text.Anchor = TextAnchor.LowerRight;
                Text.Font = GameFont.Tiny;
                Widgets.Label(textRect, text);
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private void DrawSkillsList(Rect mainRect)
        {

        }

        private void DrawStatsList(Rect mainRect)
        {

        }

        private void DrawMaterialsCostList(Rect mainRect)
        {

        }
    }
}
