﻿using Verse;

namespace MD3_Droids
{
    public interface ICharge
    {
        float TotalCharge { get; set; }
        float MaxEnergy { get; }
        bool AddPowerDirect(float amount);
        bool RemovePowerDirect(float amount);
        bool Charge(float rate);
        bool Deplete(float rate);
        bool ShouldUsePower { get; set; }
        float EnergyUseRate { get; }
        bool DesiresCharge { get; }
        bool CanTryGetCharge { get; }
        Pawn Parent { get; }
        float PowerSafeThreshold { get; }
        float PowerLowThreshold { get; }
        float PowerCriticalThreshold { get; }
    }
}
