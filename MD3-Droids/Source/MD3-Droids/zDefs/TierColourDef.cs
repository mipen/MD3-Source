using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class TierColourDef : Def
    {
        public float r = 0f;
        public float g = 0f;
        public float b = 0f;
        public float a = 1f;

        public Color GetColor()
        {
            return new Color(r, g, b, a);
        }

        public static Color Named(string defName)
        {
            var def = DefDatabase<TierColourDef>.GetNamed(defName);
            return new Color(def.r, def.g, def.b, def.a);
        }

    }
}
