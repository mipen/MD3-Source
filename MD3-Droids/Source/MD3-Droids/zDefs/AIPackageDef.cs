using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class AIPackageDef : Def
    {
        public List<WorkTypeDef> workTypes = new List<WorkTypeDef>();
        public float cpuUsage = 0f;

        public bool CapableOfWorkType(WorkTypeDef def)
        {
            return workTypes.Contains(def);
        }

        public static AIPackageDef Named(string name)
        {
            return DefDatabase<AIPackageDef>.GetNamed(name);
        }
    }
}
