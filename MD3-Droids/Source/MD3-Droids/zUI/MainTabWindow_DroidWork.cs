using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public class MainTabWindow_DroidWork : MainTabWindow_Work
    {
        public MainTabWindow_DroidWork()
        {

        }

        protected override IEnumerable<Pawn> Pawns
        {
            get
            {
                List<Droid> droids = UtilityWorldObjectManager.GetUtilityWorldObject<DroidManager>().AllDroids;
                Log.Message(droids.Count.ToString());
                foreach (var d in droids)
                {
                    yield return (Pawn)d;
                }
            }
        }

    }
}
