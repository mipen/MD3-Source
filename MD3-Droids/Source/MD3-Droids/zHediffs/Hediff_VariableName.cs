using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class Hediff_VariableName : Hediff
    {
        public override string LabelBase
        {
            get
            {
                if (!(def is DroidUpgradeHediffDef))
                    return $"{Part.Label} {def.label}";
                else
                {
                    var upgradeDef = def as DroidUpgradeHediffDef;
                    return $"{Part.Label} {(def.label.Replace(upgradeDef.tagToRemove,""))}";
                }
            }
        }

    }
}
