using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace MD3_Droids
{
    public class ThinkNode_ConditionalDroid : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn is Droid && pawn.Faction == Faction.OfPlayer;
        }
    }
}
