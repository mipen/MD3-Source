using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public static class DroidGenerator
    {
        public static Droid GenerateDroid(PawnKindDef kindDef, DroidDesign design, Faction faction = null)
        {
            try
            {
                Droid d = (Droid)PawnGenerator.GeneratePawn(kindDef, faction);
                d.design = design;
                d.InitialiseFromDesign();
                return d;
            }
            catch (Exception ex)
            {
                Log.Error($"Error generating droid: {ex}");
                return null;
            }
        }
    }
}
