using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class HediffStageSaveable : HediffStage, IExposable
    {
        private List<StatOffsetSaveable> statOffsetSaveables = new List<StatOffsetSaveable>();
        private List<CapModSaveable> capModSaveables = new List<CapModSaveable>();

        public void ExposeData()
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                //Saving the game, build saveable lists and save them
                statOffsetSaveables = new List<StatOffsetSaveable>();
                capModSaveables = new List<CapModSaveable>();
                if (statOffsets != null && statOffsets.Count > 0)
                {
                    foreach (var so in statOffsets)
                        statOffsetSaveables.Add(new StatOffsetSaveable(so));
                }
                if (capMods != null && capMods.Count > 0)
                {
                    foreach (var cm in capMods)
                        capModSaveables.Add(new CapModSaveable(cm));
                }
                Scribe_Collections.Look(ref statOffsetSaveables, "statOffsetSaveables", LookMode.Deep);
                Scribe_Collections.Look(ref capModSaveables, "capModSaveables", LookMode.Deep);
            }
            else
            {
                Scribe_Collections.Look(ref statOffsetSaveables, "statOffsetSaveables", LookMode.Deep);
                Scribe_Collections.Look(ref capModSaveables, "capModSaveables", LookMode.Deep);

                if (statOffsetSaveables.Count > 0)
                {
                    statOffsets = new List<StatModifier>();
                    foreach (var sos in statOffsetSaveables)
                        statOffsets.Add(sos.StatModifier);
                }
                if (capModSaveables.Count > 0)
                {
                    capMods = new List<PawnCapacityModifier>();
                    foreach (var cms in capModSaveables)
                        capMods.Add(cms.PawnCapacityModifier);
                }

                statOffsetSaveables = new List<StatOffsetSaveable>();
                capModSaveables = new List<CapModSaveable>();
            }
        }
    }
}
