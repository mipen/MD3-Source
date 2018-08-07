using Verse;

namespace MD3_Droids
{
    public class GroupPackPair : IExposable
    {
        public DroidCustomiseGroupDef group = null;
        public PartCustomisePack pack = null;
        public GroupPackPair()
        {

        }
        public GroupPackPair(DroidCustomiseGroupDef group, PartCustomisePack pack)
        {
            this.group = group;
            this.pack = pack;
        }
        public void ExposeData()
        {
            Scribe_Defs.Look(ref group, "group");
            Scribe_Deep.Look(ref pack, "pack");
        }
    }
}
