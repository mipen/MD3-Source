using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public static class ThingDefExt
    {
        public static ThingDef Clone(this ThingDef thingDef)
        {
            return (ThingDef)(typeof(ThingDef).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(thingDef, null));
        }
    }
}
