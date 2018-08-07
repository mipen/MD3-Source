using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace MD3_Droids
{
    public class StatOffsetSaveable : IExposable
    {
        public StatDef stat = null;
        public float value = 0;

        public StatModifier StatModifier
        {
            get
            {
                return new StatModifier()
                {
                    stat = stat,
                    value = value
                };
            }
        }

        public StatOffsetSaveable()
        {

        }

        public StatOffsetSaveable(StatModifier so)
        {
            stat = so.stat;
            value = so.value;
        }

        public void ExposeData()
        {
            Scribe_Defs.Look(ref stat, "statDef");
            Scribe_Values.Look(ref value, "statValue");
        }
    }
}
