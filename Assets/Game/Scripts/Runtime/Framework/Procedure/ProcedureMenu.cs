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

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
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
            GameEntry.UI.CloseUIForm(_menuSerialId);
        }

        public void EnterGame()
        {
            _enterGame = true;
        }
    }
}