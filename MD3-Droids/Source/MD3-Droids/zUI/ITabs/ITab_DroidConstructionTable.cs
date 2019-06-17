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
        private BlueprintWindowHandler bpHandler;

        private static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);

        private const float SectionMargin = 10f;
        private const float DesignEntryHeight = 30f;
        private const float DesignEntryMargin = 0f;
        private const float EntryHeightWithMargin = DesignEntryHeight + DesignEntryMargin;
        private Vector2 designsScrollPos = default(Vector2);
        private Blueprint selBlueprint = null;

        private static readonly Vector2 CreateButtonSize = new Vector2(150f, 29f);
        public static readonly Vector2 SmallSize = new Vector2(260f, 800f);
        public static readonly Vector2 LargeSize = new Vector2(1310f, 800f);
        public static bool DrawStats = true;

        public ITab_DroidConstructionTable()
        {
            size = SmallSize;
            labelKey = "Blueprints".Translate();
        }

        protected override void FillTab()
        {
            if (selBlueprint == null)
                size = SmallSize;
            else
                size = LargeSize;

            Rect baseRect = new Rect(0f, 20f, size.x, size.y - 20f);
            Rect mainRect = baseRect.ContractedBy(10f);

            try
            {
                GUI.BeginGroup(mainRect);

                //Designs list area
                Rect designsListRect = new Rect(0f, 0f, 240f, mainRect.height - CreateButtonSize.y - SectionMargin);
                Widgets.DrawBoxSolid(designsListRect, BoxColor);
                DrawDesignList(designsListRect);

                //DEBUG:: Spawn droid button
                Rect spawnButtonRect = new Rect(designsListRect.xMax - 70f, designsListRect.yMax - 30f, 50f, 30f);
                if (Widgets.ButtonText(spawnButtonRect, "spawn"))
                {
                    if (selBlueprint != null)
                    {
                        Droid d = DroidGenerator.GenerateDroid(DefDatabase<PawnKindDef>.GetNamed("MD3_Droid"), selBlueprint, Faction.OfPlayer);
                        GenSpawn.Spawn(d, ((Building)SelObject).InteractionCell, ((Building)SelObject).Map);
                    }
                }

                //Create button
                Rect createButtonRect = new Rect(designsListRect.xMax - CreateButtonSize.x, designsListRect.yMax + SectionMargin, CreateButtonSize.x, CreateButtonSize.y);
                if (Widgets.ButtonText(createButtonRect, "New".Translate()))
                {
                    Find.WindowStack.Add(new Dialog_NewBlueprint(new Blueprint()));
                }

                //Delete button
                if (selBlueprint != null)
                {
                    Rect deleteButtonRect = new Rect(0f, createButtonRect.y, createButtonRect.x - 2f, CreateButtonSize.y);
                    if (Widgets.ButtonText(deleteButtonRect, "Delete".Translate()) && selBlueprint != null)
                    {
                        Dialog_Confirm confirm = new Dialog_Confirm("ConfirmDeleteBlueprint".Translate(), delegate
                         {
                             DroidManager.Instance.Blueprints.Remove(selBlueprint);
                             selBlueprint = null;
                         });
                        Find.WindowStack.Add(confirm);
                    }
                }

                if (selBlueprint != null)
                {
                    Rect blueprintRect = new Rect(designsListRect.xMax + SectionMargin, 0f, mainRect.width - designsListRect.width - SectionMargin, mainRect.height);
                    bpHandler?.DrawWindow(blueprintRect);
                }
            }
            finally
            {
                GUI.EndGroup();
            }
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

        private void DrawDesignEntry(Rect rect, Blueprint bp, bool alternate)
        {
            try
            {
                GUI.BeginGroup(rect);
                Rect entryRect = new Rect(0f, 0f, rect.width, rect.height - DesignEntryMargin);

                if (Widgets.ButtonInvisible(entryRect))
                {
                    if (selBlueprint != null && selBlueprint == bp)
                        selBlueprint = null;
                    else
                    {
                        selBlueprint = bp;
                        bpHandler = new BlueprintWindowHandler(selBlueprint, BlueprintHandlerState.Normal);
                        bpHandler.CloseButtonVisible = false;
                        bpHandler.EditButtonVisible = true;
                    }
                }

                if (selBlueprint != null && selBlueprint == bp)
                    Widgets.DrawHighlightSelected(entryRect);
                else if (Mouse.IsOver(entryRect))
                    Widgets.DrawHighlight(entryRect);
                else if (alternate)
                    Widgets.DrawAltRect(entryRect);

                Rect textRect = new Rect(SectionMargin, 0, entryRect.width - SectionMargin, entryRect.height);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(textRect, bp.Label);
                Text.Anchor = TextAnchor.UpperLeft;

                string text = bp.ChassisType == ChassisType.Small ? "SmallChassis".Translate() : bp.ChassisType == ChassisType.Medium ? "MediumChassis".Translate() : bp.ChassisType == ChassisType.Large ? "LargeChassis".Translate() : "Undefined droid chassis   ";
                text += "   ";
                Text.Anchor = TextAnchor.LowerRight;
                Text.Font = GameFont.Tiny;
                Widgets.Label(textRect, text);
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }
            finally
            {
                GUI.EndGroup();
                Text.Anchor = TextAnchor.UpperLeft;
            }
        }

    }
}
