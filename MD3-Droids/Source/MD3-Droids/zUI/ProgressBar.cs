using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class ProgressBar
    {

        #region Variables
        public static readonly Color Blue = new Color(0.094f, 0.592f, 0.905f, 1f);
        public static readonly Color Orange = new Color(0.874f, 0.36f, 0.066f, 1f);
        public static readonly Color Red = new Color(0.964f, 0.082f, 0.074f, 1f);

        private float curValue = 0f;
        private float workingWidth = 0f;
        #endregion


        public void DrawProgressBar(Rect rect, float value, float maxValue, StatModifier sm, string toolTip = "")
        {
            try
            {
                GUI.BeginGroup(rect);
                Rect bgRect = new Rect(0f, 0f, rect.width, rect.height);
                Widgets.DrawAltRect(bgRect);


                float percentage = GetFillPercentage(value, maxValue);
                float width = rect.width * percentage;
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

        private float GetFillPercentage(float targetValue, float maxValue)
        {
            if (curValue > maxValue)
            {
                curValue = maxValue;
                return 1f;
            }
            float difference = targetValue - curValue;
            float change = (float)Math.Pow(0.045 * (difference), 2);
            if (change < 0.04)
                change = 0.04f;
            if (difference < 0)
                change *= -1;
            curValue += change;
            Log.Message($"Change value: {change}");
            return curValue / maxValue;
        }
    }
}
