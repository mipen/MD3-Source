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
        private bool capableOfViolence = true;
        private List<AIPackageDef> aIPackageDefs = new List<AIPackageDef>();

        public Droid Droid => droid;

        public Droid_AIPackageTracker(Droid droid)
        {
            this.droid = droid;
        }

        public void AddPackage(AIPackageDef def)
        {
            if (!aIPackageDefs.Contains(def))
                aIPackageDefs.Add(def);
        }

        public bool CapableOfWorkType(WorkTypeDef def)
        {
            foreach(var package in aIPackageDefs)
            {
                if (package.CapableOfWorkType(def))
                    return true;
            }
            return false;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref capableOfViolence, "capableOfViolence");
            Scribe_Collections.Look(ref aIPackageDefs, "aiPackageDefs", LookMode.Def);
        }

        public void SpawnSetup()
        {
            foreach(var aiDef in aIPackageDefs)
            {
                if(aiDef.workTypes.Count>0)
                {
                    foreach(var wDef in aiDef.workTypes)
                    {
                        if(wDef.relevantSkills.Count>0)
                        {
                            foreach (var skill in wDef.relevantSkills)
                                droid.skills.GetSkill(skill).Level = 20;
                        }
                    }
                }
            }
        }
    }
}
