using RimWorld;
using UnityEngine;
using Verse;

namespace MD3_Droids
{
    public class Dialog_NewBlueprint : Window
    {
        public override Vector2 InitialSize => new Vector2(1280f, 900f);
        public static readonly Vector2 MinSize = new Vector2(700f, 700f);
        private Blueprint blueprint;
        private BlueprintWindowHandler bpHandler;

        public Dialog_NewBlueprint(Blueprint blueprint)
        {
            this.blueprint = blueprint;
            resizeable = true;
            draggable = true;

            bpHandler = new BlueprintWindowHandler(blueprint, BlueprintHandlerState.New);
            bpHandler.EventCancel += () => { Close(); };
            bpHandler.EventAccept += () =>
            {
                if (blueprint.IsValid())
                {
                    DroidManager.Instance.Blueprints.Add(blueprint);
                    Close();
                }
                else
                {
                    Messages.Message(new Message("InvalidBlueprint".Translate(), MessageTypeDefOf.RejectInput));
                }
            };

            absorbInputAroundWindow = true;
            forcePause = true;

            StatsReportUtility.Reset();
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            ITab_DroidConstructionTable.DrawStats = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (windowRect.width < MinSize.x)
            {
                resizeable = false;
                windowRect.width = MinSize.x;
                resizeable = true;
            }
            if (windowRect.height < MinSize.y)
            {
                resizeable = false;
                windowRect.height = MinSize.y;
                resizeable = true;
            }
            bpHandler.DrawWindow(inRect);
        }
    }
}
