using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD3_Droids
{
    public class DroidManager : UtilityWorldObject
    {
        private List<Droid> droids = new List<Droid>();

        public List<Droid> AllDroids => droids;

        public DroidManager()
        {

        }

        public void RegisterDroid(Droid droid)
        {
            droids.Add(droid);
        }

    }
}
