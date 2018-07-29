using RimWorld;
using System.Collections.Generic;
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
        public List<MD3PawnCapacityModifier> capMods = new List<MD3PawnCapacityModifier>();
        public List<StatModifier> requirements = new List<StatModifier>();
        public ThingDef partThingDef = null;
        public TierColourDef color = null;

        public string GetTooltip()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(description);
            if (statOffsets.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Stats: ");
                foreach (var offset in statOffsets)
                {
                    sb.AppendLine($"   {offset.stat.LabelCap} {(offset.value > 0 ? "+" : "-")}{offset.stat.ValueToString(offset.value)}");
                }
            }
            if (capMods.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Capacity Modifiers: ");
                foreach (var cap in capMods)
                {
                    sb.AppendLine($"   {cap.capacity.LabelCap} {(cap.offset > 0 ? "+" : "-")}{cap.offset}%");
                }
            }
            if (requirements.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Requirements: ");
                foreach(var r in requirements)
                {
                    sb.AppendLine($"   {r.stat.LabelCap} {(r.value > 0 ? "+" : "-")}{r.stat.ValueToString(r.value)}");
                }
            }
            return sb.ToString();
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (var e in base.ConfigErrors())
                yield return e;
            if (description.NullOrEmpty())
                yield return "no description";
        }

    }
}
