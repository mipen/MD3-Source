using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Verse;

namespace MD3_Droids
{
    public class MD3PawnCapacityModifier : PawnCapacityModifier
    {
        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "capacity", xmlRoot.Name);
            offset = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
        }
    }
}
