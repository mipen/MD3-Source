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
        public static Color boxColor = new Color(0.1098f, 0.1294f, 0.149f);

        private const float SectionMargin = 10f;
        private const float DesignEntryHeight = 30f;
        private const float DesignEntryMargin = 0f;
        private const float EntryHeightWithMargin = DesignEntryHeight + DesignEntryMargin;
        private Vector2 scrollPos = default(Vector2);
        private DroidDesign selDesign = null;

        private static Vector2 CreateButtonSize = new Vector2(150f, 29f);

        public ITab_DroidDesigns()
        {
            size = new Vector2(1310, 600);
            labelKey = "Designs";//TODO:: implement proper label key
        }

        protected override void FillTab()
        {
            Rect baseRect = new Rect(0f, 20f, size.x, size.y - 20f);
            Rect mainRect = baseRect.ContractedBy(10f);

            GUI.BeginGroup(mainRect);

            //Designs list area
            Rect designsListRect = new Rect(0f, 0f, 240f, mainRect.height - CreateButtonSize.y - SectionMargin);
            Widgets.DrawBoxSolid(designsListRect, boxColor);
            DrawDesignList(designsListRect);
            //Create button
            Rect createButtonRect = new Rect(designsListRect.xMax - CreateButtonSize.x, designsListRect.yMax + SectionMargin, CreateButtonSize.x, CreateButtonSize.y);
            if (Widgets.ButtonText(createButtonRect, "Create New"))
            {
                DroidManager.Instance.Designs.Add(new DroidDesign() { Label = "Design " });
            }

            //Droid display area
            Rect droidDisplayRect = new Rect(designsListRect.xMax + SectionMargin, 0f, 420f, designsListRect.height);
            Widgets.DrawBoxSolid(droidDisplayRect, boxColor);
            DrawDroidDisplay(droidDisplayRect);
            //Design label
            Rect designLabelRect = new Rect(droidDisplayRect.x, droidDisplayRect.yMax + SectionMargin, droidDisplayRect.width, CreateButtonSize.y);
            if (selDesign != null)
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = GameFont.Medium;
                Widgets.Label(designLabelRect, selDesign.Label + selDesign.ID.ToString()); //TODO:: show only design label
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }

            //Parts list area
            Rect partsRect = new Rect(droidDisplayRect.xMax + SectionMargin, 0f, 240f, 260f);
            Widgets.DrawBoxSolid(partsRect, boxColor);
            DrawPartsList(partsRect);

            float rectHeight = (mainRect.height - partsRect.height - (SectionMargin * 2)) / 2;
            //AI packages area
            Rect aiPackagesRect = new Rect(partsRect.x, partsRect.yMax + SectionMargin, partsRect.width, rectHeight);
            Widgets.DrawBoxSolid(aiPackagesRect, boxColor);
            DrawAIPackagesList(aiPackagesRect);

            //Skills list area
            Rect skillsRect = new Rect(partsRect.x, aiPackagesRect.yMax + SectionMargin, partsRect.width, rectHeight);
            Widgets.DrawBoxSolid(skillsRect, boxColor);
            DrawSkillsList(skillsRect);

            //Stats display area
            Rect statsRect = new Rect(partsRect.xMax + SectionMargin, 0f, mainRect.width - partsRect.xMax, mainRect.height);
            Widgets.DrawBoxSolid(statsRect, boxColor);
            DrawStatsList(statsRect);

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
                    selDesign = design;

                if (selDesign != null && selDesign == design)
                    Widgets.DrawHighlightSelected(entryRect);
                else if (Mouse.IsOver(entryRect))
                    Widgets.DrawHighlight(entryRect);
                else if (alternate)
                    Widgets.DrawAltRect(entryRect);

                Text.Anchor = TextAnchor.MiddleLeft;
                Rect textRect = new Rect(SectionMargin, 0, entryRect.height, entryRect.width - SectionMargin);
                Widgets.Label(textRect, design.Label + design.ID);//TODO:: show only design label
                Text.Anchor = TextAnchor.UpperLeft;
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private void DrawDroidDisplay(Rect mainRect)
        {

        }

        private void DrawPartsList(Rect mainRect)
        {

        }

        private void DrawAIPackagesList(Rect mainRect)
        {

        }

        private void DrawSkillsList(Rect mainRect)
        {

        }

        private void DrawStatsList(Rect mainRect)
        {

        }
    }
}
