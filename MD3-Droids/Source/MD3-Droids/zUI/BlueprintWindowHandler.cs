using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class BlueprintWindowHandler
    {
        private BlueprintHandlerState state = BlueprintHandlerState.Normal;

        private Blueprint blueprint;
        public Blueprint Blueprint => blueprint;

        private static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);

        //private const float DroidDisplayAreaWidth = 420f;
        //private const float StatsDisplayAreaWidth = 300f;
        //private const float DisplayAreaWidth = 300f;
        private float PartSelectorAreaWidth = 0f;
        private float StatsAreaWidth = 0f;
        private float DisplayAreaWidth = 0f;
        private const float SectionMargin = 10f;
        private const float HeaderRectHeight = 45f;
        private const float TextBoxHeight = 28f;
        private const float TextBoxWidth = 200f;
        private const float FooterRectHeight = 100f;
        private const float DrawBarHeight = 25f;
        private static readonly Vector2 ButtonSize = new Vector2(120f, 30f);
        private Vector2 partsScrollPos = default(Vector2);
        private Vector2 aiScrollPos = default(Vector2);
        private Vector2 skillsScrollPos = default(Vector2);
        private ProgressBar powerBar = new ProgressBar();
        private ProgressBar cpuBar = new ProgressBar();

        public delegate void Close();
        public event Close EventClose;
        public delegate void Cancel();
        public event Cancel EventCancel;
        public delegate void Accept();
        public event Accept EventAccept;


        public BlueprintWindowHandler(Blueprint blueprint, BlueprintHandlerState state)
        {
            this.blueprint = blueprint;
            this.state = state;

            StatsReportUtility.Reset();
        }

        public void DrawWindow(Rect inRect)
        {
            try
            {
                GUI.BeginGroup(inRect);

                #region Header
                //Header
                Rect headerRect = new Rect(0f, 0f, inRect.width, HeaderRectHeight);
                try
                {
                    GUI.BeginGroup(headerRect);
                    //Draw the label
                    string nameLabelString;
                    float nameLabelXPos;
                    float nameLabelWidth;

                    if (state == BlueprintHandlerState.Edit || state == BlueprintHandlerState.New)
                    {
                        nameLabelString = "DesignName".Translate();
                        nameLabelWidth = Text.CalcSize(nameLabelString).x;
                        nameLabelXPos = headerRect.width / 2 - (nameLabelWidth + 5f + TextBoxWidth) / 2;
                    }
                    else
                    {
                        nameLabelString = "DesignName".Translate() + blueprint.Label;
                        nameLabelWidth = Text.CalcSize(nameLabelString).x;
                        nameLabelXPos = headerRect.width / 2 - nameLabelWidth / 2;
                    }

                    Rect nameLabelRect = new Rect(nameLabelXPos, 0f, nameLabelWidth, HeaderRectHeight);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Widgets.Label(nameLabelRect, nameLabelString);
                    Text.Anchor = TextAnchor.UpperLeft;

                    //if the state is New or Edit, draw the textbox.
                    if (state == BlueprintHandlerState.Edit || state == BlueprintHandlerState.New)
                    {
                        Rect nameRect = new Rect(nameLabelRect.xMax + 5f, HeaderRectHeight / 2 - TextBoxHeight / 2, 200f, TextBoxHeight);
                        blueprint.Label = Widgets.TextField(nameRect, blueprint.Label);
                    }

                }
                finally
                {
                    GUI.EndGroup();
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                #endregion

                #region Main Rect
                //MainRect
                Rect mainRect = new Rect(0f, headerRect.yMax + SectionMargin, inRect.width, inRect.height - HeaderRectHeight - FooterRectHeight - SectionMargin * 2);
                try
                {
                    GUI.BeginGroup(mainRect);

                    PartSelectorAreaWidth = (float)Math.Floor(mainRect.width * 0.45);
                    StatsAreaWidth = (float)Math.Floor(mainRect.width * 0.30);
                    DisplayAreaWidth = (float)Math.Floor(mainRect.width * 0.25);

                    //Parts list
                    float leftRectsHeight = mainRect.height / 3 - (SectionMargin * 2) / 3;
                    Rect partsRect = new Rect(0f, 0f, DisplayAreaWidth, leftRectsHeight);
                    BlueprintUIUtil.DrawPartsList(partsRect, ref partsScrollPos, blueprint);

                    //AI list
                    Rect aiRect = new Rect(0f, partsRect.yMax + SectionMargin, partsRect.width, leftRectsHeight);
                    BlueprintUIUtil.DrawAIList(aiRect, ref aiScrollPos, blueprint, true);

                    //Skills
                    Rect skillsRect = new Rect(0f, aiRect.yMax + SectionMargin, partsRect.width, leftRectsHeight);
                    BlueprintUIUtil.DrawSkillsList(skillsRect, ref skillsScrollPos, blueprint, true);

                    //Droid part selector
                    Rect droidDisplayRect = new Rect(partsRect.xMax + SectionMargin, 0f, PartSelectorAreaWidth, mainRect.height);
                    BlueprintUIUtil.DrawPartSelector(droidDisplayRect, blueprint, true);

                    //Stats 
                    Rect statsRect = new Rect(droidDisplayRect.xMax + SectionMargin, 0f, StatsAreaWidth, leftRectsHeight * 2 + SectionMargin);
                    Widgets.DrawBoxSolid(statsRect, BoxColor);
                    StatsReportUtility.DrawStatsReport(statsRect, BlueprintUIUtil.StatDummy(blueprint));

                    //Costs 
                    Rect costsRect = new Rect(statsRect.x, statsRect.yMax + SectionMargin, statsRect.width, leftRectsHeight);
                    Widgets.DrawBoxSolid(costsRect, BoxColor);

                }
                finally
                {
                    GUI.EndGroup();
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                #endregion

                #region Footer
                //Footer area
                Rect footerRect = new Rect(0f, inRect.height - FooterRectHeight, mainRect.width, FooterRectHeight);
                DrawFooter(footerRect);
                #endregion
            }
            finally
            {
                GUI.EndGroup();
                Text.Anchor = TextAnchor.UpperLeft;
            }
        }

        private void DrawFooter(Rect rect)
        {
            try
            {
                GUI.BeginGroup(rect);

                if (state == BlueprintHandlerState.Normal)
                {
                    Rect closeButtonRect = new Rect(0f, rect.height / 2 - ButtonSize.y / 2, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(closeButtonRect, "Close".Translate()))
                    {
                        EventClose?.Invoke();
                    }
                }
                else if (state == BlueprintHandlerState.New || state == BlueprintHandlerState.Edit)
                {
                    Rect cancelButtonRect = new Rect(0f, rect.height / 2 - ButtonSize.y / 2, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(cancelButtonRect, "Cancel".Translate()))
                    {
                        EventCancel?.Invoke();
                    }

                    Rect acceptButtonRect = new Rect(rect.width - ButtonSize.x, cancelButtonRect.y, ButtonSize.x, ButtonSize.y);
                    if (Widgets.ButtonText(acceptButtonRect, "Accept".Translate()))
                    {
                        EventAccept?.Invoke();
                    }
                }

                //Draw cpu, power draw and max power draw bars
                float cpuY = Mathf.Floor((FooterRectHeight - (DrawBarHeight * 2)) / 3);
                Rect cpuDrawRect = new Rect(DisplayAreaWidth + SectionMargin, cpuY, PartSelectorAreaWidth, DrawBarHeight);
                var cpuUsage = blueprint.GetUsedCPU;
                cpuBar.DrawProgressBar(cpuDrawRect, cpuUsage.value, blueprint.GetMaxCPU.value, cpuUsage, blueprint.CPUTooltip);

                Rect powerDrawRect = new Rect(cpuDrawRect.x, cpuDrawRect.yMax + cpuY, cpuDrawRect.width, cpuDrawRect.height);
                var powerDrain = blueprint.GetPowerDrain;
                powerBar.DrawProgressBar(powerDrawRect, powerDrain.value, blueprint.GetMaxPowerDrain.value, powerDrain, blueprint.PowerDrainTooltip);
            }
            finally
            {
                GUI.EndGroup();
            }
        }


    }
}
