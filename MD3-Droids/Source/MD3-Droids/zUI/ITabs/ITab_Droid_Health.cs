using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class ITab_Droid_Health : ITab_Pawn_Health
    {
        private Pawn PawnForHealth
        {
            get
            {
                if (SelPawn == null)
                {
                    return (SelThing as Corpse)?.InnerPawn;
                }
                return SelPawn;
            }
        }

        protected override void FillTab()
        {
            Pawn pawnForHealth = PawnForHealth;
            if (pawnForHealth == null)
            {
                Log.Error("Health tab found no selected pawn to display.", false);
            }
            else
            {
                Corpse corpse = base.SelThing as Corpse;
                bool showBloodLoss = corpse == null || corpse.Age < 60000;
                Rect outRect = new Rect(0f, 20f, size.x, size.y - 20f);
                HealthCardUtility.DrawPawnHealthCard(outRect, pawnForHealth, false, showBloodLoss, base.SelThing);
            }
        }
    }
}
