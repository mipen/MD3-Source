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
    public static class BlueprintUIUtil
    {
        #region Variables
        public static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);
        public static readonly Vector2 ButtonSize = new Vector2(120f, 30f);

        #region PartSelector
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
        public const float PartSelectorArrowHeight = 25f;
        public const float PartSelectorMargin = 10f;
        public const float PartSelectorHeaderHeight = 35f;
        public const float PartSelectorFooterHeight = 100f;
        public const float PartSelectorSideBarWidth = 25f;
        #endregion

        #region Parts List
        public const float PartGroupTitleHeight = 40f;
        public const float PartEntryHeight = 30f;
        private const float PartsListTitleHeight = 30f;
        #endregion

        #region AIPackages List
        public const float AIPackagesTitleBar = 30f;
        public const float AIEntryHeight = 30f;
        public static readonly Vector2 AddAIButtonSize = new Vector2(30f, 30f);
        #endregion

        #region Progress Bar
        public static readonly Color Blue = new Color(0.094f, 0.592f, 0.905f, 1f);
        public static readonly Color Orange = new Color(0.874f, 0.36f, 0.066f, 1f);
        public static readonly Color Red = new Color(0.964f, 0.082f, 0.074f, 1f);
        #endregion

        #region Slot
        public static readonly Texture2D DefaultSlotIcon;
        public static readonly Vector2 TexRectSize = new Vector2(128f, 128f);
        public static readonly Vector2 SlotRectSize = new Vector2(64f, 64f);
        public static readonly Vector2 IconRectSize = new Vector2(32f, 32f);
        public const float SlotLabelHeight = 25f;
        public static readonly float TotalSlotRectHeight = SlotRectSize.y + SlotLabelHeight + 10f;
        #endregion

        #region Skills List

        private const float SkillsEntryHeight = 40f;
        private const float SkillsEntryLevelDisplayWidth = 20f;

        #endregion

        #endregion

        #region Properties
        private static Droid statDummy = null;
        #endregion

        static BlueprintUIUtil()
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

        public static Droid StatDummy(Blueprint bp)
        {
            if (statDummy == null || statDummy.kindDef != bp.KindDef || statDummy.blueprint != bp)
            {
                statDummy = (Droid)PawnGenerator.GeneratePawn(bp.KindDef);
                statDummy.blueprint = bp;
                statDummy.InitialiseFromBlueprint();
                StatsReportUtility.Reset();
            }
            return statDummy;
        }

        public static void DrawPartSelector(Rect mainRect, Blueprint bp, BlueprintHandlerState state)
        {
            try
            {
                Rect inRect = mainRect.ContractedBy(10f);

                Widgets.DrawBoxSolid(mainRect, BoxColor);
                Widgets.DrawBox(mainRect);

                GUI.BeginGroup(inRect);

                #region Header
                Rect headerRect = new Rect(0f, 0f, inRect.width, PartSelectorHeaderHeight);
                try
                {
                    GUI.BeginGroup(headerRect);
                    if (state == BlueprintHandlerState.New)
                    {
                        Rect chassisTypeRect = new Rect(headerRect.width / 2 - ButtonSize.x / 2, headerRect.height / 2 - ButtonSize.y / 2, ButtonSize.x, ButtonSize.y);
                        if (Widgets.ButtonText(chassisTypeRect, GetChassisString(bp.ChassisType)))
                        {
                            Func<List<FloatMenuOption>> chassisTypeOptionsMaker = delegate
                            {
                                List<FloatMenuOption> list = new List<FloatMenuOption>();
                                if (bp.ChassisType != ChassisType.Small)
                                    list.Add(new FloatMenuOption("SmallChassis".Translate(), delegate
                                    {
                                        bp.ChassisType = ChassisType.Small;
                                    }));
                                if (bp.ChassisType != ChassisType.Medium)
                                    list.Add(new FloatMenuOption("MediumChassis".Translate(), delegate
                                    {
                                        bp.ChassisType = ChassisType.Medium;
                                    }));
                                if (bp.ChassisType != ChassisType.Large)
                                    list.Add(new FloatMenuOption("LargeChassis".Translate(), delegate
                                    {
                                        bp.ChassisType = ChassisType.Large;
                                    }));
                                return list;
                            };
                            Find.WindowStack.Add(new FloatMenu(chassisTypeOptionsMaker()));
                        }
                    }
                    else
                    {
                        string headerLabelString = GetChassisString(bp.ChassisType);
                        Rect headerLabelRect = new Rect(0f, 0f, headerRect.width, headerRect.height);
                        Text.Anchor = TextAnchor.MiddleCenter;
                        Widgets.Label(headerLabelRect, headerLabelString);
                        Text.Anchor = TextAnchor.UpperLeft;
                    }
                }
                finally
                {
                    GUI.EndGroup();
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                #endregion

                //Footer
                Rect footerRect = new Rect(0f, inRect.height - PartSelectorFooterHeight, inRect.width, PartSelectorFooterHeight);
                DrawBottomSlots(footerRect, bp, state);

                #region Side Arrows
                //Side arrows
                if (state == BlueprintHandlerState.Edit || state == BlueprintHandlerState.New)
                {
                    Rect leftSideRect = new Rect(0f, headerRect.height, PartSelectorSideBarWidth, inRect.height - headerRect.height - footerRect.height);
                    try
                    {
                        GUI.BeginGroup(leftSideRect);
                        Rect headArrowRect = new Rect(0f, leftSideRect.height / 6 - PartSelectorArrowHeight / 2f, leftSideRect.width, PartSelectorArrowHeight);
                        Widgets.DrawHighlightIfMouseover(headArrowRect);
                        Widgets.DrawTextureFitted(headArrowRect, TexUI.ArrowTexLeft, 1f);
                        if (Widgets.ButtonInvisible(headArrowRect))
                        {
                            bp.HeadGraphicDef = DroidGraphics.GetPreviousHead(bp.ChassisType);
                        }

                        Rect bodyArrowRect = new Rect(0f, leftSideRect.height / 2, leftSideRect.width, PartSelectorArrowHeight);
                        Widgets.DrawHighlightIfMouseover(bodyArrowRect);
                        Widgets.DrawTextureFitted(bodyArrowRect, TexUI.ArrowTexLeft, 1f);
                        if (Widgets.ButtonInvisible(bodyArrowRect))
                        {
                            bp.BodyGraphicDef = DroidGraphics.GetPreviousBody(bp.ChassisType);
                        }
                    }
                    finally
                    {
                        GUI.EndGroup();
                    }
                    Rect rightSideRect = new Rect(inRect.width - PartSelectorSideBarWidth, headerRect.height, PartSelectorSideBarWidth, leftSideRect.height);
                    try
                    {
                        GUI.BeginGroup(rightSideRect);
                        Rect headArrowRect = new Rect(0f, leftSideRect.height / 6 - PartSelectorArrowHeight / 2f, leftSideRect.width, PartSelectorArrowHeight);
                        Widgets.DrawHighlightIfMouseover(headArrowRect);
                        Widgets.DrawTextureFitted(headArrowRect, TexUI.ArrowTexRight, 1f);
                        if (Widgets.ButtonInvisible(headArrowRect))
                        {
                            bp.HeadGraphicDef = DroidGraphics.GetNextHead(bp.ChassisType);
                        }

                        Rect bodyArrowRect = new Rect(0f, leftSideRect.height / 2, leftSideRect.width, PartSelectorArrowHeight);
                        Widgets.DrawHighlightIfMouseover(bodyArrowRect);
                        Widgets.DrawTextureFitted(bodyArrowRect, TexUI.ArrowTexRight, 1f);
                        if (Widgets.ButtonInvisible(bodyArrowRect))
                        {
                            bp.BodyGraphicDef = DroidGraphics.GetNextBody(bp.ChassisType);
                        }
                    }
                    finally
                    {
                        GUI.EndGroup();
                    }
                }
                #endregion

                //Draw selector
                Rect selectorRect = new Rect(PartSelectorSideBarWidth + 5f, headerRect.yMax, inRect.width - 10f - PartSelectorSideBarWidth * 2f, inRect.height - headerRect.height - footerRect.height - PartSelectorMargin);
                DrawParts(selectorRect, bp, state);
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        public static void DrawAIList(Rect mainRect, ref Vector2 scrollPos, Blueprint bp, BlueprintHandlerState state)
        {
            try
            {
                Widgets.DrawBoxSolid(mainRect, BoxColor);
                GUI.BeginGroup(mainRect);

                Rect titleRect = new Rect(10f, 0f, mainRect.width - 10f, AIPackagesTitleBar);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(titleRect, "AI Packages");
                Text.Anchor = TextAnchor.UpperLeft;

                if (state == BlueprintHandlerState.New || state == BlueprintHandlerState.Edit)
                {
                    Rect buttonRect = new Rect(mainRect.width - AddAIButtonSize.x, 0f, AddAIButtonSize.x, AddAIButtonSize.y);
                    if (Widgets.ButtonImage(buttonRect, AddButtonTex))
                    {
                        Dialog_AIPackages d = new Dialog_AIPackages(bp);
                        Find.WindowStack.Add(d);
                    }
                }

                if (bp.AIPackages.Count > 0)
                {
                    Rect listRect = new Rect(0f, titleRect.yMax, mainRect.width, mainRect.height - AIPackagesTitleBar);
                    DrawAIListing(listRect, ref scrollPos, bp);
                }
                else
                {
                    Rect labelRect = mainRect.AtZero();
                    Text.Font = GameFont.Small;
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(labelRect, "No AI Packages");
                    Text.Anchor = TextAnchor.UpperLeft;
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        public static void DrawPartsList(Rect mainRect, ref Vector2 scrollPos, Blueprint bp)
        {
            try
            {
                Widgets.DrawBoxSolid(mainRect, BoxColor);
                GUI.BeginGroup(mainRect);

                Rect titleRect = new Rect(10f, 0f, mainRect.width - 10f, PartsListTitleHeight);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(titleRect, "Installed parts");
                Text.Anchor = TextAnchor.UpperLeft;

                Rect outRect = new Rect(0f, titleRect.yMax, mainRect.width, mainRect.height - titleRect.yMax);

                float height = DesignPartsHeight(bp);
                if (height > 0)
                {
                    Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                    float curY = 0f;
                    Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);

                    foreach (var groupKey in bp.PartsGrouped.Keys)
                    {
                        var partsList = bp.PartsGrouped[groupKey].Where(x => x.Part.BasePart == false).ToList();
                        if (partsList.Any())
                        {
                            if (partsList[0].ChassisPoint == ChassisPoint.ArmourPlating)
                            {
                                float entryHeight = PartGroupHeight(bp.PartsGrouped[groupKey]);
                                Rect entryRect = new Rect(0f, curY, viewRect.width, entryHeight);
                                DrawPartsGroupListing(entryRect, groupKey, new List<PartCustomisePack>() { partsList[0] });
                                curY += entryHeight;
                            }
                            else
                            {
                                float entryHeight = PartGroupHeight(bp.PartsGrouped[groupKey]);
                                Rect entryRect = new Rect(0f, curY, viewRect.width, entryHeight);
                                DrawPartsGroupListing(entryRect, groupKey, bp.PartsGrouped[groupKey]);
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

        public static void DrawSkillsList(Rect mainRect, ref Vector2 scrollPos, Blueprint bp, BlueprintHandlerState state)
        {

            try
            {
                Widgets.DrawBoxSolid(mainRect, BoxColor);
                GUI.BeginGroup(mainRect);

                Rect titleRect = new Rect(10f, 0f, mainRect.width - 10f, PartsListTitleHeight);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(titleRect, "Skills");
                Text.Anchor = TextAnchor.UpperLeft;

                if (bp.Skills.Count > 0)
                {
                    Rect outRect = new Rect(0f, PartsListTitleHeight, mainRect.width, mainRect.height - PartsListTitleHeight);

                    float height = bp.Skills.Count * SkillsEntryHeight;
                    Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                    float curY = 0f;
                    bool alternate = false;
                    Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);
                    foreach (var skill in bp.Skills)
                    {
                        Rect entryRect = new Rect(0f, curY, viewRect.width, SkillsEntryHeight);
                        DrawSkillsEntry(entryRect, skill, bp, alternate, state);
                        curY += SkillsEntryHeight;
                        alternate = !alternate;
                    }
                    Widgets.EndScrollView();
                }
                else
                {
                    Rect labelRect = new Rect(0f, 0f, mainRect.width, mainRect.height);
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(labelRect, "No skills");
                    Text.Anchor = TextAnchor.UpperLeft;
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

        public static void DrawSlot(PartCustomisePack slot, Rect rect, ChassisType ct, BlueprintHandlerState state)
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

                if (state == BlueprintHandlerState.New || state == BlueprintHandlerState.Edit)
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

        private static void DrawParts(Rect rect, Blueprint bp, BlueprintHandlerState state)
        {
            try
            {
                GUI.BeginGroup(rect);

                //TODO:: draw gradient background
                float multiplier = Math.Min(rect.width / 328f, rect.height / 404f);
                if (multiplier > 1.2f)
                    multiplier = 1.2f;

                //Body rect
                Vector2 bodyRectAdjusted = new Vector2(BodyRectSize.x * multiplier, BodyRectSize.y * multiplier);
                Rect bodyRect = new Rect(rect.width / 2 - bodyRectAdjusted.x / 2, rect.height / 2 - bodyRectAdjusted.y / 2, bodyRectAdjusted.x, bodyRectAdjusted.y);
                Widgets.DrawHighlightIfMouseover(bodyRect);
                if (bp.BodyGraphic == null)
                {
                    bp.BodyGraphicDef = DroidGraphics.GetFirstBody(bp.ChassisType);
                }
                Widgets.DrawTextureFitted(bodyRect, bp.BodyGraphic.MatSouth.mainTexture, 1.6f);

                if (Widgets.ButtonInvisible(bodyRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidChassis"), bp, BodyTex, state);
                    Find.WindowStack.Add(cp);
                }

                //Head rect
                Vector2 headRectAdjusted = new Vector2(HeadRectSize.x * multiplier, HeadRectSize.y * multiplier);
                Rect headRect = new Rect(rect.width / 2 - headRectAdjusted.x / 2, bodyRect.y - PartSelectorMargin - headRectAdjusted.y, headRectAdjusted.x, headRectAdjusted.y);
                Widgets.DrawHighlightIfMouseover(headRect);
                if (bp.HeadGraphic == null)
                {
                    bp.HeadGraphicDef = DroidGraphics.GetFirstHead(bp.ChassisType);
                }
                Widgets.DrawTextureFitted(headRect, bp.HeadGraphic.MatSouth.mainTexture, 1.7f);

                if (Widgets.ButtonInvisible(headRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidHead"), bp, HeadTex, state);
                    Find.WindowStack.Add(cp);
                }

                //Left arm rect
                Vector2 armRectAdjusted = new Vector2(ArmRectSize.x * multiplier, ArmRectSize.y * multiplier);
                Rect leftArmRect = new Rect(bodyRect.x - PartSelectorMargin - armRectAdjusted.x, bodyRect.y, armRectAdjusted.x, armRectAdjusted.y);
                if (Mouse.IsOver(leftArmRect))
                    Widgets.DrawTextureFitted(leftArmRect, LeftArmHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(leftArmRect, LeftArmTex, 1);
                if (Widgets.ButtonInvisible(leftArmRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidLeftArm"), bp, LeftArmTex, state);
                    Find.WindowStack.Add(cp);
                }

                //Right arm rect
                Rect rightArmRect = new Rect(bodyRect.xMax + PartSelectorMargin, bodyRect.y, armRectAdjusted.x, armRectAdjusted.y);
                if (Mouse.IsOver(rightArmRect))
                    Widgets.DrawTextureFitted(rightArmRect, RightArmHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(rightArmRect, RightArmTex, 1);
                if (Widgets.ButtonInvisible(rightArmRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidRightArm"), bp, RightArmTex, state);
                    Find.WindowStack.Add(cp);
                }

                //Left leg rect
                Vector2 legRectAdjusted = new Vector2(LegRectSize.x * multiplier, LegRectSize.y * multiplier);
                Rect leftLegRect = new Rect(leftArmRect.x + legRectAdjusted.x / 2, bodyRect.yMax + PartSelectorMargin, legRectAdjusted.x, legRectAdjusted.y);
                if (Mouse.IsOver(leftLegRect))
                    Widgets.DrawTextureFitted(leftLegRect, LeftLegHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(leftLegRect, LeftLegTex, 1);
                if (Widgets.ButtonInvisible(leftLegRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidLeftLeg"), bp, LeftLegTex, state);
                    Find.WindowStack.Add(cp);
                }

                //Right leg rect 
                Rect rightLegRect = new Rect(rightArmRect.x - legRectAdjusted.x / 2, leftLegRect.y, legRectAdjusted.x, legRectAdjusted.y);
                if (Mouse.IsOver(rightLegRect))
                    Widgets.DrawTextureFitted(rightLegRect, RightLegHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(rightLegRect, RightLegTex, 1);
                if (Widgets.ButtonInvisible(rightLegRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidRightLeg"), bp, RightLegTex, state);
                    Find.WindowStack.Add(cp);
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private static void DrawBottomSlots(Rect rect, Blueprint bp, BlueprintHandlerState state)
        {
            try
            {
                GUI.BeginGroup(rect);

                List<object> list = new List<object>() { 1, 2, 3 };

                float slotWidth = rect.width / 3;
                Rect armourSlotRect = new Rect(slotWidth, rect.height - TotalSlotRectHeight, slotWidth, TotalSlotRectHeight);
                if (bp.HasArmourPlating)
                {
                    List<PartCustomisePack> armour = bp.GetPartCustomisePacks(DroidCustomiseGroupDef.Named("MD3_DroidArmourPlating"), true);
                    var pack = armour.First();
                    DrawSlot(pack, armourSlotRect, bp.ChassisType, state);
                    foreach (var p in armour)
                        p.Part = pack.Part;
                    bp.Recache();
                }

                Rect motivator1SlotRect = new Rect(0f, armourSlotRect.y, armourSlotRect.width, armourSlotRect.height);
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private static void DrawSkillsEntry(Rect entryRect, SkillLevel skill, Blueprint bp, bool alternate, BlueprintHandlerState state)
        {
            try
            {
                if (Mouse.IsOver(entryRect))
                    Widgets.DrawHighlight(entryRect);
                else if (alternate)
                    Widgets.DrawAltRect(entryRect);

                GUI.BeginGroup(entryRect);

                if (state == BlueprintHandlerState.New || state == BlueprintHandlerState.Edit)
                {
                    Rect sliderRect = new Rect(0f, 0f, entryRect.width, entryRect.height);
                    int prevLevel = skill.Level;
                    skill.Level = Mathf.RoundToInt(Widgets.HorizontalSlider(sliderRect, skill.Level, 0, 20, true, $"{skill.Skill.LabelCap} ({skill.Level})", "0", "20"));
                    if (skill.Level != prevLevel)
                    {
                        bp.Recache();
                        bp.AddSkillsToDroid(StatDummy(bp));
                        StatsReportUtility.Reset();
                    }
                }
                else
                {
                    Rect skillLabelRect = new Rect(0f, 0f, (entryRect.width / 2), entryRect.height);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Widgets.Label(skillLabelRect, skill.Skill.LabelCap);

                    Rect skillLevelRect = new Rect(skillLabelRect.xMax, 0f, entryRect.width / 2 - 5f, entryRect.height);
                    Text.Anchor = TextAnchor.MiddleRight;
                    Widgets.Label(skillLevelRect, skill.Level.ToString());
                    Text.Anchor = TextAnchor.UpperLeft;
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        private static void DrawAIListing(Rect listRect, ref Vector2 scrollPos, Blueprint bp)
        {
            try
            {
                GUI.BeginGroup(listRect);

                Rect outRect = new Rect(0f, 0f, listRect.width, listRect.height);

                float height = bp.AIPackages.Count * AIEntryHeight;
                Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                float curY = 0f;
                bool alternate = false;
                Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);
                foreach (var p in bp.AIPackages)
                {
                    Rect entryRect = new Rect(0f, curY, viewRect.width, AIEntryHeight);
                    DrawAIEntry(entryRect, p, alternate);
                    curY += AIEntryHeight;
                    alternate = !alternate;
                }
                Widgets.EndScrollView();
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

        private static float DesignPartsHeight(Blueprint bp)
        {
            float num = 0f;
            if (bp.PartsGrouped.Keys.Count > 0)
            {
                foreach (var group in bp.PartsGrouped.Values)
                {
                    num += PartGroupHeight(group);
                }
            }
            return num;
        }

        private static string GetChassisString(ChassisType type)
        {
            if (type == ChassisType.Small)
                return "SmallChassis".Translate();
            else if (type == ChassisType.Medium)
                return "MediumChassis".Translate();
            else if (type == ChassisType.Large)
                return "LargeChassis".Translate();
            else
                Log.Error("Should not get here");
            return "error";
        }
    }
}
