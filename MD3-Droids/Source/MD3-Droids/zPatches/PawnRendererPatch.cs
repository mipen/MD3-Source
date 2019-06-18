using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids.zPatches
{
    [HarmonyPatch(typeof(PawnRenderer))]
    [HarmonyPatch("BaseHeadOffsetAt")]
    public class PawnRendererPatch
    {
        [HarmonyPrefix]
        static bool BaseHeadOffsetAt(PawnRenderer __instance, ref Rot4 rotation, ref Vector3 __result)
        {
            Pawn p = (Pawn)typeof(PawnRenderer).GetField("pawn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(__instance);
            if (p is Droid)
            {
                Vector2 headOffset = new Vector2(0.09f, 0.34f);
                switch (rotation.AsInt)
                {
                    case 0:
                        __result = new Vector3(0f, 0f, headOffset.y);
                        break;
                    case 1:
                        __result = new Vector3(headOffset.x, 0f, headOffset.y);
                        break;
                    case 2:
                        __result = new Vector3(0f, 0f, headOffset.y);
                        break;
                    case 3:
                        __result = new Vector3(0f - headOffset.x, 0f, headOffset.y);
                        break;
                    default:
                        {
                            Log.Error("BaseHeadOffsetAt error in " + p);
                            __result = Vector3.zero;
                            break;
                        }
                }
                return false;
            }
            else
                return true;
        }
    }
}
