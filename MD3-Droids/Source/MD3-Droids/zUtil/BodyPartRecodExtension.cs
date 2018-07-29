using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public static class BodyPartRecodExtension
    {
        public static DroidChassisPartDef GetChassisPartDef(this BodyPartRecord record)
        {
            if (record.def is DroidChassisPartDef)
                return record.def as DroidChassisPartDef;
            return null;
        }
    }
}
