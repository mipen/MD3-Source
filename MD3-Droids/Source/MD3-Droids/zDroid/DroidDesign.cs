using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class DroidDesign : IExposable
    {
        private int id = -1;
        private string label = "";

        public int ID => id;
        public string Label { get => label; set => label = value; }

        public DroidDesign()
        {
            id = DroidManager.Instance.GetUniqueID();
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref id, "designID");
        }
    }
}
