//------------------------------------------------------------
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
        private int _levelIndex = 0;

        public override bool UseNativeDialog
        {
            get { return false; }
        }


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _levelIndex = procedureOwner.GetData<VarInt32>("LevelIndex");
            _returnMenu = false;
            _restart = false;
            _fpsCounter = new FpsCounter(0.5f);
            _procedureOwner = procedureOwner;

            GameManager.Create(_levelIndex);
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
                procedureOwner.SetData<VarString>("NextScene", AssetUtility.GetLevelSceneAsset(_levelIndex));
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

        public void NextLevel()
        {
            _levelIndex++;
            _procedureOwner.SetData<VarInt32>("LevelIndex", _levelIndex);
            _procedureOwner.SetData<VarString>("NextScene", AssetUtility.GetLevelSceneAsset(_levelIndex));
            ChangeState<ProcedureChangeScene>(_procedureOwner);
        }
    }
}