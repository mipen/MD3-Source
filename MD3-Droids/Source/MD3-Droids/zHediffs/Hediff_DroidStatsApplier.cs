using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class Hediff_DroidStatsApplier : HediffWithComps
    {
        public Droid Droid => pawn as Droid;
        public string label = "";
        public override bool ShouldRemove => false;
        public override bool Visible => false;
        public override string LabelBase => label;
        private int addedPartHealth = -1;
        public int AddedPartHealth { get => addedPartHealth; set => addedPartHealth = value; }

        public HediffStageSaveable Stage = null;
        public override HediffStage CurStage => Stage;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref addedPartHealth, "addedPartHealth");
            Scribe_Deep.Look(ref Stage, "Stage");
            Scribe_Values.Look(ref label, "label");
        }
    }
}
