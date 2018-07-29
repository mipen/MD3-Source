using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class DroidChassisPartRecord : BodyPartRecord
    {
        public BodyPosition bodyPosition = BodyPosition.Undefined;

        public DroidChassisPartDef defAsDroidDef
        {
            get
            {
                if (def is DroidChassisPartDef)
                    return def as DroidChassisPartDef;
                return null;
            }
        }
    }
}
