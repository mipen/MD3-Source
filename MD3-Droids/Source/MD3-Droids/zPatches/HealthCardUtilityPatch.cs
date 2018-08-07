using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids.zPatches
{
    [HarmonyPatch(typeof(HealthCardUtility))]
    [HarmonyPatch("DrawOverviewTab")]
    public class DrawOverviewTabPatch
    {
        [HarmonyPrefix]
        static bool DrawOverviewTab(ref Rect leftRect, ref Pawn pawn, ref float curY)
        {
            if (pawn is Droid)
            {
                curY += 4f;
                Pawn p = pawn;
                Text.Font = GameFont.Small;
                if (!pawn.Dead)
                {
                    IEnumerable<PawnCapacityDef> source = pawn.def.race.Humanlike ? (from x in DefDatabase<PawnCapacityDef>.AllDefs
                                                                                     where x.showOnHumanlikes
                                                                                     select x) : ((!pawn.def.race.Animal) ? DefDatabase<PawnCapacityDef>.AllDefs.Where((PawnCapacityDef x) => x.showOnMechanoids) : DefDatabase<PawnCapacityDef>.AllDefs.Where((PawnCapacityDef x) => x.showOnAnimals));
                    foreach (PawnCapacityDef item in from act in source
                                                     orderby act.listOrder
                                                     select act)
                    {
                        if (PawnCapacityUtility.BodyCanEverDoCapacity(pawn.RaceProps.body, item))
                        {
                            PawnCapacityDef activityLocal = item;
                            Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, item);
                            Func<string> textGetter = () => (!p.Dead) ? HealthCardUtility.GetPawnCapacityTip(p, activityLocal) : "";
                            curY = (float)(typeof(HealthCardUtility).GetMethod("DrawLeftRow",BindingFlags.NonPublic|BindingFlags.Static).Invoke(new object(), new object[] { leftRect, curY, item.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, new TipSignal(textGetter, pawn.thingIDNumber ^ item.index) }));
                        }
                    }
                }
                return false;
            }
            else
                return true;
        }
    }
}
