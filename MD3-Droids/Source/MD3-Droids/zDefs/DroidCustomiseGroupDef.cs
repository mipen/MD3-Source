using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class DroidCustomiseGroupDef : Def
    {
        public List<CustomiseGroupListItem> Parts = new List<CustomiseGroupListItem>();

        public static DroidCustomiseGroupDef Named(string defName)
        {
            return DefDatabase<DroidCustomiseGroupDef>.GetNamed(defName);
        }
    }
}
