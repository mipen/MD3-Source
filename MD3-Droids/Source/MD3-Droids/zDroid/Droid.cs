using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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

        public float MaxEnergy => this.GetStatValue(DroidStatDefOf.PowerStorage);

        public bool ShouldUsePower { get => shouldUseCharge; set => shouldUseCharge = value; }

        public float EnergyUseRate => this.GetStatValue(DroidStatDefOf.PowerDrain);

        public float EnergyUseRateMax => this.GetStatValue(DroidStatDefOf.PowerDrainMaxRate);

        public bool DesiresCharge => totalCharge < MaxEnergy;

        public bool CanTryGetCharge => Active;

        public Pawn Parent => this;

        public float PowerSafeThreshold => 0.7f;

        public float PowerLowThreshold => 0.4f;

        public float PowerCriticalThreshold => 0.15f;
        #endregion

        private bool active = true;
        public DroidDesign design;
        public Droid_WorkManager workManager;

        public bool Active { get => active; set => active = value; }

        public Droid_AIPackageTracker aiPackages;

        public Droid() : base()
        {
            design = new DroidDesign();
            story = new Pawn_StoryTracker(this);
            skills = new Pawn_SkillTracker(this);
            playerSettings = new Pawn_PlayerSettings(this);
            workSettings = new Pawn_WorkSettings(this);
            workSettings.EnableAndInitialize();
            foreach (var wtd in DefDatabase<WorkTypeDef>.AllDefs)
            {
                workSettings.SetPriority(wtd, 1);
            }
            aiPackages = new Droid_AIPackageTracker(this);
            workManager = new Droid_WorkManager(this);
            playerSettings.medCare = MedicalCareCategory.NoCare;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref design, "design");
            Scribe_Values.Look(ref totalCharge, "totalCharge");
            Scribe_Values.Look(ref shouldUseCharge, "shouldUseCharge");
            Scribe_Deep.Look(ref aiPackages, "aiPackages", this);
        }

        public override void Tick()
        {
            base.Tick();
            if (ShouldUsePower)
            {
                Deplete(EnergyUseRate);
            }
        }

        public void InitialiseFromDesign()
        {
            design.Recache();
            design.AddHediffsToDroid(this);
            aiPackages.SpawnSetup();
        }

        public List<BodyPartRecord> GetBodyPartRecords(ChassisPoint cp, BodyPosition bp)
        {
            return (from t in def.race.body.AllParts
                    where t.def is DroidChassisPartDef && ((DroidChassisPartDef)t.def).ChassisPoint == cp && (!(t is DroidChassisPartRecord) || ((DroidChassisPartRecord)t).bodyPosition == bp)
                    select t).ToList();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            DroidManager.Instance.RegisterDroid(this);
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
            str.Append($"Current energy: {TotalCharge.ToString("0")}Wd / {MaxEnergy}Wd, Drain: {EnergyUseRate}W");
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
