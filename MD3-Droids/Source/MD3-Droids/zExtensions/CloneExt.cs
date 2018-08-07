using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace MD3_Droids
{
    public static class ObjExt
    {
        public static T Clone<T>(this object obj, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return (T)(typeof(T).GetMethod("MemberwiseClone", bindingFlags)).Invoke(obj, null);
        }

        public static T Clone<T>(this object obj, string defName = "", BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance) where T : Def, new()
        {
            var clone = (T)(typeof(T).GetMethod("MemberwiseClone", bindingFlags)).Invoke(obj, null);
            if (!defName.NullOrEmpty())
                clone.defName = defName;
            return clone;
        }
    }
}
