using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class Dialog_EditDesign : Window
    {
        private DroidDesign design;
        private bool isNewDesign = false;

        private static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);

        private const float DroidDisplayAreaWidth = 420f;
        private const float StatsDisplayAreaWidth = 300f;
        private const float DisplayAreaWidth = 300f;
        private const float SectionMargin = 10f;
        private const float TitleRectHeight = 45f;
        private const float FooterRectHeight = 100f;
        private const float DrawBarHeight = 25f;
        private static readonly Vector2 ButtonSize = new Vector2(120f, 30f);

        private string designName = "";

        public override Vector2 InitialSize => new Vector2(1070f, 800f);

        public Dialog_EditDesign(DroidDesign design, bool isNewDesign)
        {
            this.design = design;
            this.isNewDesign = isNewDesign;
            if (!isNewDesign)
                designName = design.Label;

            absorbInputAroundWindow = true;
            forcePause = true;
            doCloseX = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            try
            {
                GUI.BeginGroup(inRect);

                string nameRectLabel = "Design name: ";
                Rect nameLabelRect = new Rect(0f, 0f, Text.CalcSize(nameRectLabel).x, TitleRectHeight);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(nameLabelRect, nameRectLabel);
                Text.Anchor = TextAnchor.UpperLeft;
                //Text box
                float textBoxHeight = 28;
                Rect nameRect = new Rect(nameLabelRect.xMax + 5f, TitleRectHeight / 2 - textBoxHeight / 2, 200f, textBoxHeight);
                designName = Widgets.TextField(nameRect, designName);

                Rect titleRect = new Rect(0f, 0f, inRect.width, TitleRectHeight);
                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = GameFont.Medium;
                string title = design.ChassisType == ChassisType.Small ? "Small Chassis" : design.ChassisType == ChassisType.Medium ? "Medium Chassis" : design.ChassisType == ChassisType.Large ? "Large Chassis" : "Undefined droid chassis";
                Widgets.Label(titleRect, title);
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;

                //MainRect
                Rect mainRect = new Rect(0f, TitleRectHeight, inRect.width, inRect.height - TitleRectHeight);
                GUI.BeginGroup(mainRect);
                float availableHeight = mainRect.height - FooterRectHeight - SectionMargin;
                //Parts area
                float partsRectHeight = Mathf.Floor((availableHeight / 3) * 2) - SectionMargin;
                Rect partsRect = new Rect(0f, 0f, DisplayAreaWidth, partsRectHeight);
                Widgets.DrawBoxSolid(partsRect, BoxColor);
                DroidDesignUIHandler.DrawPartsList(partsRect, design);

                //AI area
                float aiRectHeight = availableHeight - partsRectHeight - SectionMargin;
                Rect aiRect = new Rect(0f, partsRect.yMax + 10f, partsRect.width, aiRectHeight);
                Widgets.DrawBoxSolid(aiRect, BoxColor);
                DroidDesignUIHandler.DrawAIList(aiRect, design, true);

                //Droid display area
                Rect droidDisplayRect = new Rect(partsRect.xMax + 10f, 0f, DroidDisplayAreaWidth, availableHeight);
                Widgets.DrawBoxSolid(droidDisplayRect, BoxColor);
                DrawDroidDisplay(droidDisplayRect);

                //Stats area
                Rect statsRect = new Rect(droidDisplayRect.xMax + 10f, 0f, StatsDisplayAreaWidth, partsRectHeight);
                Widgets.DrawBoxSolid(statsRect, BoxColor);

                //Cost area
                Rect costsRect = new Rect(statsRect.x, statsRect.yMax + 10f, statsRect.width, aiRectHeight);
                Widgets.DrawBoxSolid(costsRect, BoxColor);

                //Footer area
                Rect footerRect = new Rect(0f, droidDisplayRect.yMax + SectionMargin, mainRect.width, FooterRectHeight);
                DrawFooter(footerRect);
            }
            finally
            {
                GUI.EndGroup();
                GUI.EndGroup();
                Text.Anchor = TextAnchor.UpperLeft;
            }
        }

        private void DrawDroidDisplay(Rect rect)
        {
            DroidDesignUIHandler.DrawPartSelector(rect, design, true);
        }

        private void DrawFooter(Rect rect)
        {
            try
            {
                GUI.BeginGroup(rect);

                Rect cancelButtonRect = new Rect(0f, rect.height - ButtonSize.y, ButtonSize.x, ButtonSize.y);
                if (Widgets.ButtonText(cancelButtonRect, "Cancel"))
                {
                    Find.WindowStack.TryRemove(this);
                }

                Rect acceptButtonRect = new Rect(rect.width - ButtonSize.x, cancelButtonRect.y, ButtonSize.x, ButtonSize.y);
                if (Widgets.ButtonText(acceptButtonRect, "Accept"))
                {
                    if (!designName.NullOrEmpty())
                    {
                        Find.WindowStack.TryRemove(this);
                        if (isNewDesign)
                        {
                            design.Label = designName;
                            DroidManager.Instance.Designs.Add(design);
                        }
                        else
                        {
                            //TODO:: if editing existing design, apply changes
                        }
                    }
                    else
                    {
                        Messages.Message(new Message("Enter a design name", DefDatabase<MessageTypeDef>.GetNamed("RejectInput")));
                    }
                }

                //Draw cpu, power draw and max power draw bars
                float cpuY = Mathf.Floor((FooterRectHeight - (DrawBarHeight * 2)) / 3);
                Rect cpuDrawRect = new Rect(DisplayAreaWidth + SectionMargin, cpuY, DroidDisplayAreaWidth, DrawBarHeight);
                var cpuUsage = design.GetUsedCPU;
                DroidDesignUIHandler.DrawProgressBar(cpuDrawRect, cpuUsage.value, design.GetMaxCPU.value, cpuUsage, design.CPUTooltip);

                Rect powerDrawRect = new Rect(cpuDrawRect.x, cpuDrawRect.yMax + cpuY, cpuDrawRect.width, cpuDrawRect.height);
                var powerDrain = design.GetPowerDrain;
                DroidDesignUIHandler.DrawProgressBar(powerDrawRect, powerDrain.value, design.GetMaxPowerDrain.value, powerDrain, design.PowerDrainTooltip);
            }
            finally
            {
                GUI.EndGroup();
            }
        }
    }
}
