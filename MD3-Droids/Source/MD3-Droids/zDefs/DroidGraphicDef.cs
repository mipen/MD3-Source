using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class DroidGraphicDef : Def
    {
        public string name = "";
        public ChassisType chassisType;
        public DroidGraphicType graphicType;
        private Graphic_Multi _graphic = null;

        private string GetPath()
        {
            string path = "";
            if (chassisType == ChassisType.Small)
                path = "Droids/DroidSmall";
            else if (chassisType == ChassisType.Medium)
                path = "Droids/DroidMedium";
            else if (chassisType == ChassisType.Large)
                path = "Droids/DroidLarge";

            if (graphicType == DroidGraphicType.Head)
                path = $"{path}/Heads";
            else
                path = $"{path}/Bodies";

            return $"{path}/{name}";
        }

        public Graphic_Multi GetGraphic()
        {
            if (_graphic == null)
                _graphic = (Graphic_Multi)GraphicDatabase.Get(typeof(Graphic_Multi), GetPath(), ShaderDatabase.Cutout, new Vector2(1.5f, 1.5f), Color.white, Color.white);
            return _graphic;
        }
    }
}
