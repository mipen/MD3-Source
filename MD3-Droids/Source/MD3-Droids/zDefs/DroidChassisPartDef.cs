using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class DroidChassisPartDef : BodyPartDef
    {
        public ChassisPoint ChassisPoint = ChassisPoint.Undefined;
        public ChassisType ChassisType = ChassisType.Any;
        public bool BasePart = false;
        public bool allowBasePartsInSameChassisPoint = false;
        public List<StatModifier> statOffsets = new List<StatModifier>();
        public ThingDef partThingDef = null;
        public TierColourDef color = null;

        public string GetTooltip()
        {
            return description;//TODO:: include stat offsets here
        }

        //public override IEnumerable<string> ConfigErrors()
        //{
        //    if (BasePart && !allowBasePartsInSameChassisPoint)
        //    {
        //        var list = DefDatabase<DroidChassisPartDef>.AllDefsListForReading.Where(x => x!=this && x.BasePart == true && x.ChassisType == this.ChassisType && x.ChassisPoint == ChassisPoint);
        //        if (list.Any())
        //        {
        //            yield return $"{defName} has config errors: def set as base part, but there are other defs set as base part for the same chassis type and chassis point.";
        //            foreach (var d in list)
        //                yield return d.defName;
        //        }
        //    }

        //    foreach (var e in base.ConfigErrors())
        //    {
        //        yield return e;
        //    }
        //}
    }
}
