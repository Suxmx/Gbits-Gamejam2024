//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureMenu : ProcedureBase
    {
        private int _menuSerialId;
        private ProcedureOwner _procedureOwner;

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            if (GameEntry.Cutscene.IsPlayingCutscene) GameEntry.Cutscene.FadeCutscene(null);
            _procedureOwner = procedureOwner;
            _menuSerialId = (int)GameEntry.UI.OpenUIForm(UIFormId.MenuForm);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.UI.CloseUIForm(_menuSerialId);
            GameEntry.UI.TryCloseUIFormById(UIFormId.LevelChooseForm);
        }

        public void EnterGame(int index)
        {
            _procedureOwner.SetData<VarInt32>("LevelIndex", index);
            _procedureOwner.SetData<VarString>("NextScene", AssetUtility.GetLevelSceneSubName(index));
            GameEntry.Cutscene.PlayCutscene(DoChangeState);
        }

        private void DoChangeState()
        {
            ChangeState<ProcedureChangeScene>(_procedureOwner);
        }
    }
}