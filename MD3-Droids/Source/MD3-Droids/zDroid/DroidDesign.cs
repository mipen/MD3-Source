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
        private Color color;
        private Graphic_Multi bodyGraphic;
        private Graphic_Multi headGraphic;
        private List<AIPackageDef> aiPackages = new List<AIPackageDef>();
        private ChassisType chassisType = ChassisType.Medium;
        //TODO:: implement skills list
        private List<PartCustomisePack> parts = new List<PartCustomisePack>();
        private BodyDef bodyDef = null;
        public string Label = "";
        public bool CapableOfViolence = false;
        public int VersionNumber = 1;

        public int ID => id;
        public List<PartCustomisePack> Parts => parts;
        public List<AIPackageDef> AIPackages => aiPackages;
        public ChassisType ChassisType => chassisType;
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
            Scribe_Collections.Look(ref parts, "parts");
            Scribe_Values.Look(ref CapableOfViolence, "capableOfViolence");
            Scribe_Values.Look(ref VersionNumber, "versionRun");
        }

        public void AddPart(PartCustomisePack pcp)
        {
            foreach (var p in Parts)
            {
                if (p.ChassisPoint == pcp.ChassisPoint && p.BodyPosition == pcp.BodyPosition)
                {
                    RemovePart(p);
                }
            }
            parts.Add(pcp);
        }

        public void RemovePart(PartCustomisePack pcp)
        {
            parts.Remove(pcp);
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

        public IEnumerable<PartCustomisePack> GetPartCustomisePacks(DroidCustomiseGroupDef group)
        {
            foreach (var li in group.Parts)
            {
                //First check if part has been modified before
                var list = (from t in parts
                            where t.ChassisPoint == li.ChassisPoint && t.BodyPosition == li.BodyPosition
                            select t).ToList();
                if (list.Count > 0)
                {
                    foreach (var pcp in list)
                    {
                        if (li.BodyPosition != BodyPosition.Undefined)
                        {
                            if (pcp.BodyPosition == li.BodyPosition)
                                yield return pcp;
                        }
                        else
                            yield return pcp;
                    }
                }
                else
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
                                    AddPart(newPCP);
                                    yield return newPCP;
                                }
                            }
                            else
                            {
                                //Otherwise, just make a new part with undefined body position
                                DroidChassisPartDef dcpd = partRecord.GetChassisPartDef();
                                if (dcpd == null)
                                    throw new InvalidOperationException($"{partRecord.body.defName} contains a bodypart which is not type DroidChassisPartDef: {partRecord.def.defName}");
                                var newPCP = new PartCustomisePack(li.ChassisPoint, dcpd);
                                AddPart(newPCP);
                                yield return newPCP;
                            }
                        }
                    }
                    else
                        Log.Error($"Unable to find any base parts at ChassisPoint: {li.ChassisPoint} and BodyPosition: {li.BodyPosition} for BaseBodyDef: {BaseBodyDef.defName}");
                }
            }
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
    }
}
