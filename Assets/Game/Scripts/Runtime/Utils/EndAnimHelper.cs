using System;
using UnityEngine;

namespace GameMain
{
    public class EndAnimHelper : MonoBehaviour
    {
        private bool _anim1End, _anim2End;
        private Animator _animator;

        public void StartEndAnim()
        {
            _animator ??= GetComponent<Animator>();
            _animator.Play("EndAnim1");
        }

        public void OnEndAnim2End()
        {
            _anim2End = true;
        }

        public void OnEndAnim1End()
        {
            _anim1End = true;
        }

        private void Update()
        {
            if (_anim1End && Input.anyKeyDown)
            {
                _anim1End = false;
                _animator.Play("EndAnim2");
            }

            if (_anim2End && Input.anyKeyDown)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}