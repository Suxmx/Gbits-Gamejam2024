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
        
        public void PlayCutscene()
        {
            gameObject.SetActive(true);
            _animator.Play("Enter",0,0);
        }
        
        public void FadeCutscene()
        {
            _animator.SetTrigger("Fade");
        }

        public void OnCutsceneEnterEnd()
        {
            Debug.Log("OnCutsceneEnterEnd");
            UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>().Fire(this,OnCutsceneEnterArgs.Create());
        }
        public void OnCutsceneFadeEnd()
        {
            UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>().Fire(this,OnCutsceneFadeArgs.Create());
            gameObject.SetActive(false);
        }
    }
}