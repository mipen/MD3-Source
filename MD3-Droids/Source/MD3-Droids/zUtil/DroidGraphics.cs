using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD3_Droids
{
    [StaticConstructorOnStartup]
    public static class DroidGraphics
    {
        private static Dictionary<ChassisType, List<DroidGraphicDef>> heads = new Dictionary<ChassisType, List<DroidGraphicDef>>();
        private static Dictionary<ChassisType, List<DroidGraphicDef>> bodies = new Dictionary<ChassisType, List<DroidGraphicDef>>();

        public static Dictionary<ChassisType, List<DroidGraphicDef>> Heads => heads;
        public static Dictionary<ChassisType, List<DroidGraphicDef>> Bodies => bodies;

        private static Dictionary<ChassisType, int> HeadIndex = new Dictionary<ChassisType, int>();
        private static Dictionary<ChassisType, int> BodyIndex = new Dictionary<ChassisType, int>();

        static DroidGraphics()
        {
            HeadIndex.Add(ChassisType.Small, 0);
            HeadIndex.Add(ChassisType.Medium, 0);
            HeadIndex.Add(ChassisType.Large, 0);
            BodyIndex.Add(ChassisType.Small, 0);
            BodyIndex.Add(ChassisType.Medium, 0);
            BodyIndex.Add(ChassisType.Large, 0);

            Bodies.Add(ChassisType.Small, new List<DroidGraphicDef>());
            Bodies.Add(ChassisType.Medium, new List<DroidGraphicDef>());
            Bodies.Add(ChassisType.Large, new List<DroidGraphicDef>());
            Heads.Add(ChassisType.Small, new List<DroidGraphicDef>());
            Heads.Add(ChassisType.Medium, new List<DroidGraphicDef>());
            Heads.Add(ChassisType.Large, new List<DroidGraphicDef>());

            //TODO:: Include small and large chassis types

            //Medium droids
            foreach (DroidGraphicDef d in DefDatabase<DroidGraphicDef>.AllDefsListForReading.Where((x) => x.chassisType == ChassisType.Medium && x.graphicType == DroidGraphicType.Body))
            {
                Bodies[ChassisType.Medium].Add(d);
            }
            foreach (DroidGraphicDef d in DefDatabase<DroidGraphicDef>.AllDefsListForReading.Where((x) => x.chassisType == ChassisType.Medium && x.graphicType == DroidGraphicType.Head))
            {
                Heads[ChassisType.Medium].Add(d);
            }

            //large droids
            foreach (DroidGraphicDef d in DefDatabase<DroidGraphicDef>.AllDefsListForReading.Where((x) => x.chassisType == ChassisType.Large && x.graphicType == DroidGraphicType.Body))
            {
                Bodies[ChassisType.Large].Add(d);
            }
            foreach (DroidGraphicDef d in DefDatabase<DroidGraphicDef>.AllDefsListForReading.Where((x) => x.chassisType == ChassisType.Large && x.graphicType == DroidGraphicType.Head))
            {
                Heads[ChassisType.Large].Add(d);
            }
        }

        public static DroidGraphicDef GetFirstHead(ChassisType type)
        {
            return Heads[type].First();
        }

        public static DroidGraphicDef GetFirstBody(ChassisType type)
        {
            return Bodies[type].First();
        }

        public static DroidGraphicDef GetNextHead(ChassisType type)
        {
            int index = HeadIndex[type] + 1;
            try
            {
                List<DroidGraphicDef> list = Heads[type];
                if (index > list.Count - 1)
                    index = 0;
                return list[index];
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving droid head graphic for ChassisType: {type}\n{ex.Message}\nReturning Default");
                index = 0;
                return GetFirstHead(type);
            }
            finally
            {
                HeadIndex[type] = index;
            }
        }

        public static DroidGraphicDef GetPreviousHead(ChassisType type)
        {
            int index = HeadIndex[type] - 1;
            try
            {
                List<DroidGraphicDef> list = Heads[type];
                if (index < 0)
                    index = list.Count - 1;
                return list[index];
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving droid head graphic for ChassisType: {type}\n{ex.Message}\nReturning Default");
                index = 0;
                return GetFirstHead(type);
            }
            finally
            {
                HeadIndex[type] = index;
            }
        }

        public static DroidGraphicDef GetNextBody(ChassisType type)
        {
            int index = BodyIndex[type] + 1;
            try
            {
                List<DroidGraphicDef> list = Bodies[type];
                if (index > list.Count - 1)
                    index = 0;
                return list[index];
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving droid body graphic for ChassisType: {type}\n{ex.Message}\nReturning Default");
                index = 0;
                return GetFirstBody(type);
            }
            finally
            {
                BodyIndex[type] = index;
            }
        }

        public static DroidGraphicDef GetPreviousBody(ChassisType type)
        {
            int index = BodyIndex[type] - 1;
            try
            {
                List<DroidGraphicDef> list = Bodies[type];
                if (index < 0)
                    index = list.Count - 1;
                return list[index];
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving droid body graphic for ChassisType: {type}\n{ex.Message}\nReturning Default");
                index = 0;
                return GetFirstBody(type);
            }
            finally
            {
                BodyIndex[type] = index;
            }
        }
    }
}
