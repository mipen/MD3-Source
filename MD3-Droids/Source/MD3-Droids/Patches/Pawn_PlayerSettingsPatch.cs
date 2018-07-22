using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD3_Droids
{
    [HarmonyPatch(typeof(Pawn_PlayerSettings))]
    [HarmonyPatch("ResetMedicalCare")]
    class PlayerSettingsPatch
    {
        [HarmonyPrefix]
        static bool ResetMedicalCare(Pawn_PlayerSettings __instance)
        {
            if ((typeof(Pawn_PlayerSettings).GetField("pawn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(__instance)) is Droid)
            {
                return false;
            }
            return true;
        }
    }
}
