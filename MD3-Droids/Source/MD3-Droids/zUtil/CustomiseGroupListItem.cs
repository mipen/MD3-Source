using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD3_Droids
{
    public class CustomiseGroupListItem
    {

        public ChassisPoint ChassisPoint = ChassisPoint.Undefined;
        public BodyPosition BodyPosition = BodyPosition.Undefined;

        public CustomiseGroupListItem(ChassisPoint cp = ChassisPoint.Undefined, BodyPosition bp = BodyPosition.Undefined)
        {
            ChassisPoint = cp;
            BodyPosition = bp;
        }

        public CustomiseGroupListItem()
        {

        }
    }
}
