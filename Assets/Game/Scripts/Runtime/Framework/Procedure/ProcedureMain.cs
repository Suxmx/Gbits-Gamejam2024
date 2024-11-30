﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using NodeCanvas.DialogueTrees;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        public float CurrentFps => _fpsCounter.CurrentFps;

        private bool _returnMenu = false;
        private bool _restart = false;
        private FpsCounter _fpsCounter;
        private ProcedureOwner _procedureOwner;

        public override bool UseNativeDialog
        {
            get { return false; }
        }


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            bool cutscene = false;
            if (procedureOwner.HasData("PlayCutscene"))
            {
                cutscene=procedureOwner.GetData<VarBoolean>("PlayCutscene");
            }
            _returnMenu = false;
            _restart = false;
            _fpsCounter = new FpsCounter(0.5f);
            _procedureOwner = procedureOwner;

            GameManager.Create();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            _fpsCounter.Update(elapseSeconds, realElapseSeconds);
            if (_returnMenu)
            {
                procedureOwner.SetData<VarString>("NextScene", AssetUtility.MenuSceneName);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            else if (_restart)
            {
                procedureOwner.SetData<VarString>("NextScene", AssetUtility.MainSceneName);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameManager.Instance.OnExit();
        }

        public void ReturnMenu()
        {
            _returnMenu = true;
        }

        public void Restart()
        {
            _procedureOwner.SetData<VarBoolean>("PlayCutscene", true);
            _restart = true;
        }
    }
}