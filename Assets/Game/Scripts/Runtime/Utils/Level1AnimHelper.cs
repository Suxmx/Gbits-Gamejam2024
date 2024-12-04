using System;
using UnityEngine;

namespace GameMain
{
    public class Level1AnimHelper : MonoBehaviour
    {
        private Animator _animator;
        private bool _anim1End = false;
        private bool _tip1End = false;

        public void Play()
        {
            _animator ??= GetComponent<Animator>();
            _animator.Play("Anim1");
        }

        public void Anim1End()
        {
            _anim1End = true;
        }

        public void Anim2End()
        {
            transform.Find("Image").gameObject.SetActive(false);
            _animator.Play("TipAnim1");
        }

        public void TipAnim1End()
        {
            _tip1End = true;
        }

        public void TipAnim2End()
        {
            gameObject.SetActive(false);
            GameManager.Instance.ShowUIFormAndInitEnd();
        }

        private void Update()
        {
            if (Input.anyKey && _anim1End)
            {
                _anim1End = false;
                _animator.Play("Anim2");
            }

            if (Input.anyKey && _tip1End)
            {
                _tip1End = false;
                _animator.Play("TipAnim2");
            }
        }
    }
}