using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids.zPatches
{
    [HarmonyPatch(typeof(HediffSet))]
    [HarmonyPatch("GetPartHealth")]
    public class HediffSetPatch
    {
        [HarmonyPrefix]
        static bool GetPartHealth(HediffSet __instance, ref BodyPartRecord part, ref float __result)
        {
            if (part != null && part.def is DroidChassisPartDef)
            {
                DroidChassisPartDef pd = part.def as DroidChassisPartDef;
                float num = part.def.GetMaxHealth(__instance.pawn);
                for (int i = 0; i < __instance.hediffs.Count; i++)
                {
                    if (__instance.hediffs[i] is Hediff_MissingPart && __instance.hediffs[i].Part == part)
                    {
                        return true;
                    }
                    if (__instance.hediffs[i].Part == part)
                    {
                        Hediff_Injury hediff_Injury = __instance.hediffs[i] as Hediff_Injury;
                        if (hediff_Injury != null)
                        {
                            num -= hediff_Injury.Severity;
                        }

                        Hediff_DroidStatsApplier droid_stats = __instance.hediffs[i] as Hediff_DroidStatsApplier;
                        if (droid_stats != null)
                        {
                            if (droid_stats.AddedPartHealth > 0)
                                num += droid_stats.AddedPartHealth;
                        }
                    }
                }
                if (num < 0f)
                {
                    num = 0f;
                }
                __result = (float)Mathf.RoundToInt(num);
                return false;
            }
            else
                return true;
        }
    }
}
