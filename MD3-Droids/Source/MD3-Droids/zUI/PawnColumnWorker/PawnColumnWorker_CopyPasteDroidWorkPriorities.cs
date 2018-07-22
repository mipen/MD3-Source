using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class PawnColumnWorker_CopyPasteDroidWorkPriorities : PawnColumnWorker_CopyPasteWorkPriorities
    {
        private static DefMap<WorkTypeDef, int> clipboard;

        protected override bool AnythingInClipboard => clipboard != null;

        protected override void CopyFrom(Pawn p)
        {
            if (clipboard == null)
            {
                clipboard = new DefMap<WorkTypeDef, int>();
            }
            List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
            for (int i = 0; i < allDefsListForReading.Count; i++)
            {
                WorkTypeDef workTypeDef = allDefsListForReading[i];
                clipboard[workTypeDef] = (p.story.WorkTypeIsDisabled(workTypeDef) ? 3 : p.workSettings.GetPriority(workTypeDef));
            }
        }

        protected override void PasteTo(Pawn p)
        {
            if (p is Droid)
            {
                Droid d = p as Droid;
                List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
                for (int i = 0; i < allDefsListForReading.Count; i++)
                {
                    WorkTypeDef workTypeDef = allDefsListForReading[i];
                    if (d.aiPackages.CapableOfWorkType(workTypeDef))
                    {
                        p.workSettings.SetPriority(workTypeDef, clipboard[workTypeDef]);
                    }
                }
            }
        }
    }
}
