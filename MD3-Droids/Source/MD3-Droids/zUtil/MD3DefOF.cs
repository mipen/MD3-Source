using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD3_Droids
{
    internal class MD3DefOF
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class DefOf : Attribute
        { }
    }
}
