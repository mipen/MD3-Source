using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class PawnColumnWorker_DroidWorkPriority : PawnColumnWorker_WorkPriority
    {
        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            if (pawn is Droid)
            {
                Droid droid = pawn as Droid;
                if (!pawn.Dead && pawn.workSettings != null && pawn.workSettings.EverWork)
                {

                    Text.Font = GameFont.Medium;
                    float x = rect.x + (rect.width - 25f) / 2f;
                    float y = rect.y + 2.5f;
                    bool incapable = false;// IsIncapableOfWholeWorkType(pawn, def.workType);
                    if (droid.aiPackages.CapableOfWorkType(def.workType))
                        WidgetsWork.DrawWorkBoxFor(x, y, pawn, def.workType, incapable);
                    Rect rect2 = new Rect(x, y, 25f, 25f);
                    TooltipHandler.TipRegion(rect2, () => WidgetsWork.TipForPawnWorker(pawn, def.workType, incapable), pawn.thingIDNumber ^ def.workType.GetHashCode());
                    Text.Font = GameFont.Small;
                }
            }
        }
    }
}
