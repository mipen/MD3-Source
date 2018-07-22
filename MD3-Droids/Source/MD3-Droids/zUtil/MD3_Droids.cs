using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace MD3_Droids
{
    [StaticConstructorOnStartup]
    public class MD3_Droids
    {
        static MD3_Droids()
        {
            var harmony = HarmonyInstance.Create("com.MD3.Droids");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }

}
