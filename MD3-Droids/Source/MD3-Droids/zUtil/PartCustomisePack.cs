using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class PartCustomisePack : IExposable
    {
        private ChassisPoint chassisPoint = ChassisPoint.Undefined;
        private BodyPosition bodyPosition = BodyPosition.Undefined;
        private DroidChassisPartDef part = null;

        public ChassisPoint ChassisPoint => chassisPoint;
        public BodyPosition BodyPosition => bodyPosition;
        public DroidChassisPartDef Part
        {
            get
            {
                return part;
            }
            set
            {
                part = value;
            }
        }


        public PartCustomisePack(ChassisPoint cp, DroidChassisPartDef part = null, BodyPosition position = BodyPosition.Undefined)
        {
            chassisPoint = cp;
            this.part = part;
            bodyPosition = position;
        }

        public PartCustomisePack CreateCopy()
        {
            return new PartCustomisePack(ChassisPoint, Part, BodyPosition);
        }

        public void CopyFrom(PartCustomisePack pack)
        {
            chassisPoint = pack.ChassisPoint;
            bodyPosition = pack.BodyPosition;
            part = pack.Part;
        }

        public void ExposeData()
        {
            Scribe_Defs.Look(ref part, "part");
            Scribe_Values.Look(ref chassisPoint, "chassisPoint");
            Scribe_Values.Look(ref bodyPosition, "bodyPosition");
        }
    }
}
