//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        public float CurrentFps => _fpsCounter.CurrentFps;

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
            if (procedureOwner.HasData("LevelIndex"))
            {
                _levelIndex = procedureOwner.GetData<VarInt32>("LevelIndex");
            }
            else _levelIndex = 1;
            _fpsCounter = new FpsCounter(0.5f);
            _procedureOwner = procedureOwner;

            GameManager.Create(_levelIndex);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            _fpsCounter?.Update(elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameManager.Instance.OnExit();
            GameEntry.UI.TryCloseUIFormById(UIFormId.PauseForm);
            GameEntry.UI.TryCloseUIFormById(UIFormId.LevelChooseForm);
        }

        public void ReturnMenu()
        {
            _procedureOwner.SetData<VarString>("NextScene", AssetUtility.MenuSceneName);
            GameEntry.Cutscene.PlayCutscene(DoChangeState);
        }

        public void Restart()
        {
            _procedureOwner.SetData<VarString>("NextScene", AssetUtility.GetLevelSceneSubName(_levelIndex));
            GameEntry.Cutscene.PlayCutscene(DoChangeState);
        }

        public void NextLevel()
        {
            _levelIndex++;
            Debug.Log($"level:{_levelIndex}");
            if (_levelIndex > AssetUtility.LevelCount)
            {
                Debug.Log("change to end");
                _procedureOwner.SetData<VarString>("NextScene", AssetUtility.EndSceneName);
                ChangeState<ProcedureChangeScene>(_procedureOwner);
                return;
            }

            _procedureOwner.SetData<VarInt32>("LevelIndex", _levelIndex);
            _procedureOwner.SetData<VarString>("NextScene", AssetUtility.GetLevelSceneSubName(_levelIndex));
            GameEntry.Cutscene.PlayCutscene(DoChangeState);
        }

        public void ChooseLevel(int index)
        {
            _levelIndex = index;
            _procedureOwner.SetData<VarInt32>("LevelIndex", _levelIndex);
            _procedureOwner.SetData<VarString>("NextScene", AssetUtility.GetLevelSceneSubName(_levelIndex));
            GameEntry.Cutscene.PlayCutscene(DoChangeState);
        }

        private void DoChangeState()
        {
            ChangeState<ProcedureChangeScene>(_procedureOwner);
        }
    }
}