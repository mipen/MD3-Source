using Harmony;
using System.Reflection;
using Verse;

namespace MD3_Droids
{
    
    public class MD3_DroidsMod : Mod
    {
        public static MD3_DroidsMod Instance;

        public MD3_DroidsMod(ModContentPack pack) : base(pack)
        {
            Instance = this;
        }

    }

}
