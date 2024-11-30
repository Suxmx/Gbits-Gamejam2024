using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;

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

        public void PlayCutscene()
        {
            _graphics.gameObject.SetActive(true);
            _animator.Play("Enter",0,0);
        }

        public void FadeCutscene()
        {
            _animator.SetTrigger("Fade");
        }
    }
}