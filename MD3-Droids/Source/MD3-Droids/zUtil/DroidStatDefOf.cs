﻿using RimWorld;
using Verse;

namespace MD3_Droids
{
    public static class DroidStatDefOf
    {
        public static StatDef CPUCapacity => DefDatabase<StatDef>.GetNamed("MD3_DroidCPUCapacity");
        public static StatDef CPUUsage => DefDatabase<StatDef>.GetNamed("MD3_DroidCPUUsage");
        public static StatDef PowerStorage => DefDatabase<StatDef>.GetNamed("MD3_DroidPowerStorage");
        public static StatDef PowerDrain => DefDatabase<StatDef>.GetNamed("MD3_DroidPowerDrain");
        public static StatDef PowerDrainMaxRate => DefDatabase<StatDef>.GetNamed("MD3_DroidPowerMaxRate");
    }
}
