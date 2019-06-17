using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace MD3_Droids
{
    public class DroidManager : UtilityWorldObject
    {
        private List<Droid> droids = new List<Droid>();
        private List<Thing> chargers = new List<Thing>();
        private List<Blueprint> blueprints;
        private int idCount = 0;

        public List<Droid> AllDroids => droids;
        public List<Thing> AllChargers => chargers;
        public List<Blueprint> Blueprints
        {
            get
            {
                if (blueprints == null)
                    blueprints = new List<Blueprint>();
                return blueprints;
            }
        }


        public static DroidManager Instance => UtilityWorldObjectManager.GetUtilityWorldObject<DroidManager>();

        public DroidManager()
        {

        }

        public int GetUniqueID()
        {
            return idCount++;
            //TODO:: save idCount in config file so that all games have unique id's
        }

        public override void ExposeData()
        {
            base.ExposeData();
            //TODO:: implement saving designs properly
            Scribe_Collections.Look(ref blueprints, "designs", LookMode.Deep);
        }

        private void LoadDroidDesigns()
        {
            blueprints = new List<Blueprint>();
            //TODO:: Load droid designs here
        }

        public void RegisterDroid(Droid droid)
        {
            droids.Add(droid);
        }

        public void DeregisterDroid(Droid droid)
        {
            droids.Remove(droid);
        }

        public void RegisterCharger(Thing charger)
        {
            if (charger != null && charger.TryGetComp<CompDroidCharger>() != null && !chargers.Contains(charger))
            {
                chargers.Add(charger);
            }
        }

        public void DeregisterCharger(Thing charger)
        {
            if (charger != null && chargers.Contains(charger))
            {
                chargers.Remove(charger);
            }
        }

        public Thing ClosestChargerFor(ICharge droid, Map map, float distance = 9999f)
        {
            Predicate<Thing> pred = (Thing thing) => { return thing.TryGetComp<CompDroidCharger>() != null && thing.TryGetComp<CompDroidCharger>().IsAvailable(droid); };
            return GenClosest.ClosestThing_Global_Reachable(droid.Parent.Position, map, AllChargers.AsEnumerable(), PathEndMode.OnCell, TraverseParms.For(droid.Parent), distance, pred);
        }

    }
}
