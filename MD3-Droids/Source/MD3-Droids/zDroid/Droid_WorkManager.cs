using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD3_Droids
{
    public class Droid_WorkManager
    {
        private Droid droid;
        private List<WorkGiver> workGiversInOrderCache = null;
        private List<WorkGiver> workGiversInOrderEmergencyCache = null;

        public Droid Droid => droid;

        public Droid_WorkManager(Droid droid)
        {
            this.droid = droid;
        }

        public List<WorkGiver> WorkGiversInOrder
        {
            get
            {
                if (workGiversInOrderCache == null)
                {
                    List<WorkGiver> list = new List<WorkGiver>();

                    foreach (var workGiver in droid.workSettings.WorkGiversInOrderNormal)
                    {
                        if (Droid.aiPackages.CapableOfWorkType(workGiver.def.workType))
                        {
                            list.Add(workGiver);
                        }
                    }
                    workGiversInOrderCache = list;
                }
                return workGiversInOrderCache;
            }
        }

        public List<WorkGiver> WorkGiversInOrderEmergency
        {
            get
            {
                if (workGiversInOrderEmergencyCache == null)
                {
                    List<WorkGiver> list = new List<WorkGiver>();

                    foreach (var workGiver in droid.workSettings.WorkGiversInOrderEmergency)
                    {
                        if (Droid.aiPackages.CapableOfWorkType(workGiver.def.workType))
                        {
                            list.Add(workGiver);
                        }
                    }

                    workGiversInOrderEmergencyCache = list;
                }
                return workGiversInOrderEmergencyCache;
            }
        }
    }
}
