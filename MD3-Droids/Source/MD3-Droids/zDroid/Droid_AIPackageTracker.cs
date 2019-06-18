using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class Droid_AIPackageTracker : IExposable
    {
        private Droid droid;

        public Droid Droid => droid;

        public Droid_AIPackageTracker(Droid droid)
        {
            this.droid = droid;
        }

        public bool CapableOfWorkType(WorkTypeDef def)
        {
            foreach(var package in droid.blueprint.AIPackages)
            {
                if (package.CapableOfWorkType(def))
                    return true;
            }
            return false;
        }

        public void ExposeData()
        {
        }

        public void SpawnSetup()
        {

        }
    }
}
