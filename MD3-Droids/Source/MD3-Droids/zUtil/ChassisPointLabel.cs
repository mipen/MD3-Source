using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD3_Droids
{
    public static class ChassisPointLabel
    {
        public static string Get(ChassisPoint cp, BodyPosition bp = BodyPosition.Undefined)
        {
            switch (cp)
            {
                case ChassisPoint.ArmourPlating:
                    return "Armour plating";
                case ChassisPoint.ArmourHardPoint:
                    return "Armour hard point";
                case ChassisPoint.MotivatorManipulation:
                    return "Motivator (manipulation)";
                case ChassisPoint.MotivatorMoving:
                    return "Motivator (moving)";
                case ChassisPoint.CPU:
                    return "CPU";
                case ChassisPoint.VisualReceptor:
                    if (bp == BodyPosition.LeftVisualReceptor)
                        return "Left visual receptor";
                    else if (bp == BodyPosition.RightVisualReceptor)
                        return "Right visual receptor";
                    else
                        return "Visual receptor";
                case ChassisPoint.MediumHead:
                    return "Head";
                case ChassisPoint.MediumChassis:
                    return "Chassis";
                case ChassisPoint.MediumShoulder:
                    if (bp == BodyPosition.LeftShoulder)
                        return "Left shoulder";
                    else if (bp == BodyPosition.LeftShoulder)
                        return "Right shoulder";
                    else
                        return "Shoulder";
                case ChassisPoint.MediumArm:
                    if (bp == BodyPosition.LeftArm)
                        return "Left arm";
                    else if (bp == BodyPosition.RightArm)
                        return "Right arm";
                    else
                        return "Arm";
                case ChassisPoint.MediumHand:
                    if (bp == BodyPosition.RightHand)
                        return "Right hand";
                    else if (bp == BodyPosition.LeftArm)
                        return "Left hand";
                    else
                        return "Hand";
                case ChassisPoint.MediumFinger:
                    return "Finger";
                case ChassisPoint.MediumLegSocket:
                    return "Leg socket";
                case ChassisPoint.MediumLeg:
                    if (bp == BodyPosition.LeftLeg)
                        return "Left leg";
                    else if (bp == BodyPosition.RightLeg)
                        return "Right leg";
                    else
                        return "Leg";
                case ChassisPoint.MediumFoot:
                    if (bp == BodyPosition.LeftFoot)
                        return "Left foot";
                    else if (bp == BodyPosition.RightFoot)
                        return "Right foot";
                    else
                        return "Foot";
                default:
                    return "Slot";
            }
        }
    }
}
