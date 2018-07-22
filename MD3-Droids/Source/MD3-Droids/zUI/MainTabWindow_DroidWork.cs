using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class MainTabWindow_DroidWork : MainTabWindow_Work
    {
        private static bool generatedDefs = false;
        protected override PawnTableDef PawnTableDef => DefDatabase<PawnTableDef>.GetNamed("MD3_DroidWork");

        public MainTabWindow_DroidWork()
        {
            if (!generatedDefs)
            {
                foreach (var def in GenerateImpliedDefs())
                {
                    DefGenerator.AddImpliedDef(def);
                }
                generatedDefs = true;
            }
        }

        private IEnumerable<PawnColumnDef> GenerateImpliedDefs()
        {
            PawnTableDef workTable = DefDatabase<PawnTableDef>.GetNamed("MD3_DroidWork");
            bool moveWorkTypeLabelDown2 = false;
            using (IEnumerator<WorkTypeDef> enumerator2 = (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
                                                           where d.visible
                                                           select d).Reverse().GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    WorkTypeDef def = enumerator2.Current;
                    moveWorkTypeLabelDown2 = !moveWorkTypeLabelDown2;
                    PawnColumnDef d2 = new PawnColumnDef
                    {
                        defName = "DroidWorkPriority_" + def.defName,
                        workType = def,
                        moveWorkTypeLabelDown = moveWorkTypeLabelDown2,
                        workerClass = typeof(PawnColumnWorker_DroidWorkPriority),
                        sortable = true,
                        modContentPack = def.modContentPack
                    };
                    workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, d2);
                    yield return d2;
                }
            }
        }
        protected override IEnumerable<Pawn> Pawns
        {
            get
            {
                List<Droid> droids = UtilityWorldObjectManager.GetUtilityWorldObject<DroidManager>().AllDroids;
                foreach (var d in droids)
                {
                    yield return (Pawn)d;
                }
            }
        }

    }
}
