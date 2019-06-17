using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class Dialog_CustomisePartGroup : Window
    {
        private static readonly Vector2 ButtonSize = new Vector2(120f, 40f);
        private const float HorizontalMargin = 20f;
        private const float SlotVerticalMargin = 20f;
        private BlueprintHandlerState state;

        private Texture2D partTex = null;
        private DroidCustomiseGroupDef group;
        private Blueprint design;

        public override Vector2 InitialSize => new Vector2(500, 400f + 30f);

        public PartCustomisePack Slot1 = null;
        public PartCustomisePack Slot1Temp = null;
        public PartCustomisePack Slot2 = null;
        public PartCustomisePack Slot2Temp = null;
        public PartCustomisePack Slot3 = null;
        public PartCustomisePack Slot3Temp = null;
        public PartCustomisePack Slot4 = null;
        public PartCustomisePack Slot4Temp = null;
        public PartCustomisePack Slot5 = null;
        public PartCustomisePack Slot5Temp = null;
        public PartCustomisePack Slot6 = null;
        public PartCustomisePack Slot6Temp = null;

        public Dialog_CustomisePartGroup(DroidCustomiseGroupDef group, Blueprint design, Texture2D partTex, BlueprintHandlerState state)
        {
            this.partTex = partTex;
            this.state = state;

            this.group = group;
            this.design = design;
            Setup();

            //doCloseX = true;
            forcePause = true;
            absorbInputAroundWindow = true;
        }

        private void Setup()
        {
            List<PartCustomisePack> list = design.GetPartCustomisePacks(group).ToList();
            if (list.Count > 0)
            {
                int count = Mathf.Min(6, list.Count);
                //Initialise the values of each slot
                for (int i = 0; i < count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            Slot1 = list[i];
                            Slot1Temp = Slot1.CreateCopy();
                            break;
                        case 1:
                            Slot2 = list[i];
                            Slot2Temp = Slot2.CreateCopy();
                            break;
                        case 2:
                            Slot3 = list[i];
                            Slot3Temp = Slot3.CreateCopy();
                            break;
                        case 3:
                            Slot4 = list[i];
                            Slot4Temp = Slot4.CreateCopy();
                            break;
                        case 4:
                            Slot5 = list[i];
                            Slot5Temp = Slot5.CreateCopy();
                            break;
                        case 5:
                            Slot6 = list[i];
                            Slot6Temp = Slot6.CreateCopy();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public override void DoWindowContents(Rect inRect)
        {

            try
            {
                GUI.BeginGroup(inRect);

                //Draw label
                Rect labelRect = new Rect(0f, 0f, inRect.width, 40f);
                Text.Anchor = TextAnchor.UpperCenter;
                Text.Font = GameFont.Medium;
                Widgets.Label(labelRect, group.label);
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
                Widgets.DrawLine(new Vector2(labelRect.x + 15f, labelRect.yMax - 3f), new Vector2(labelRect.xMax - 15f, labelRect.yMax - 3f), Color.white, 1f);

                Rect texRect = new Rect(inRect.width / 2 - BlueprintUIUtil.TexRectSize.x / 2, inRect.height / 2 - BlueprintUIUtil.TexRectSize.y / 2 - 7f, BlueprintUIUtil.TexRectSize.x, BlueprintUIUtil.TexRectSize.y);
                Widgets.DrawTextureFitted(texRect, partTex, 1f);

                //Draw Left slot boxes
                Rect Slot2Rect = new Rect(0f, (texRect.y + texRect.height / 2) - BlueprintUIUtil.SlotRectSize.y / 2, inRect.width - texRect.xMax, BlueprintUIUtil.TotalSlotRectHeight);
                if (Slot2 != null)
                    BlueprintUIUtil.DrawSlot(Slot2Temp, Slot2Rect, design.ChassisType, state);

                Rect Slot1Rect = new Rect(Slot2Rect.x, Slot2Rect.y - BlueprintUIUtil.TotalSlotRectHeight, Slot2Rect.width, Slot2Rect.height);
                if (Slot1 != null)
                    BlueprintUIUtil.DrawSlot(Slot1Temp, Slot1Rect, design.ChassisType, state);

                Rect Slot3Rect = new Rect(Slot2Rect.x, Slot2Rect.yMax, Slot2Rect.width, Slot2Rect.height);
                if (Slot3 != null)
                    BlueprintUIUtil.DrawSlot(Slot3Temp, Slot3Rect, design.ChassisType, state);

                //Draw right slot boxes
                Rect Slot4Rect = new Rect(texRect.xMax, Slot1Rect.y, inRect.width - texRect.xMax, Slot2Rect.height);
                if (Slot4 != null)
                    BlueprintUIUtil.DrawSlot(Slot4Temp, Slot4Rect, design.ChassisType, state);

                Rect Slot5Rect = new Rect(Slot4Rect.x, Slot2Rect.y, Slot4Rect.width, Slot2Rect.height);
                if (Slot5 != null)
                    BlueprintUIUtil.DrawSlot(Slot5Temp, Slot5Rect, design.ChassisType, state);

                Rect Slot6Rect = new Rect(Slot4Rect.x, Slot3Rect.y, Slot4Rect.width, Slot2Rect.height);
                if (Slot6 != null)
                    BlueprintUIUtil.DrawSlot(Slot6Temp, Slot6Rect, design.ChassisType, state);

                //Draw Buttons
                if (state == BlueprintHandlerState.New || state == BlueprintHandlerState.Edit)
                {
                    //Accept
                    Rect acceptButtonRect = new Rect(inRect.width - ButtonSize.x, inRect.height - ButtonSize.y, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(acceptButtonRect, "Accept".Translate()))
                    {
                        if (Slot1 != null)
                            Slot1.CopyFrom(Slot1Temp);
                        if (Slot2 != null)
                            Slot2.CopyFrom(Slot2Temp);
                        if (Slot3 != null)
                            Slot3.CopyFrom(Slot3Temp);
                        if (Slot4 != null)
                            Slot4.CopyFrom(Slot4Temp);
                        if (Slot5 != null)
                            Slot5.CopyFrom(Slot5Temp);
                        if (Slot6 != null)
                            Slot6.CopyFrom(Slot6Temp);
                        BlueprintUIUtil.StatDummy(design).InitialiseFromDesign();
                        RimWorld.StatsReportUtility.Reset();
                        Find.WindowStack.TryRemove(this);
                    }
                    //Cancel
                    Rect cancelButtonRect = new Rect(0f, inRect.height - ButtonSize.y, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(cancelButtonRect, "Cancel".Translate()))
                    {
                        Find.WindowStack.TryRemove(this);
                    }
                }
                else
                {
                    Rect closeButtonRect = new Rect(inRect.width / 2 - ButtonSize.x / 2, inRect.height - ButtonSize.y, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(closeButtonRect, "Close".Translate()))
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
    }
}
