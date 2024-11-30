using System;
using GameFramework.Event;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = GameMain.GameEntry;

namespace Game.Scripts.Runtime.Cutscene
{
    public class CutsceneComponent : UnityGameFramework.Runtime.GameFrameworkComponent
    {
        private Animator _animator;
        private GameObject _graphics;

        protected override void Awake()
        {
            base.Awake();
            _graphics = transform.Find("Graphics").gameObject;
            _animator = _graphics.GetComponent<Animator>();
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(OnCutsceneFadeArgs.EventId, OnCutsceneFadeEnd);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(OnCutsceneFadeArgs.EventId, OnCutsceneFadeEnd);
        }

        public void PlayCutscene(float speed=1)
        {
            _graphics.gameObject.SetActive(true);
            _animator.speed = speed;
            _animator.Play("Enter", 0, 0);
            _animator.Update(0);
        }

        public void FadeCutscene()
        {
            _animator.Play("Fade", 0, 0);
        }

        private void OnCutsceneFadeEnd(object sender, GameEventArgs e)
        {
            _graphics.gameObject.SetActive(false);
        }
        
    }
}