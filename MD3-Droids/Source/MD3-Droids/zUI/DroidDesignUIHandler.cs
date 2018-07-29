using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    [StaticConstructorOnStartup]
    public static class DroidDesignUIHandler
    {
        public static Texture2D HeadTex;
        public static Texture2D HeadHoverTex;

        public static Texture2D BodyTex;
        public static Texture2D BodyHoverTex;

        public static Texture2D LeftArmTex;
        public static Texture2D LeftArmHoverTex;

        public static Texture2D RightArmTex;
        public static Texture2D RightArmHoverTex;

        public static Texture2D LeftLegTex;
        public static Texture2D LeftLegHoverTex;

        public static Texture2D RightLegTex;
        public static Texture2D RightLegHoverTex;

        public static readonly Vector2 HeadRectSize = new Vector2(92, 92);
        public static readonly Vector2 BodyRectSize = new Vector2(128, 192);
        public static readonly Vector2 ArmRectSize = new Vector2(100, 100);
        public static readonly Vector2 LegRectSize = new Vector2(100, 100);
        public const float Margin = 10f;

        static DroidDesignUIHandler()
        {
            GetTextures();
        }

        public static void GetTextures()
        {
            HeadTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_Head");
            HeadHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_HeadHover");

            BodyTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_Body");
            BodyHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_BodyHover");

            LeftArmTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftArm");
            LeftArmHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftArmHover");

            RightArmTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightArm");
            RightArmHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightArmHover");

            LeftLegTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftLeg");
            LeftLegHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_LeftLegHover");

            RightLegTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightLeg");
            RightLegHoverTex = ContentFinder<Texture2D>.Get("UI/PartSelector/PartSelector_RightLegHover");
        }

        public static void DrawPartSelector(Rect mainRect, DroidDesign design, bool editMode)
        {
            try
            {
                Rect inRect = mainRect.ContractedBy(10f);

                GUI.BeginGroup(inRect);

                //Body rect
                Rect bodyRect = new Rect((inRect.width / 2) - (BodyRectSize.x / 2), inRect.center.y - (BodyRectSize.y / 2), BodyRectSize.x, BodyRectSize.y);
                //Widgets.DrawBox(bodyRect);
                if (Mouse.IsOver(bodyRect))
                    Widgets.DrawTextureFitted(bodyRect, BodyHoverTex, 1.3f);
                else
                    Widgets.DrawTextureFitted(bodyRect, BodyTex, 1.3f);
                if (Widgets.ButtonInvisible(bodyRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidChassis"), design, BodyTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Head rect
                Rect headRect = new Rect((inRect.width / 2) - (HeadRectSize.x / 2), bodyRect.y - Margin - HeadRectSize.y, HeadRectSize.x, HeadRectSize.y);
               // Widgets.DrawBox(headRect);
                if (Mouse.IsOver(headRect))
                    Widgets.DrawTextureFitted(headRect, HeadHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(headRect, HeadTex, 1);
                if (Widgets.ButtonInvisible(headRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidHead"), design, HeadTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Left arm rect
                Rect leftArmRect = new Rect(bodyRect.x - Margin - ArmRectSize.x, bodyRect.y, ArmRectSize.x, ArmRectSize.y);
               // Widgets.DrawBox(leftArmRect);
                if (Mouse.IsOver(leftArmRect))
                    Widgets.DrawTextureFitted(leftArmRect, LeftArmHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(leftArmRect, LeftArmTex, 1);
                if (Widgets.ButtonInvisible(leftArmRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidLeftArm"), design, LeftArmTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Right arm rect
                Rect rightArmRect = new Rect(bodyRect.xMax + Margin, bodyRect.y, ArmRectSize.x, ArmRectSize.y);
                //Widgets.DrawBox(rightArmRect);
                if (Mouse.IsOver(rightArmRect))
                    Widgets.DrawTextureFitted(rightArmRect, RightArmHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(rightArmRect, RightArmTex, 1);
                if (Widgets.ButtonInvisible(rightArmRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidRightArm"), design, RightArmTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Left leg rect
                Rect leftLegRect = new Rect(leftArmRect.x + LegRectSize.x / 2, bodyRect.yMax + Margin, LegRectSize.x, LegRectSize.y);
                //Widgets.DrawBox(leftLegRect);
                if (Mouse.IsOver(leftLegRect))
                    Widgets.DrawTextureFitted(leftLegRect, LeftLegHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(leftLegRect, LeftLegTex, 1);
                if (Widgets.ButtonInvisible(leftLegRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidLeftLeg"), design, LeftLegTex, editMode);
                    Find.WindowStack.Add(cp);
                }

                //Right leg rect 
                Rect rightLegRect = new Rect(rightArmRect.x - LegRectSize.x / 2, leftLegRect.y, LegRectSize.x, LegRectSize.y);
                //Widgets.DrawBox(rightLegRect);
                if (Mouse.IsOver(rightLegRect))
                    Widgets.DrawTextureFitted(rightLegRect, RightLegHoverTex, 1);
                else
                    Widgets.DrawTextureFitted(rightLegRect, RightLegTex, 1);
                if (Widgets.ButtonInvisible(rightLegRect))
                {
                    Dialog_CustomisePartGroup cp = new Dialog_CustomisePartGroup(DroidCustomiseGroupDef.Named("MD3_MediumDroidRightLeg"), design, RightLegTex, editMode);
                    Find.WindowStack.Add(cp);
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }
    }
}
