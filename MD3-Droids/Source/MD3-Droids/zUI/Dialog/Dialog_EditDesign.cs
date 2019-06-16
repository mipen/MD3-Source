using RimWorld;
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
        private Vector2 partsScrollPos = default(Vector2);
        private Vector2 aiScrollPos = default(Vector2);
        private Vector2 skillsScrollPos = default(Vector2);
        private ProgressBar powerBar = new ProgressBar();
        private ProgressBar cpuBar = new ProgressBar();

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

            StatsReportUtility.Reset();
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
                GUI.BeginGroup(nameRect);
                designName = Widgets.TextField(nameRect.AtZero(), designName);
                GUI.EndGroup();

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
                float leftRectsHeight = availableHeight / 3 - (SectionMargin * 2) / 3;
                Rect partsRect = new Rect(0f, 0f, DisplayAreaWidth, leftRectsHeight);
                DroidDesignUIHandler.DrawPartsList(partsRect, ref partsScrollPos, design);

                //AI area
                Rect aiRect = new Rect(0f, partsRect.yMax + SectionMargin, partsRect.width, leftRectsHeight);
                DroidDesignUIHandler.DrawAIList(aiRect, ref aiScrollPos, design, true);

                //Skills area
                Rect skillsRect = new Rect(0f, aiRect.yMax + SectionMargin, partsRect.width, leftRectsHeight);
                DroidDesignUIHandler.DrawSkillsList(skillsRect, ref skillsScrollPos, design, true);

                //Droid display area
                Rect droidDisplayRect = new Rect(partsRect.xMax + SectionMargin, 0f, DroidDisplayAreaWidth, availableHeight);
                DroidDesignUIHandler.DrawPartSelector(droidDisplayRect, design, true);

                //Stats area
                Rect statsRect = new Rect(droidDisplayRect.xMax + SectionMargin, 0f, StatsDisplayAreaWidth, leftRectsHeight * 2 + SectionMargin);
                Widgets.DrawBoxSolid(statsRect, BoxColor);
                StatsReportUtility.DrawStatsReport(statsRect, DroidDesignUIHandler.StatDummy(design));

                //Cost area
                Rect costsRect = new Rect(statsRect.x, statsRect.yMax + SectionMargin, statsRect.width, leftRectsHeight);
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

        private void DrawFooter(Rect rect)
        {
            try
            {
                GUI.BeginGroup(rect);

                Rect cancelButtonRect = new Rect(0f, rect.height - ButtonSize.y, ButtonSize.x, ButtonSize.y);
                if (Widgets.ButtonText(cancelButtonRect, "Cancel"))
                {
                    Close();
                }

                Rect acceptButtonRect = new Rect(rect.width - ButtonSize.x, cancelButtonRect.y, ButtonSize.x, ButtonSize.y);
                if (Widgets.ButtonText(acceptButtonRect, "Accept"))
                {
                    if (!designName.NullOrEmpty())
                    {
                        Close();
                        if (isNewDesign)
                        {
                            design.Label = designName;
                            DroidManager.Instance.Designs.Add(design);
                        }
                        else
                        {
                            //TODO:: if editing existing design, apply changes
                            //TODO:: Determine if design is valid, deny accepting and saving if it is not!
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
                cpuBar.DrawProgressBar(cpuDrawRect, cpuUsage.value, design.GetMaxCPU.value, cpuUsage, design.CPUTooltip);

                Rect powerDrawRect = new Rect(cpuDrawRect.x, cpuDrawRect.yMax + cpuY, cpuDrawRect.width, cpuDrawRect.height);
                var powerDrain = design.GetPowerDrain;
                powerBar.DrawProgressBar(powerDrawRect, powerDrain.value, design.GetMaxPowerDrain.value, powerDrain, design.PowerDrainTooltip);
            }
            finally
            {
                GUI.EndGroup();
            }
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            ITab_DroidDesigns.DrawStats = true;
        }
    }
}
