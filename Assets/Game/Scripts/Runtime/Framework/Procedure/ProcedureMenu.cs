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
            _procedureOwner = procedureOwner;
            _menuSerialId = (int)GameEntry.UI.OpenUIForm(UIFormId.MenuForm);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            procedureOwner.SetData<VarBoolean>("PlayCutscene", true);
        }

        public void EnterGame(int index)
        {
            _procedureOwner.SetData<VarInt32>("LevelIndex", index);
            _procedureOwner.SetData<VarString>("NextScene", AssetUtility.GetLevelSceneAsset(index));
            ChangeState<ProcedureChangeScene>(_procedureOwner);
        }
    }
}