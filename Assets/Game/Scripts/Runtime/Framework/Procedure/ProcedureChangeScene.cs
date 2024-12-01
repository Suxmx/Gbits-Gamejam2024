//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private bool _changeToMenu = false;
        private bool _changeToMain = false;
        private bool _changeToSplash = false;

        private bool m_IsChangeSceneComplete = false;
        private bool _pendingLoadScene;
        private string _loadSceneName;
        private bool _hasCutscene = false;

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //初始化场景切换状态
            m_IsChangeSceneComplete = false;
            _pendingLoadScene = false;
            _hasCutscene = procedureOwner.HasData("PlayCutscene") && procedureOwner.GetData<VarBoolean>("PlayCutscene");

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(UnloadSceneSuccessEventArgs.EventId, OnUnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            GameEntry.Event.Subscribe(OnCutsceneEnterArgs.EventId, OnCutsceneEnter);

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            string sceneName = procedureOwner.GetData<VarString>("NextScene");
            _loadSceneName = sceneName;
            if (!_hasCutscene)
            {
                DoChangeScene(sceneName);
            }
            else
            {
                GameEntry.Cutscene.PlayCutscene();
            }
        }

        private void DoChangeScene(string sceneName)
        {
            // 停止所有声音
            GameEntry.Audio.StopAllSounds();

            // 隐藏所有实体
            GameEntry.Entity.HideAllLoadingEntities();
            GameEntry.Entity.HideAllLoadedEntities();

            // 卸载所有场景
            List<string> loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames().ToList();
            for (int i = 0; i < loadedSceneAssetNames.Count; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }


            _changeToMenu = sceneName == AssetUtility.MenuSceneName;
            _changeToMain = sceneName.Contains("Level");
            _changeToSplash = sceneName == AssetUtility.SplashSceneName;

            //如果是重新加载当前scene
            if (loadedSceneAssetNames.FindIndex(x => x == AssetUtility.GetSceneAsset(sceneName)) < 0)
            {
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(sceneName), AssetPriority.SceneAsset, this);
            }
            else
            {
                _pendingLoadScene = true;
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (_hasCutscene)
            {
                GameEntry.Cutscene.FadeCutscene();
            }

            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(UnloadSceneSuccessEventArgs.EventId, OnUnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            GameEntry.Event.Unsubscribe(OnCutsceneEnterArgs.EventId, OnCutsceneEnter);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_IsChangeSceneComplete)
            {
                return;
            }


            if (_changeToMenu)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }
            else if (_changeToMain)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
            else if (_changeToSplash)
            {
                ChangeState<ProcedureSplash>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            // if (m_BackgroundMusicId > 0)
            // {
            //     GameEntry.Sound.PlayMusic(m_BackgroundMusicId);
            // }

            m_IsChangeSceneComplete = true;
        }

        private void OnUnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            if (_pendingLoadScene)
            {
                _pendingLoadScene = false;
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(_loadSceneName), AssetPriority.SceneAsset,
                    this);
            }
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName,
                ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }

        private void OnCutsceneEnter(object sender, GameEventArgs e)
        {
            if (!_hasCutscene) return;

            // Debug.Log("Cutscene enter");
            DoChangeScene(_loadSceneName);
            if (GameEntry.UI.GetUIFormById(UIFormId.MenuForm) &&
                GameEntry.UI.GetUIFormById(UIFormId.MenuForm).gameObject.activeInHierarchy)
            {
                GameEntry.UI.CloseUIFormById(UIFormId.MenuForm);
            }

            if (GameEntry.UI.GetUIFormById(UIFormId.LevelChooseForm) &&
                GameEntry.UI.GetUIFormById(UIFormId.LevelChooseForm).gameObject.activeInHierarchy)
            {
                GameEntry.UI.CloseUIFormById(UIFormId.LevelChooseForm);
            }
        }
    }
}