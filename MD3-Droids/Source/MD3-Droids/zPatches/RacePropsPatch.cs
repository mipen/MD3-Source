using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{

    [HarmonyPatch(typeof(RaceProperties))]
    [HarmonyPatch("IsFlesh", PropertyMethod.Getter)]
    class IsFleshPatch
    {
        [HarmonyPrefix]
        static bool IsFlesh(RaceProperties __instance, ref bool __result)
        {
            if (__instance.FleshType == DefDatabase<FleshTypeDef>.GetNamed("MD3_Droid"))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

}
