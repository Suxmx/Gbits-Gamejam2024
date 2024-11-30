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

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        private bool _enterGame;
        private ProcedureOwner _owner;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _owner = procedureOwner;
            _enterGame = false;
            _menuSerialId = (int)GameEntry.UI.OpenUIForm(UIFormId.MenuForm);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (_enterGame)
            {
                procedureOwner.SetData<VarString>("NextScene", AssetUtility.MainSceneName);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            procedureOwner.SetData<VarBoolean>("PlayCutscene", true);
        }

        public void EnterGame(int index)
        {
            _owner.SetData<VarInt32>("LevelIndex", index);
            _enterGame = true;
        }
    }
}