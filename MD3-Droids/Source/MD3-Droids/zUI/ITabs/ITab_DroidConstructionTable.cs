using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class ITab_DroidConstructionTable : ITab
    {
        private static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);

        private const float SectionMargin = 10f;
        private const float DesignEntryHeight = 30f;
        private const float DesignEntryMargin = 0f;
        private const float EntryHeightWithMargin = DesignEntryHeight + DesignEntryMargin;
        private Vector2 designsScrollPos = default(Vector2);
        private Vector2 partsScrollPos = default(Vector2);
        private Vector2 skillsScrollPos = default(Vector2);
        private Vector2 aiScrollPos = default(Vector2);
        private Blueprint selDesign = null;

        private static readonly Vector2 CreateButtonSize = new Vector2(150f, 29f);
        public static readonly Vector2 SmallSize = new Vector2(260f, 700f);
        public static readonly Vector2 LargeSize = new Vector2(1310f, 700f);
        public static bool DrawStats = true;

        public ITab_DroidConstructionTable()
        {
            size = SmallSize;
            labelKey = "Blueprints".Translate();
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
            //DEBUG:: Spawn droid button
            Rect spawnButtonRect = new Rect(designsListRect.xMax - 70f, designsListRect.yMax - 30f, 50f, 30f);
            if (Widgets.ButtonText(spawnButtonRect, "spawn"))
            {
                if (selDesign != null)
                {
                    Droid d = DroidGenerator.GenerateDroid(DefDatabase<PawnKindDef>.GetNamed("MD3_Droid"), selDesign, Faction.OfPlayer);
                    GenSpawn.Spawn(d, ((Building)SelObject).InteractionCell, ((Building)SelObject).Map);
                }
            }
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
                        Find.WindowStack.Add(new Dialog_NewBlueprint(new Blueprint(ChassisType.Medium)));
                        DrawStats = false;
                    }));
                    //list.Add(new FloatMenuOption("Large", delegate
                    //{
                    //TODO:: implement this
                    //}));
                    return list;
                };
                Find.WindowStack.Add(new FloatMenu(chassisTypeOptionsMaker()));
            }

            //Delete button
            if (selDesign != null)
            {
                Rect deleteButtonRect = new Rect(0f, createButtonRect.y, createButtonRect.x - 2f, CreateButtonSize.y);
                if (Widgets.ButtonText(deleteButtonRect, "Delete") && selDesign != null)
                {
                    Dialog_Confirm confirm = new Dialog_Confirm("Are you sure you wish to delete this design? \n\n This action cannot be undone.", delegate
                     {
                         DroidManager.Instance.Blueprints.Remove(selDesign);
                         selDesign = null;
                     });
                    Find.WindowStack.Add(confirm);
                }
            }

            if (selDesign != null)
            {
                //Part Selector area
                Rect partSelectorRect = new Rect(designsListRect.xMax + SectionMargin, 0f, 420f, designsListRect.height);
                Widgets.DrawBoxSolid(partSelectorRect, BoxColor);
                BlueprintUIUtil.DrawPartSelector(partSelectorRect, selDesign, false);

                //Design label
                Rect designLabelRect = new Rect(partSelectorRect.x, partSelectorRect.yMax + SectionMargin, partSelectorRect.width, CreateButtonSize.y);
                if (selDesign != null)
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Text.Font = GameFont.Medium;
                    Widgets.Label(designLabelRect, selDesign.Label);
                    Text.Anchor = TextAnchor.UpperLeft;
                    Text.Font = GameFont.Small;
                }

                //Parts list area
                Rect partsRect = new Rect(partSelectorRect.xMax + SectionMargin, 0f, 240f, 260f);
                BlueprintUIUtil.DrawPartsList(partsRect, ref partsScrollPos, selDesign);

                float rectHeight = (mainRect.height - partsRect.height - (SectionMargin * 2)) / 2;
                //AI packages area
                Rect aiPackagesRect = new Rect(partsRect.x, partsRect.yMax + SectionMargin, partsRect.width, rectHeight);
                BlueprintUIUtil.DrawAIList(aiPackagesRect, ref aiScrollPos, selDesign, false);

                //Skills list area
                Rect skillsRect = new Rect(partsRect.x, aiPackagesRect.yMax + SectionMargin, partsRect.width, rectHeight);
                BlueprintUIUtil.DrawSkillsList(skillsRect, ref skillsScrollPos, selDesign, false);

                //Stats display area
                Rect statsRect = new Rect(partsRect.xMax + SectionMargin, 0f, mainRect.width - partsRect.xMax, mainRect.height - skillsRect.height - SectionMargin);
                if (DrawStats)
                {
                    Widgets.DrawBoxSolid(statsRect, BoxColor);
                    StatsReportUtility.DrawStatsReport(statsRect, BlueprintUIUtil.StatDummy(selDesign));
                }

                //Bill of materials display area
                Rect materialsCostRect = new Rect(statsRect.x, skillsRect.y, statsRect.width, skillsRect.height);
                Widgets.DrawBoxSolid(materialsCostRect, BoxColor);

            }

            GUI.EndGroup();
        }

        private void DrawDesignList(Rect mainRect)
        {
            if (DroidManager.Instance.Blueprints.Count == 0)
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

                    float height = DroidManager.Instance.Blueprints.Count * EntryHeightWithMargin;
                    Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                    float curY = 0f;
                    bool alternate = false;
                    Widgets.BeginScrollView(outRect, ref designsScrollPos, viewRect);

                    foreach (var design in DroidManager.Instance.Blueprints)
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

        private void DrawDesignEntry(Rect rect, Blueprint design, bool alternate)
        {
            try
            {
                GUI.BeginGroup(rect);
                Rect entryRect = new Rect(0f, 0f, rect.width, rect.height - DesignEntryMargin);

                if (Widgets.ButtonInvisible(entryRect))
                {
                    if (selDesign != null && selDesign == design)
                        selDesign = null;
                    else
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

    }
}
