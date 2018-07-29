using RimWorld;
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
        public ChassisType chassisType = ChassisType.Any;

        public bool CapableOfWorkType(WorkTypeDef def)
        {
            return workTypes.Contains(def);
        }

        public static AIPackageDef Named(string name)
        {
            return DefDatabase<AIPackageDef>.GetNamed(name);
        }

        public string Tooltip
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(description);
                sb.AppendLine();
                sb.Append($"Uses: {cpuUsage}tf");
                return sb.ToString();
            }
        }
    }
}
