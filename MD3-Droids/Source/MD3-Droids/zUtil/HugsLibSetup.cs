using Harmony;
using System.Reflection;
using Verse;

namespace MD3_Droids
{
    [StaticConstructorOnStartup]
    public class HugsLibSetup
    {
        static HugsLibSetup()
        {
            var harmony = HarmonyInstance.Create("com.MD3.Droids");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
