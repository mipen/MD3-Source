using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class CapModSaveable : IExposable
    {
        public PawnCapacityDef capacity = null;
        public float offset = 0;
        public float setMax = 0;
        public float postFactor = 0;

        public PawnCapacityModifier PawnCapacityModifier
        {
            get
            {
                return new PawnCapacityModifier()
                {
                    capacity = capacity,
                    offset = offset,
                    setMax = setMax,
                    postFactor = postFactor
                };
            }
        }

        public CapModSaveable()
        {

        }

        public CapModSaveable(PawnCapacityModifier cm)
        {
            capacity = cm.capacity;
            offset = cm.offset;
            setMax = cm.setMax;
            postFactor = cm.postFactor;
        }

        public void ExposeData()
        {
            Scribe_Defs.Look(ref capacity, "capacityDef");
            Scribe_Values.Look(ref offset, "offset");
            Scribe_Values.Look(ref setMax, "setMax");
            Scribe_Values.Look(ref postFactor, "postFactor");
        }
    }
}
