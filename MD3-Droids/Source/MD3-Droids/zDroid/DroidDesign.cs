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

        private Color color;
        private Graphic_Multi bodyGraphic;
        private Graphic_Multi headGraphic;

        //TODO:: implement skills list
        private ChassisType chassisType = ChassisType.Medium;
        private List<AIPackageDef> aiPackages = new List<AIPackageDef>();
        private Dictionary<DroidCustomiseGroupDef, List<PartCustomisePack>> partsGrouped = new Dictionary<DroidCustomiseGroupDef, List<PartCustomisePack>>();
        public bool CapableOfViolence = false;

        private BodyDef bodyDef = null;

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
        public BodyDef BodyDef
        {
            get
            {
                //TODO:: check if bodydef has been generated, if not then generate and add to db
                if (bodyDef == null)
                    GenerateBodyDef(true);

                return bodyDef;
            }
        }
        public BodyDef BaseBodyDef
        {
            get
            {//TODO:: implement this for all chassis types
                return DefDatabase<BodyDef>.GetNamed("MD3_MediumDroid");
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

        private bool RecacheStatOffsets = true;
        private bool RecacheCapMods = true;
        private bool RecachePartRequirements = true;
        private bool RecacheAIRequirements = true;
        private List<StatModifier> statOffsets = null;
        private List<PawnCapacityModifier> capMods = null;
        private List<StatModifier> partRequirements = null;
        private List<StatModifier> aiRequirements = null;
        public List<StatModifier> StatOffsets
        {
            get
            {
                if (statOffsets == null || RecacheStatOffsets == true)
                {
                    List<StatModifier> list = new List<StatModifier>();
                    var partsList = (from t in Parts
                                     where t.Part.statOffsets.Count > 0
                                     select t).ToList();
                    if (partsList.Count > 0)
                    {
                        foreach (var p in partsList)
                        {
                            list.AddRange(p.Part.statOffsets);
                        }
                    }
                    statOffsets = list;
                    RecacheStatOffsets = false;
                }
                return statOffsets;
            }
        }
        public List<PawnCapacityModifier> CapMods
        {
            get
            {
                if (capMods == null || RecacheCapMods == true)
                {
                    List<PawnCapacityModifier> list = new List<PawnCapacityModifier>();
                    var partsList = (from t in Parts
                                     where t.Part.capMods.Count > 0
                                     select t).ToList();
                    if (partsList.Count > 0)
                    {
                        foreach (var p in partsList)
                        {
                            foreach (var capMod in p.Part.capMods)
                                list.Add(capMod);
                        }
                    }
                    capMods = list;
                    RecacheCapMods = false;
                }
                return capMods;
            }
        }
        public List<StatModifier> PartRequirements
        {
            get
            {
                if (partRequirements == null || RecachePartRequirements == true)
                {
                    List<StatModifier> list = new List<StatModifier>();
                    var partsList = (from t in Parts
                                     where t.Part.requirements.Count > 0
                                     select t).ToList();
                    if (partsList.Count > 0)
                    {
                        foreach (var p in partsList)
                        {
                            list.AddRange(p.Part.requirements);
                        }
                    }
                    partRequirements = list;
                    RecachePartRequirements = false;
                }
                return partRequirements;
            }
        }
        public List<StatModifier> AIRequirements
        {
            get
            {
                if (aiRequirements == null || RecacheAIRequirements == true)
                {
                    List<StatModifier> list = new List<StatModifier>();
                    var packages = (from t in AIPackages
                                    where t.cpuUsage > 0f
                                    select t).ToList();
                    if (packages.Count > 0)
                    {
                        foreach (var p in packages)
                        {
                            StatModifier sm = new StatModifier();
                            sm.stat = DroidStatDefOf.CPUUsage;
                            sm.value = p.cpuUsage;
                            list.Add(sm);
                        }
                    }
                    aiRequirements = list;
                    RecachePartRequirements = false;
                }
                return aiRequirements;
            }
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
            Scribe_Values.Look(ref color, "color");
            Scribe_Collections.Look(ref aiPackages, "aiPackages");
            Scribe_Collections.Look(ref partsGrouped, "parts");
            Scribe_Values.Look(ref CapableOfViolence, "capableOfViolence");
            Scribe_Values.Look(ref VersionNumber, "versionRun");
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

        public List<PartCustomisePack> GetPartCustomisePacks(DroidCustomiseGroupDef group)
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
                        Log.Error($"Unable to find any base parts at ChassisPoint: {li.ChassisPoint} and BodyPosition: {li.BodyPosition} for BaseBodyDef: {BaseBodyDef.defName}");
                }
                partsGrouped.Add(group, list);
                return partsGrouped[group];
            }
            else
                return partsGrouped[group];

            //foreach (var li in group.Parts)
            //{
            //    //First check if part has been modified before
            //    var list = (from t in partsGrouped
            //                where t.ChassisPoint == li.ChassisPoint && t.BodyPosition == li.BodyPosition
            //                select t).ToList();
            //    if (list.Count > 0)
            //    {
            //        foreach (var pcp in list)
            //        {
            //            if (li.BodyPosition != BodyPosition.Undefined)
            //            {
            //                if (pcp.BodyPosition == li.BodyPosition)
            //                    yield return pcp;
            //            }
            //            else
            //                yield return pcp;
            //        }
            //    }
            //    else
            //    {
            //        //Make a new part customise pack
            //        var baseParts = GetBasePartAt(li.ChassisPoint, li.BodyPosition).ToList();
            //        if (baseParts.Count > 0)
            //        {
            //            foreach (var partRecord in baseParts)
            //            {
            //                if (partRecord is DroidChassisPartRecord)
            //                {
            //                    //Check the body position. If the position passed is undefined, don't care what position the part has
            //                    DroidChassisPartRecord droidRecord = partRecord as DroidChassisPartRecord;
            //                    if (li.BodyPosition == BodyPosition.Undefined || droidRecord.bodyPosition == li.BodyPosition)
            //                    {
            //                        DroidChassisPartDef dcpd = droidRecord.defAsDroidDef;
            //                        if (dcpd == null)
            //                            throw new InvalidOperationException($"{partRecord.body.defName} contains a bodypart which is not type DroidChassisPartDef: {partRecord.def.defName}");
            //                        var newPCP = new PartCustomisePack(li.ChassisPoint, dcpd, droidRecord.bodyPosition);
            //                        AddPart(newPCP);
            //                        yield return newPCP;
            //                    }
            //                }
            //                else
            //                {
            //                    //Otherwise, just make a new part with undefined body position
            //                    DroidChassisPartDef dcpd = partRecord.GetChassisPartDef();
            //                    if (dcpd == null)
            //                        throw new InvalidOperationException($"{partRecord.body.defName} contains a bodypart which is not type DroidChassisPartDef: {partRecord.def.defName}");
            //                    var newPCP = new PartCustomisePack(li.ChassisPoint, dcpd);
            //                    AddPart(newPCP);
            //                    yield return newPCP;
            //                }
            //            }
            //        }
            //        else
            //            Log.Error($"Unable to find any base parts at ChassisPoint: {li.ChassisPoint} and BodyPosition: {li.BodyPosition} for BaseBodyDef: {BaseBodyDef.defName}");
            //    }
            //}
        }

        public void AddAIPackage(AIPackageDef package)
        {
            aiPackages.Add(package);
        }

        public bool CanAcceptPart(DroidChassisPartDef def)
        {
            //TODO:: return value depending on whether part has correct chassis type
            return true;
        }

        public BodyDef GenerateBodyDef(bool addToDatabase)
        {
            //TODO:: generate and return bodydef
            if (ChassisType == ChassisType.Medium)
            {
                BodyDef bd = DefDatabase<BodyDef>.GetNamed("MD3_MediumDroid");
                bodyDef = bd;
            }
            return bodyDef;
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

        public void RecacheStats()
        {
            RecacheUsedCPU = true;
            RecacheMaxCPU = true;
            RecacheMaxPowerDrain = true;
            RecachePowerDrain = true;

            RecacheStatOffsets = true;
            RecacheCapMods = true;
            RecachePartRequirements = true;
            RecacheAIRequirements = true;
        }
    }
}
