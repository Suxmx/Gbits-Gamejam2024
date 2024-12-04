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
        public bool IsPlayingCutscene => _isPlayingCutscene;
        private Animator _animator;
        private GameObject _graphics;

        /// <summary>
        /// 若有其他回调不便在PlayCutscene中传入，可在此添加
        /// </summary>
        public Action OnEnterEnd;

        /// <summary>
        /// 若有其他回调不便在FadeCutscene中传入，可在此添加
        /// </summary>
        public Action OnFadeEnd;

        private bool _isPlayingCutscene = false;

        protected override void Awake()
        {
            base.Awake();
            _graphics = transform.Find("Graphics").gameObject;
            _animator = _graphics.GetComponent<Animator>();
        }

        public void PlayCutscene(Action onEnterEnd, float speed = 1)
        {
            if (_isPlayingCutscene) return;
            OnEnterEnd += onEnterEnd;
            _isPlayingCutscene = true;
            _graphics.gameObject.SetActive(true);
            _animator.speed = speed;
            _animator.Play("Enter", 0, 0);
        }

        public void FadeCutscene(Action onFadeEnd, float speed = 1)
        {
            if (!_isPlayingCutscene) return;
            OnFadeEnd += onFadeEnd;
            _animator.speed = speed;
            _animator.Play("Fade", 0, 0);
        }

        public void AnimEnterEnd()
        {
            OnEnterEnd?.Invoke();
        }

        public void AnimFadeEnd()
        {
            _isPlayingCutscene = false;
            OnFadeEnd?.Invoke();
            OnFadeEnd = null;
            OnEnterEnd = null;
            _graphics.gameObject.SetActive(false);
        }
    }
}