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

        public List<Droid> AllDroids => droids;
        public List<Thing> AllChargers => chargers;

        public static DroidManager Instance => UtilityWorldObjectManager.GetUtilityWorldObject<DroidManager>();

        public DroidManager()
        {

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
