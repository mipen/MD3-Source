using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public static class BodyPartRecordExtension
    {
        public static DroidChassisPartDef GetChassisPartDef(this BodyPartRecord record)
        {
            if (record.def is DroidChassisPartDef)
                return record.def as DroidChassisPartDef;
            return null;
        }

        public static BodyPartRecord Copy(this BodyPartRecord record, BodyPartRecord parent)
        {
            BodyPartRecord newRecord;
            if (record is DroidChassisPartRecord)
            {
                newRecord = new DroidChassisPartRecord();
                DroidChassisPartRecord oldRec = record as DroidChassisPartRecord;
                ((DroidChassisPartRecord)newRecord).bodyPosition = oldRec.bodyPosition;
            }
            else
            {
                newRecord = new BodyPartRecord();
            }
            newRecord.def = record.def;
            newRecord.customLabel = record.customLabel;
            newRecord.height = record.height;
            newRecord.depth = record.depth;
            newRecord.coverage = record.coverage;
            if (parent != null)
                newRecord.parent = parent;
            if (record.parts.Count > 0)
            {
                foreach (var part in record.parts)
                {
                    newRecord.parts.Add(part.Copy(newRecord));
                }
            }
            return newRecord;
        }

        public static string OverviewString(this BodyPartRecord rec, ref StringBuilder sb, string indent)
        {
            if (sb == null)
                sb = new StringBuilder();
            sb.AppendLine($"{indent}Def: {rec.def.defName} Label:{rec.customLabel} Type: {rec.GetType().ToString()}");
            if (rec.parts.Count > 0)
            {
                foreach (var part in rec.parts)
                {
                    part.OverviewString(ref sb, indent + "-");
                }
            }
            return sb.ToString();
        }
    }
}