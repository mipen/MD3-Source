using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class ITab_Droid_Design : ITab
    {
        private static readonly Color BoxColor = new Color(0.1098f, 0.1294f, 0.149f);

        private const float SectionMargin = 10f;
        private const float DroidDisplayWidth = 420f;
        private const float DisplayAreaWidth = 300f;

        public Droid Droid => SelPawn as Droid;

        public ITab_Droid_Design()
        {
            size = new Vector2(730f+20f, 800f);
            labelKey = "Design";
        }

        protected override void FillTab()
        {
            Rect baseRect = new Rect(0f, 20f, size.x, size.y-20f);
            Rect mainRect = new Rect(baseRect.ContractedBy(10f));

            try
            {
                GUI.BeginGroup(mainRect);

                Rect displayRect = new Rect(0f, 0f, DisplayAreaWidth, mainRect.height);
                Widgets.DrawBoxSolid(displayRect, BoxColor);
                Widgets.DrawBox(displayRect);

                Rect droidDisplayRect = new Rect(displayRect.xMax + SectionMargin, 0f, DroidDisplayWidth, mainRect.height);
                DroidDesignUIHandler.DrawPartSelector(droidDisplayRect, Droid.design, false);
            }
            finally
            {
                GUI.EndGroup();
            }
        }
    }
}
