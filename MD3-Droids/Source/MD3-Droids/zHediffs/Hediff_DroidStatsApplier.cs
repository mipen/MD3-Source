using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class Hediff_DroidStatsApplier : Hediff
    {

        public Droid Droid => pawn as Droid;

        public override HediffStage CurStage
        {
            get
            {
                HediffStage hediffStage = new HediffStage();
                hediffStage.statOffsets = new List<StatModifier>();

                var tempList = new List<StatModifier>();

                var statOffsets = Droid.design.StatOffsets;
                if (statOffsets.Count > 0)
                {
                    tempList.AddRange(statOffsets);
                }
                var capmods = Droid.design.CapMods;
                if (capmods.Count > 0)
                {
                    foreach (var cap in capmods)
                    {
                        if (!hediffStage.capMods.Any(x => x.capacity == cap.capacity))
                        {
                            hediffStage.capMods.Add(cap);
                        }
                        else
                        {
                            var hediffCap = hediffStage.capMods.Where(x => x.capacity == cap.capacity).First();
                            hediffCap.offset = cap.offset;
                        }
                    }
                }
                var requirements = Droid.design.PartRequirements;
                if (requirements.Count > 0)
                {
                    tempList.AddRange(requirements);
                }
                var aiReqs = Droid.design.AIRequirements;
                if (aiReqs.Count > 0)
                {
                    tempList.AddRange(aiReqs);
                }

                foreach (var offset in tempList)
                {
                    if (!hediffStage.statOffsets.Any(x => x.stat == offset.stat))
                        hediffStage.statOffsets.Add(offset);
                    else
                    {
                        var stageOffset = hediffStage.statOffsets.Where(x => x.stat == offset.stat).First();
                        stageOffset.value += offset.value;
                    }
                }

                return hediffStage;
            }
        }
    }
}
