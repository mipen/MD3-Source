using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public static class BodyDefExt
    {
        public static BodyDef Copy(this BodyDef def, string defName)
        {
            BodyDef newDef = new BodyDef();

            newDef.defName = defName;
            newDef.label = def.label;
            newDef.description = def.description;
            newDef.corePart = def.corePart.Copy(null);
            newDef.ResolveReferences();
            return newDef;
        }

        public static string OverviewString(this BodyDef def)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(def.defName);
            sb.AppendLine(def.label);
            sb.AppendLine("=================");
            sb.AppendLine("Parts: ");
            def.corePart.OverviewString(ref sb, "-");
            return sb.ToString();
        }
    }
}
