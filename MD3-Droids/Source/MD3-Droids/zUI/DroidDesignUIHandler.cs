using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    [StaticConstructorOnStartup]
    public static class DroidDesignUIHandler
    {
        #region Variables
        private static Vector2 partsScrollPos = default(Vector2);

        public static readonly Texture2D AddButtonTex;

        public static readonly Texture2D HeadTex;
        public static readonly Texture2D HeadHoverTex;

        public static readonly Texture2D BodyTex;
        public static readonly Texture2D BodyHoverTex;

        public static readonly Texture2D LeftArmTex;
        public static readonly Texture2D LeftArmHoverTex;

        public static readonly Texture2D RightArmTex;
        public static readonly Texture2D RightArmHoverTex;

        public static readonly Texture2D LeftLegTex;
        public static readonly Texture2D LeftLegHoverTex;

        public static readonly Texture2D RightLegTex;
        public static readonly Texture2D RightLegHoverTex;

        public static readonly Vector2 HeadRectSize = new Vector2(92, 92);
        public static readonly Vector2 BodyRectSize = new Vector2(128, 192);
        public static readonly Vector2 ArmRectSize = new Vector2(100, 100);
        public static readonly Vector2 LegRectSize = new Vector2(100, 100);
        public const float PartSelectorMargin = 10f;

        public const float PartGroupTitleHeight = 40f;
        public const float PartEntryHeight = 30f;
        private const float PartsListTitleHeight = 30f;

        public const float AIPackagesTitleBar = 30f;
        public const float AIEntryHeight = 30f;
        public static readonly Vector2 AddAIButtonSize = new Vector2(30f, 30f);

        public static readonly Color Blue = new Color(0.094f, 0.592f, 0.905f, 1f);
        public static readonly Color Orange = new Color(0.874f, 0.36f, 0.066f, 1f);
        public static readonly Color Red = new Color(0.964f, 0.082f, 0.074f, 1f);

        public static readonly Texture2D DefaultSlotIcon;
        public static readonly Vector2 TexRectSize = new Vector2(128f, 128f);
        public static readonly Vector2 SlotRectSize = new Vector2(64f, 64f);
        public static readonly Vector2 IconRectSize = new Vector2(32f, 32f);
        public const float SlotLabelHeight = 25f;
        public static readonly float TotalSlotRectHeight = SlotRectSize.y + SlotLabelHeight + 10f;

        public static readonly float PartSelectorMinDistFromBottom = (BodyRectSize.y + PartSelectorMargin + LegRectSize.y + PartSelectorMargin + TotalSlotRectHeight);
        #endregion

        static DroidDesignUIHandler()
        {
            DefaultSlotIcon = ContentFinder<Texture2D>.Get("Things/Item/Health/HealthItemProsthetic");
            AddButtonTex = ContentFinder<Texture2D>.Get("UI/AddButton");

            HeadTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_Head");
            HeadHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_HeadHover");

            BodyTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_Body");
            BodyHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_BodyHover");

            LeftArmTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftArm");
            LeftArmHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftArmHover");

            RightArmTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightArm");
            RightArmHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightArmHover");

            LeftLegTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftLeg");
            LeftLegHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftLegHover");

            RightLegTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightLeg");
            RightLegHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightLegHover");

        }

        public static void DrawPartSelector(Rect mainRect, DroidDesign design, bool editMode)
        {
            try
            {
                Rect inRect = mainRect.ContractedBy(10f);
                if (inRect.height < (HeadRectSize.y + PartSelectorMargin + BodyRectSize.y + PartSelectorMargin + LegRectSize.y + PartSelectorMargin + TotalSlotRectHeight))
                    Log.Error($"Rect is too small for PartSelector height: {inRect.height}");

                GUI.BeginGroup(inRect);

                //Body rect
                float bodyRectY = inRect.center.y - (BodyRectSize.y / 2);
                if (inRect.height - bodyRectY < PartSelectorMinDistFromBottom)
                    bodyRectY = inRect.height - PartSelectorMinDistFromBottom;
                Rect bodyRect = new Rect((inRect.width / 2) - (BodyRectSize.x / 2), bodyRectY, BodyRectSize.x, BodyRectSize.y);
                if (Mouse.IsOver(bodyRect))
                    Widgets.DrawTextureFitted(bodyRect, BodyHoverTex, 1.3f);
                else
                    Widgets.DrawTextureFitted(bodyRect, BodyTex, 1.3f);
                if (Widgets.ButtonInvisible(bodyRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidChassis"), design, BodyTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Head rect
                Rect headRect = new Rect((inRect.width / 2) - (HeadRectSize.x / 2), bodyRect.y - PartSelectorMargin - HeadRectSize.y, HeadRectSize.x, HeadRectSize.y);
                if (Mouse.IsOver(headRect))
                    Widgets.DrawTextureFitted(headRect, HeadHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(headRect, HeadTex, 1);
                if (Widgets.ButtonInvisible(headRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidHead"), design, HeadTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Left arm rect
                Rect leftArmRect = new Rect(bodyRect.x - PartSelectorMargin - ArmRectSize.x, bodyRect.y, ArmRectSize.x, ArmRectSize.y);
                if (Mouse.IsOver(leftArmRect))
                    Widgets.DrawTextureFitted(leftArmRect, LeftArmHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(leftArmRect, LeftArmTex, 1);
                if (Widgets.ButtonInvisible(leftArmRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidLeftArm"), design, LeftArmTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Right arm rect
                Rect rightArmRect = new Rect(bodyRect.xMax + PartSelectorMargin, bodyRect.y, ArmRectSize.x, ArmRectSize.y);
                if (Mouse.IsOver(rightArmRect))
                    Widgets.DrawTextureFitted(rightArmRect, RightArmHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(rightArmRect, RightArmTex, 1);
                if (Widgets.ButtonInvisible(rightArmRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidRightArm"), design, RightArmTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Left leg rect
                Rect leftLegRect = new Rect(leftArmRect.x + LegRectSize.x / 2, bodyRect.yMax + PartSelectorMargin, LegRectSize.x, LegRectSize.y);
                if (Mouse.IsOver(leftLegRect))
                    Widgets.DrawTextureFitted(leftLegRect, LeftLegHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(leftLegRect, LeftLegTex, 1);
                if (Widgets.ButtonInvisible(leftLegRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidLeftLeg"), design, LeftLegTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Right leg rect 
                Rect rightLegRect = new Rect(rightArmRect.x - LegRectSize.x / 2, leftLegRect.y, LegRectSize.x, LegRectSize.y);
                if (Mouse.IsOver(rightLegRect))
                    Widgets.DrawTextureFitted(rightLegRect, RightLegHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(rightLegRect, RightLegTex, 1);
                if (Widgets.ButtonInvisible(rightLegRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidRightLeg"), design, RightLegTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Bottom slots
                float slotWidth = inRect.width / 3;
                Rect armourSlotRect = new Rect(slotWidth, inRect.height - TotalSlotRectHeight, slotWidth, TotalSlotRectHeight);
                if (design.HasArmourPlating)
                {
                    List<PartCustomisePack> armour = design.GetPartCustomisePacks(DroidCustomiseGroupDef.Named("MD3_DroidArmourPlating"), true);
                    var pack = armour.First();
                    DrawSlot(pack, armourSlotRect, design.ChassisType, editMode);
                    foreach (var p in armour)
                        p.Part = pack.Part;
                    design.Recache();
                }

                Rect motivator1SlotRect = new Rect(0f, armourSlotRect.y, armourSlotRect.width, armourSlotRect.height);
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        public static void DrawAIList(Rect mainRect, DroidDesign design, bool editMode)
        {
            try
            {
                GUI.BeginGroup(mainRect);

                Rect titleRect = new Rect(10f, 0f, mainRect.width - 10f, AIPackagesTitleBar);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(titleRect, "AI Packages");
                Text.Anchor = TextAnchor.UpperLeft;

                if (editMode)
                {
                    Rect buttonRect = new Rect(mainRect.width - AddAIButtonSize.x, 0f, AddAIButtonSize.x, AddAIButtonSize.y);
                    if (Widgets.ButtonImage(buttonRect, AddButtonTex))
                    {
                        Dialog_AIPackages d = new Dialog_AIPackages(design);
                        Find.WindowStack.Add(d);
                    }
                }

                if (design.AIPackages.Count > 0)
                {
                    Rect listRect = new Rect(0f, titleRect.yMax, mainRect.width, mainRect.height - AIPackagesTitleBar);
                    DrawAIListing(listRect, design);
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        public static void DrawPartsList(Rect mainRect, DroidDesign design)
        {
            try
            {
                GUI.BeginGroup(mainRect);

                Rect titleRect = new Rect(10f, 0f, mainRect.width - 10f, PartsListTitleHeight);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(titleRect, "Installed parts");
                Text.Anchor = TextAnchor.UpperLeft;

                Rect outRect = new Rect(0f, titleRect.yMax, mainRect.width, mainRect.height - titleRect.yMax);

                float height = DesignPartsHeight(design);
                if (height > 0)
                {
                    Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                    float curY = 0f;
                    Widgets.BeginScrollView(outRect, ref partsScrollPos, viewRect);

                    foreach (var groupKey in design.PartsGrouped.Keys)
                    {
                        var partsList = design.PartsGrouped[groupKey].Where(x => x.Part.BasePart == false).ToList();
                        if (partsList.Any())
                        {
                            if (partsList[0].ChassisPoint == ChassisPoint.ArmourPlating)
                            {
                                float entryHeight = PartGroupHeight(design.PartsGrouped[groupKey]);
                                Rect entryRect = new Rect(0f, curY, viewRect.width, entryHeight);
                                DrawPartsGroupListing(entryRect, groupKey, new List<PartCustomisePack>() { partsList[0] });
                                curY += entryHeight;
                            }
                            else
                            {
                                float entryHeight = PartGroupHeight(design.PartsGrouped[groupKey]);
                                Rect entryRect = new Rect(0f, curY, viewRect.width, entryHeight);
                                DrawPartsGroupListing(entryRect, groupKey, design.PartsGrouped[groupKey]);
                                curY += entryHeight;
                            }
                        }
                    }

                    Widgets.EndScrollView();
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        public static void DrawProgressBar(Rect rect, float value, float maxValue, StatModifier sm, string toolTip = "")
        {
            try
            {
                GUI.BeginGroup(rect);
                Rect bgRect = new Rect(0f, 0f, rect.width, rect.height);
                Widgets.DrawAltRect(bgRect);

                float percentage = value / maxValue;
                float width = percentage < 1 ? rect.width * percentage : rect.width;
                Rect progressRect = new Rect(0f, 0f, width, rect.height);
                Color color = Blue;
                if (percentage > 0.9f)
                    color = Red;
                else if (percentage > 0.75f)
                    color = Orange;
                Widgets.DrawBoxSolid(progressRect, color);

                Widgets.DrawBox(bgRect);

                if (Mouse.IsOver(bgRect))
                {
                    Widgets.DrawHighlight(bgRect);
                    if (!toolTip.NullOrEmpty())
                        TooltipHandler.TipRegion(bgRect, toolTip);
                }

                string text = $"{sm.stat.ValueToString(sm.value)} / {sm.stat.ValueToString(maxValue)}";
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(bgRect, text);
                Text.Anchor = TextAnchor.UpperLeft;
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        public static void DrawSlot(PartCustomisePack slot, Rect rect, ChassisType ct, bool editMode)
        {
            try
            {
                GUI.BeginGroup(rect);

                Rect slotRect = new Rect(rect.width / 2 - SlotRectSize.x / 2, 0f, SlotRectSize.x, SlotRectSize.y);
                if (Mouse.IsOver(slotRect))
                {
                    Widgets.DrawHighlightSelected(slotRect);
                    TooltipHandler.TipRegion(slotRect, slot.Part.GetTooltip());
                }
                else
                    Widgets.DrawHighlight(slotRect);
                Widgets.DrawBox(slotRect);

                Rect imageRect = new Rect(slotRect.center.x - IconRectSize.x / 2, slotRect.center.y - IconRectSize.y / 2, IconRectSize.x, IconRectSize.y);
                Widgets.DrawTextureFitted(imageRect, DefaultSlotIcon, 1f);//TODO:: show slot icon

                Rect labelRect = new Rect(0f, SlotRectSize.y, rect.width, SlotLabelHeight);
                Text.Anchor = TextAnchor.MiddleCenter;
                if (slot.Part.color != null)
                    GUI.color = slot.Part.color.GetColor();
                Widgets.Label(labelRect, slot.Part.LabelCap);
                GUI.color = Color.white;

                if (editMode)
                {
                    if (Widgets.ButtonInvisible(slotRect))
                    {
                        Dialog_SelectPart sp = new Dialog_SelectPart(slot, ct);
                        Find.WindowStack.Add(sp);
                    }
                }
            }
            finally
            {
                Text.Anchor = TextAnchor.UpperLeft;
                GUI.EndGroup();
            }
        }

        private static void DrawAIListing(Rect listRect, DroidDesign design)
        {
            try
            {
                GUI.BeginGroup(listRect);

                Rect outRect = new Rect(0f, 0f, listRect.width, listRect.height);

                float height = design.AIPackages.Count * AIEntryHeight;
                Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                float curY = 0f;
                bool alternate = false;
                foreach (var p in design.AIPackages)
                {
                    Rect entryRect = new Rect(0f, curY, viewRect.width, AIEntryHeight);
                    DrawAIEntry(entryRect, p, alternate);
                    curY += AIEntryHeight;
                    alternate = !alternate;
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private static void DrawAIEntry(Rect entryRect, AIPackageDef p, bool alternate)
        {
            if (Mouse.IsOver(entryRect))
            {
                Widgets.DrawHighlight(entryRect);
                TooltipHandler.TipRegion(entryRect, p.Tooltip);
            }
            else if (alternate)
                Widgets.DrawAltRect(entryRect);

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(entryRect, $"   {p.LabelCap}");
            Text.Anchor = TextAnchor.UpperLeft;
        }

        private static void DrawPartsGroupListing(Rect rect, DroidCustomiseGroupDef group, List<PartCustomisePack> packs)
        {
            try
            {
                GUI.BeginGroup(rect);

                Rect labelRect = new Rect(10f, 0f, rect.width - 10f, PartGroupTitleHeight);
                Text.Anchor = TextAnchor.MiddleLeft;
                Text.Font = GameFont.Medium;
                Widgets.Label(labelRect, group.LabelCap);
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;

                var list = packs.Where(x => x.Part.BasePart == false);
                float curY = labelRect.yMax;
                bool alternate = false;
                foreach (var part in list)
                {
                    Rect pRect = new Rect(0f, curY, rect.width, PartEntryHeight);
                    if (Mouse.IsOver(pRect))
                    {
                        Widgets.DrawHighlight(pRect);
                        TooltipHandler.TipRegion(pRect, part.Part.GetTooltip());
                    }
                    else if (alternate)
                        Widgets.DrawAltRect(pRect);

                    Text.Anchor = TextAnchor.MiddleLeft;
                    if (part.Part.color != null)
                        GUI.color = part.Part.color.GetColor();
                    Widgets.Label(pRect, $"   {part.Part.LabelCap}");
                    Text.Anchor = TextAnchor.UpperLeft;
                    GUI.color = Color.white;
                    curY += PartEntryHeight;
                    alternate = !alternate;
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private static float PartGroupHeight(List<PartCustomisePack> packs)
        {
            List<PartCustomisePack> list = (from p in packs
                                            where p.Part.BasePart == false
                                            select p).ToList();
            float num = 0f;
            if (list.Count > 0)
            {
                if (list[0].Part.ChassisPoint == ChassisPoint.ArmourPlating)
                {
                    num += PartGroupTitleHeight;
                    num += PartEntryHeight;
                }
                else
                {
                    num += PartGroupTitleHeight;
                    num += PartEntryHeight * list.Count;
                }
            }
            return num;
        }

        private static float DesignPartsHeight(DroidDesign design)
        {
            float num = 0f;
            if (design.PartsGrouped.Keys.Count > 0)
            {
                foreach (var group in design.PartsGrouped.Values)
                {
                    num += PartGroupHeight(group);
                }
            }
            return num;
        }
    }
}
