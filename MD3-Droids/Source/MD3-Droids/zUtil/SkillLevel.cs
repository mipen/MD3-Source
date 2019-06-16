using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class SkillLevel : IExposable
    {
        private SkillDef skill = null;
        private int levelInt = 0;

        public SkillDef Skill => skill;

        public int PowerUsage
        {
            get
            {
                //350 power usage for level 20 skill
                return Level == 0 ? 0 : Mathf.RoundToInt((float)Math.Pow((Level * 0.90139), 2) + 25);
            }
        }

        public int CPUUsage
        {
            get
            {
                //75 cpu for level 20 skill
                return Level == 0 ? 0 : Mathf.RoundToInt((float)Math.Pow((Level * 0.40311), 2) + 10);
            }
        }

        public int Level { get => levelInt; set => levelInt = value; }

        public SkillLevel()
        {

        }

        public SkillLevel(SkillDef skill, int level)
        {
            this.skill = skill;
            levelInt = level;
        }

        public SkillLevel(SkillRecord sr)
        {
            skill = sr.def;
            levelInt = sr.levelInt;
        }

        public void ExposeData()
        {
            Scribe_Defs.Look(ref skill, "skill");
            Scribe_Values.Look(ref levelInt, "levelInt");
        }

        public static List<SkillLevel> GetBlankList()
        {
            List<SkillLevel> skillLevels = new List<SkillLevel>();
            foreach (var def in DefDatabase<SkillDef>.AllDefs)
            {
                skillLevels.Add(new SkillLevel(def, 0));
            }
            return skillLevels;
        }
    }
}
