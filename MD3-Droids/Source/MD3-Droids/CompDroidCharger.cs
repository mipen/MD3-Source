using RimWorld;
using Verse;

namespace MD3_Droids
{
    public class CompDroidCharger : ThingComp
    {
        private ICharge chargee;
        private bool initialised = false;

        private CompPowerTrader Power
        {
            get
            {
                return parent.TryGetComp<CompPowerTrader>();
            }
        }

        private bool CanUse(ICharge d)
        {
            return chargee == null || chargee == d;
        }

        public bool IsAvailable(ICharge d)
        {
            return CanUse(d) && (Power != null && Power.PowerOn);
        }

        public void BeginCharge(ICharge d)
        {
            chargee = d;
            if (Power != null)
            {
                Power.powerOutputInt = -Power.Props.basePowerConsumption;
            }
        }

        public void EndCharge()
        {
            chargee = null;
            if (Power != null)
            {
                Power.powerOutputInt = -10f;
            }
        }

        public ICharge Chargee
        {
            get
            {
                return chargee;
            }
        }

        private void Destroy()
        {
            if (chargee != null)
                chargee.Parent.jobs.EndCurrentJob(Verse.AI.JobCondition.Incompletable);
            DroidManager.Instance.DeregisterCharger(parent);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            Destroy();
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            Destroy();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!initialised)
            {
                EndCharge();
                initialised = true;
            }
            DroidManager.Instance.RegisterCharger(parent);
            if (Power != null)
            {
                Power.powerOutputInt = -10f;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            Log.Message("compTick");
            if (chargee != null)
            {
                Log.Message("chargee not null");
                if (Power != null && Power.PowerOn)
                {
                    Log.Message("power on");
                    Chargee.Charge(Power.Props.basePowerConsumption * 2f);
                }
            }
        }

    }
}
