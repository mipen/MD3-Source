﻿using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    [HarmonyPatch(typeof(Pawn_HealthTracker))]
    [HarmonyPatch("NotifyPlayerOfKilled")]
    public class Pawn_HealthTrackerPatch
    {
        [HarmonyPrefix]
        static bool NotifyPlayerOfKilled(Pawn_HealthTracker __instance, ref DamageInfo? dinfo)
        {
            var pawn = (Droid)(typeof(Pawn_HealthTracker).GetField("pawn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(__instance));
            if (pawn is Droid)
            {
                Droid d = pawn as Droid;
                Log.Message("Got here");
                //TODO:: Write custom droid destroyed message
                return false;
            }
            return true;
        }
    }
}
