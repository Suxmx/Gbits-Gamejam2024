using System;
using UnityEngine;

namespace GameMain
{
    public class Sheep : GameEntityBase
    {
        [SerializeField] private float speed = 5;

        private Rigidbody _rigidbody;
        private Transform _graphics;

        private bool _bFacingRight;


        protected override void OnInit()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _graphics = transform.Find("Graphics");
            _bFacingRight = true;
        }

        protected override void OnAfterInit()
        {
        }

        protected override void OnFixedUpdate()
        {
            var targetSpeed = _bFacingRight ? speed : -speed;
            float vxDelta = (targetSpeed - _rigidbody.linearVelocity.x);
            _rigidbody.AddForce(Vector3.right * (vxDelta * 5), ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                bool flag = false;
                foreach (var contact in other.contacts)
                {
                    if (!(Vector3.Dot(other.contacts[0].normal, Vector3.up) > 0.9f ||
                         Vector3.Dot(other.contacts[0].normal, Vector3.up) < -0.9f))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag) return;

                _bFacingRight = !_bFacingRight;
                var v = _rigidbody.linearVelocity;
                v.x = -v.x;
                _rigidbody.linearVelocity = v;
            }
        }

        #region 公开

        public void ChangeFacing(bool bRight)
        {
            _bFacingRight = bRight;
        }

        public void ChangeVelocityTo(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        }

        public void AddForce(Vector3 force,ForceMode mode = ForceMode.Impulse)
        {
            _rigidbody.AddForce(force,mode);
        }

        #endregion
    }
}