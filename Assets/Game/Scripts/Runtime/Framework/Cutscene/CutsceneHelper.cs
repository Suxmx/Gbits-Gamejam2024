using System;
using Game.Scripts.Runtime.Cutscene;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Runtime
{
    public class CutsceneHelper : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void OnCutsceneEnterEnd()
        {
            Debug.Log("OnCutsceneEnterEnd");
            UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>().Fire(this,OnCutsceneEnterArgs.Create());
        }
        public void OnCutsceneFadeEnd()
        {
            Debug.Log("fade endddd");
            UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>().Fire(this,OnCutsceneFadeArgs.Create());
            gameObject.SetActive(false);
        }
    }
}