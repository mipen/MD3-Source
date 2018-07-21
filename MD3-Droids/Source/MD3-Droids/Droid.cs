using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class Droid : Pawn
    {
        public Droid() : base()
        {
            story = new Pawn_StoryTracker(this);
            skills = new Pawn_SkillTracker(this);
            workSettings = new Pawn_WorkSettings(this);
            workSettings.EnableAndInitialize();
            foreach (var wtd in DefDatabase<WorkTypeDef>.AllDefs)
            {
                workSettings.SetPriority(wtd, 1);
            }
        }
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

        }
    }
}
