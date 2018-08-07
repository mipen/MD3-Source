using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class DroidDesign : IExposable
    {
        private int id = -1;
        public string Label = "";
        public int VersionNumber = 1;

        //private Color color;
        //private Graphic_Multi bodyGraphic;
        //private Graphic_Multi headGraphic;

        //TODO:: implement skills list
        private ChassisType chassisType = ChassisType.Medium;
        private List<AIPackageDef> aiPackages = new List<AIPackageDef>();
        private Dictionary<DroidCustomiseGroupDef, List<PartCustomisePack>> partsGrouped = new Dictionary<DroidCustomiseGroupDef, List<PartCustomisePack>>();
        private List<GroupPackPair> partsCondensed = new List<GroupPackPair>();
        public bool CapableOfViolence = false;


        public int ID => id;
        public ChassisType ChassisType => chassisType;
        public List<PartCustomisePack> Parts
        {
            get
            {
                List<PartCustomisePack> list = new List<PartCustomisePack>();
                foreach (var l in partsGrouped.Values)
                {
                    list.AddRange(l);
                }
                return list;
            }
        }
        public Dictionary<DroidCustomiseGroupDef, List<PartCustomisePack>> PartsGrouped => partsGrouped;
        public List<AIPackageDef> AIPackages => aiPackages;
        public BodyDef BaseBodyDef
        {
            get
            {
                switch (ChassisType)
                {
                    case ChassisType.Small:
                        return DefDatabase<BodyDef>.GetNamed("");
                    case ChassisType.Medium:
                        return DefDatabase<BodyDef>.GetNamed("MD3_MediumDroid");
                    case ChassisType.Large:
                        return DefDatabase<BodyDef>.GetNamed("");
                    default:
                        throw new ArgumentException($"No body def found for {ChassisType}");

                }
            }
        }
        public bool HasModifiedBody
        {
            get
            {
                return Parts.Any(x => x.Part.BasePart == false ? true : false);
            }
        }
        public List<Hediff_DroidStatsApplier> Hediffs
        {
            get
            {
                List<Hediff_DroidStatsApplier> list = new List<Hediff_DroidStatsApplier>();

                var list2 = (from t in Parts
                             where t.Part.BasePart == false
                             select t).ToList();
                if (list2.Count > 0)
                {
                    foreach (var part in list2)
                    {
                        Hediff_DroidStatsApplier hediff = new Hediff_DroidStatsApplier();
                        HediffStage stage = new HediffStage();
                        stage.statOffsets = new List<StatModifier>();
                        if (part.Part.statOffsets != null && part.Part.statOffsets.Count > 0)
                            stage.statOffsets.AddRange(part.Part.statOffsets);
                        if (part.Part.requirements != null && part.Part.requirements.Count > 0)
                            stage.statOffsets.AddRange(part.Part.requirements);
                        if (part.Part.capMods.Count > 0)
                        {
                            foreach (var capMod in part.Part.capMods)
                                stage.capMods.Add(capMod);
                        }
                        hediff.Stage = stage;
                        hediff.def = DefDatabase<HediffDef>.GetNamed("MD3_DroidStatsApplier").Clone<HediffDef>($"Hediff_{part.Part.defName}");
                        hediff.label = part.Part.LabelCap;
                        list.Add(hediff);
                    }
                }
                return list;
            }
        }

        private StatModifier maxCPU = null;
        private StatModifier usedCPU = null;
        private bool RecacheMaxCPU = true;
        private bool RecacheUsedCPU = true;
        public StatModifier GetMaxCPU
        {
            get
            {
                if (maxCPU == null || RecacheMaxCPU)
                {
                    var list = (from t in Parts
                                where t.Part.statOffsets.Any(x => x.stat == DroidStatDefOf.CPUCapacity)
                                select t).ToList();
                    maxCPU = new StatModifier();
                    maxCPU.stat = DroidStatDefOf.CPUCapacity;
                    maxCPU.value = maxCPU.stat.defaultBaseValue;
                    if (list.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            var stat = p.Part.statOffsets.Where(x => x.stat == DroidStatDefOf.CPUCapacity).First();
                            maxCPU.value += stat.value;
                        }
                    }
                    RecacheMaxCPU = false;
                }
                return maxCPU;
            }
        }
        public StatModifier GetUsedCPU
        {
            get
            {
                if (usedCPU == null || RecacheUsedCPU)
                {
                    var list = (from t in Parts
                                where t.Part.requirements.Any(x => x.stat == DroidStatDefOf.CPUUsage)
                                select t).ToList();
                    usedCPU = new StatModifier();
                    usedCPU.stat = DroidStatDefOf.CPUUsage;
                    usedCPU.value = usedCPU.stat.defaultBaseValue;
                    if (list.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            var stat = p.Part.requirements.Where(x => x.stat == DroidStatDefOf.CPUUsage).First();
                            usedCPU.value += stat.value;
                        }
                    }
                    RecacheUsedCPU = false;
                }
                return usedCPU;
            }
        }
        public string CPUTooltip
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("CPU usage vs. Max CPU available."); //TODO:: show all parts which have a cpu requirement
                return sb.ToString();
            }
        }

        private StatModifier maxPowerDrain = null;
        private StatModifier powerDrain = null;
        private bool RecachePowerDrain = true;
        private bool RecacheMaxPowerDrain = true;
        public StatModifier GetMaxPowerDrain
        {
            get
            {
                if (maxPowerDrain == null || RecacheMaxPowerDrain)
                {
                    var list = (from t in Parts
                                where t.Part.statOffsets.Any(x => x.stat == DroidStatDefOf.PowerDrainMaxRate)
                                select t).ToList();
                    maxPowerDrain = new StatModifier();
                    maxPowerDrain.stat = DroidStatDefOf.PowerDrainMaxRate;
                    maxPowerDrain.value = maxPowerDrain.stat.defaultBaseValue;
                    if (list.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            var stat = p.Part.statOffsets.Where(x => x.stat == DroidStatDefOf.PowerDrainMaxRate).First();
                            maxPowerDrain.value += stat.value;
                        }
                    }
                    RecacheMaxPowerDrain = false;
                }
                return maxPowerDrain;
            }
        }
        public StatModifier GetPowerDrain
        {
            get
            {
                if (powerDrain == null || RecachePowerDrain)
                {
                    var list = (from t in Parts
                                where t.Part.requirements.Any(x => x.stat == DroidStatDefOf.PowerDrain)
                                select t).ToList();
                    powerDrain = new StatModifier();
                    powerDrain.stat = DroidStatDefOf.PowerDrain;
                    powerDrain.value = powerDrain.stat.defaultBaseValue;
                    if (list.Count > 0)
                    {
                        foreach (var p in list)
                        {
                            var stat = p.Part.requirements.Where(x => x.stat == DroidStatDefOf.PowerDrain).First();
                            powerDrain.value += stat.value;
                        }
                    }
                    RecachePowerDrain = false;
                }
                return powerDrain;
            }
        }
        public string PowerDrainTooltip
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Power drain vs. max power drain."); //TODO:: show all parts which have a cpu requirement
                return sb.ToString();
            }
        }

        public bool HasArmourPlating
        {
            get
            {
                return GetPartCustomisePacks(DroidCustomiseGroupDef.Named("MD3_DroidArmourPlating")).Count > 0;
            }
        }

        public DroidDesign()
        {

        }
        public DroidDesign(ChassisType ct = ChassisType.Medium)
        {
            id = DroidManager.Instance.GetUniqueID();
            chassisType = ct;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref id, "designID");
            Scribe_Values.Look(ref Label, "label");
            Scribe_Values.Look(ref VersionNumber, "versionRun");
            Scribe_Values.Look(ref chassisType, "chassisType");
            Scribe_Collections.Look(ref aiPackages, "aiPackages");
            Scribe_Values.Look(ref CapableOfViolence, "capableOfViolence");
            //Scribe_Values.Look(ref color, "color");
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                partsCondensed = CondenseParts();
                Scribe_Collections.Look(ref partsCondensed, "partsCondensed", LookMode.Deep);
            }
            else
            {
                Scribe_Collections.Look(ref partsCondensed, "partsCondensed", LookMode.Deep);
                LoadDictionary();
            }
        }

        public void AddHediffsToDroid(Droid d)
        {
            if (HasModifiedBody)
            {
                var packs = (from t in Parts
                             where t.Part.BasePart == false
                             select t).ToList();
                foreach (var pack in packs)
                {
                    var list = d.GetBodyPartRecords(pack.ChassisPoint, pack.BodyPosition);
                    if (list.Count > 0)
                    {
                        foreach (var rec in list)
                        {
                            var hediff = BuildHediff(pack, rec, d);
                            d.health.AddHediff(hediff, rec);
                        }
                    }
                    else
                    {
                        throw new Exception($"Have customisation packs for body but couldn't find the parts! {pack.ToString()}");
                    }
                }
            }
        }

        public Hediff BuildHediff(PartCustomisePack pack, BodyPartRecord rec, Droid d)
        {
            Hediff_DroidStatsApplier hediff = (Hediff_DroidStatsApplier)HediffMaker.MakeHediff(DroidsDefOf.MD3_DroidStatsApplier, d, rec);
            HediffStage stage = new HediffStage();
            stage.statOffsets = new List<StatModifier>();
            hediff.AddedPartHealth = pack.Part.addedPartHealth;
            if (pack.Part.statOffsets != null && pack.Part.statOffsets.Count > 0)
                stage.statOffsets.AddRange(pack.Part.statOffsets);
            if (pack.Part.requirements != null && pack.Part.requirements.Count > 0)
                stage.statOffsets.AddRange(pack.Part.requirements);
            if (pack.Part.capMods.Count > 0)
            {
                foreach (var capMod in pack.Part.capMods)
                    stage.capMods.Add(capMod);
            }
            hediff.Stage = stage;
            hediff.label = pack.Part.LabelCap;
            return hediff;
        }

        public IEnumerable<BodyPartRecord> GetBasePartAt(ChassisPoint cp, BodyPosition bp = BodyPosition.Undefined)
        {
            foreach (var record in BaseBodyDef.AllParts)
            {
                if (record.def is DroidChassisPartDef)
                {
                    var def = record.def as DroidChassisPartDef;
                    if (def.ChassisPoint == cp)
                    {
                        if (record is DroidChassisPartRecord)
                        {
                            var chassisRecord = record as DroidChassisPartRecord;
                            if (bp == BodyPosition.Undefined || chassisRecord.bodyPosition == bp)
                                yield return record;
                        }
                        else
                        {
                            yield return record;
                        }
                    }
                }
            }
        }

        public List<PartCustomisePack> GetPartCustomisePacks(DroidCustomiseGroupDef group, bool silentFail = false)
        {
            if (!partsGrouped.Keys.Contains(group))
            {
                List<PartCustomisePack> list = new List<PartCustomisePack>();
                foreach (var li in group.Parts)
                {
                    //Make a new part customise pack
                    var baseParts = GetBasePartAt(li.ChassisPoint, li.BodyPosition).ToList();
                    if (baseParts.Count > 0)
                    {
                        foreach (var partRecord in baseParts)
                        {
                            if (partRecord is DroidChassisPartRecord)
                            {
                                //Check the body position. If the position passed is undefined, don't care what position the part has
                                DroidChassisPartRecord droidRecord = partRecord as DroidChassisPartRecord;
                                if (li.BodyPosition == BodyPosition.Undefined || droidRecord.bodyPosition == li.BodyPosition)
                                {
                                    DroidChassisPartDef dcpd = droidRecord.defAsDroidDef;
                                    if (dcpd == null)
                                        throw new InvalidOperationException($"{partRecord.body.defName} contains a bodypart which is not type DroidChassisPartDef: {partRecord.def.defName}");
                                    var newPCP = new PartCustomisePack(li.ChassisPoint, dcpd, droidRecord.bodyPosition);
                                    list.Add(newPCP);
                                }
                            }
                            else
                            {
                                //Otherwise, just make a new part with undefined body position
                                DroidChassisPartDef dcpd = partRecord.GetChassisPartDef();
                                if (dcpd == null)
                                    throw new InvalidOperationException($"{partRecord.body.defName} contains a bodypart which is not type DroidChassisPartDef: {partRecord.def.defName}");
                                var newPCP = new PartCustomisePack(li.ChassisPoint, dcpd);
                                list.Add(newPCP);
                            }
                        }
                    }
                    else
                    {
                        if (!silentFail)
                            Log.Error($"Unable to find any base parts at ChassisPoint: {li.ChassisPoint} and BodyPosition: {li.BodyPosition} for BaseBodyDef: {BaseBodyDef.defName}");
                    }
                }
                partsGrouped.Add(group, list);
                return partsGrouped[group];
            }
            else
                return partsGrouped[group];
        }

        public void AddAIPackage(AIPackageDef package)
        {
            aiPackages.Add(package);
        }

        public BodyDef GenerateBodyDef(bool addToDatabase)
        {
            BodyDef bd = BaseBodyDef.Copy(BaseBodyDef.defName + this.GetHashCode().ToString());
            var list = (from t in Parts
                        where t.Part.BasePart == false
                        select t).ToList();
            if (list.Count > 0)
            {
                foreach (var part in list)
                {
                    var list2 = (from t in bd.AllParts
                                 where t.def is DroidChassisPartDef && ((DroidChassisPartDef)t.def).ChassisType == part.Part.ChassisType && ((DroidChassisPartDef)t.def).ChassisPoint == part.Part.ChassisPoint
                                 select t).ToList();
                    if (list2.Count > 0)
                    {
                        foreach (var record in list2)
                        {
                            if (record is DroidChassisPartRecord)
                            {
                                DroidChassisPartRecord dRec = record as DroidChassisPartRecord;
                                if (dRec.bodyPosition == part.BodyPosition)
                                    record.def = part.Part;
                            }
                            else
                                record.def = part.Part;
                            if (!record.customLabel.NullOrEmpty())
                            {
                                if (record.customLabel.StartsWith("left"))
                                    record.customLabel = $"left {record.def.label}";
                                else if (record.customLabel.StartsWith("right"))
                                    record.customLabel = $"right {record.def.label}";
                            }
                        }
                    }
                }
            }
            if (addToDatabase)
                DefDatabase<BodyDef>.Add(bd);
            return bd;
        }

        private List<GroupPackPair> CondenseParts()
        {
            List<GroupPackPair> list = new List<GroupPackPair>();
            foreach (var kv in partsGrouped)
            {
                foreach (var pack in kv.Value)
                    list.Add(new GroupPackPair(kv.Key, pack));
            }
            return list;
        }
        private void LoadDictionary()
        {
            foreach (var pair in partsCondensed)
            {
                if (!partsGrouped.Keys.Contains(pair.group))
                {
                    partsGrouped.Add(pair.group, new List<PartCustomisePack>() { pair.pack });
                }
                else
                {
                    if (!partsGrouped[pair.group].Any(x => x.Equals(pair.pack)))
                    {
                        partsGrouped[pair.group].Add(pair.pack);
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"========={Label} - ID: {ID}=========");
            sb.AppendLine($"Parts: {Parts.Count}");
            foreach (var p in Parts)
            {
                sb.AppendLine($"    -Part def: {p.Part}, ChassisPoint: {p.ChassisPoint}, BodyPosition: {p.BodyPosition}");
            }
            return sb.ToString();
        }

        public void Recache()
        {
            RecacheUsedCPU = true;
            RecacheMaxCPU = true;
            RecacheMaxPowerDrain = true;
            RecachePowerDrain = true;
        }
    }
}
