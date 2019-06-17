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
        private BlueprintWindowHandler bpHandler;

        public ITab_Droid_Design()
        {
            size = new Vector2(1280f, 800f);
            labelKey = "Blueprint".Translate();
            bpHandler = new BlueprintWindowHandler(null, BlueprintHandlerState.Normal);
            bpHandler.EventClose += () => { CloseTab(); };
            //TODO:: Handle edit button event.

        }

        protected override void FillTab()
        {
            Rect mainRect = new Rect(10f, 10f, size.x - 20f, size.y - 20f);

            try
            {
                GUI.BeginGroup(mainRect);
                bpHandler.Blueprint = Droid.blueprint;
                bpHandler.DrawWindow(mainRect);
            }
            finally
            {
                GUI.EndGroup();
            }
        }
    }
}
