using RimWorld;
using System;
using Verse;

namespace MD3_Droids
{
    public static class DroidGenerator
    {
        public static Droid GenerateDroid(PawnKindDef kindDef, Blueprint bp, Faction faction = null)
        {
            try
            {
                Droid d = (Droid)PawnGenerator.GeneratePawn(kindDef, faction);
                d.blueprint = bp;
                d.InitialiseFromBlueprint();
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
