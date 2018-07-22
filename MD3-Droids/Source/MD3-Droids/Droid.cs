using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class Droid : Pawn, ICharge
    {
        #region Charge Properties

        private float totalCharge = 150f;
        private bool shouldUseCharge = true;

        public float TotalCharge { get => totalCharge; set => totalCharge = value; }

        public float MaxEnergy => 1000f; //TODO:: Implement total energy derived from used parts

        public bool ShouldUsePower { get => shouldUseCharge; set => shouldUseCharge = value; }

        public float EnergyUseRate => 10f;

        public bool DesiresCharge => totalCharge < MaxEnergy;

        public bool CanTryGetCharge => Active;

        public Pawn Parent => this;

        public float PowerSafeThreshold => 0.7f;

        public float PowerLowThreshold => 0.4f;

        public float PowerCriticalThreshold => 0.15f;
        #endregion

        private bool active = true;

        public bool Active { get => active; set => active = value; }

        public Droid() : base()
        {
            story = new Pawn_StoryTracker(this);
            skills = new Pawn_SkillTracker(this);
            playerSettings = new Pawn_PlayerSettings(this);
            workSettings = new Pawn_WorkSettings(this);
            workSettings.EnableAndInitialize();
            foreach (var wtd in DefDatabase<WorkTypeDef>.AllDefs)
            {
                workSettings.SetPriority(wtd, 1);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref totalCharge, "totalCharge");
            Scribe_Values.Look(ref shouldUseCharge, "shouldUseCharge");
        }

        public override void Tick()
        {
            base.Tick();
            if(ShouldUsePower)
            {
                Deplete(EnergyUseRate);
            }
            if (needs != null)
                Log.Message(needs.mood.CurLevel.ToString());
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            UtilityWorldObjectManager.GetUtilityWorldObject<DroidManager>().RegisterDroid(this);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            DroidManager.Instance.DeregisterDroid(this);
            base.Destroy(mode);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            DroidManager.Instance.DeregisterDroid(this);
            base.DeSpawn(mode);
        }

        public override string GetInspectString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine(base.GetInspectString());
            str.Append($"Current energy: {TotalCharge.ToString("0")}Wd / {MaxEnergy}Wd");
            return str.ToString();
        }

        #region Charge Methods
        public bool AddPowerDirect(float amount)
        {
            TotalCharge += amount;
            if (TotalCharge > MaxEnergy)
            {
                TotalCharge = MaxEnergy;
                return false;
            }
            return true;
        }

        public bool RemovePowerDirect(float amount)
        {
            TotalCharge -= amount;
            if (TotalCharge < 0)
            {
                TotalCharge = 0f;
                return false;
            }
            return true;
        }

        public bool Charge(float rate)
        {
            if (TotalCharge < MaxEnergy)
            {
                TotalCharge += (rate * CompPower.WattsToWattDaysPerTick);
                if (TotalCharge > MaxEnergy)
                    TotalCharge = MaxEnergy;
                return true;
            }
            return false;
        }

        public bool Deplete(float rate)
        {
            if (TotalCharge > 0)
            {
                TotalCharge -= (rate * CompPower.WattsToWattDaysPerTick);
                if (TotalCharge < 0)
                    TotalCharge = 0;
                return true;
            }
            return false;
        }
        #endregion
    }
}
