using System;
using UnityEngine;

namespace GameMain
{
    public class Bouncer : BuildItemBase
    {
        private Vector3 _direction => transform.rotation * Vector3.up;
        private bool _bdontChange => Mathf.Abs(_direction.x) < 1e-2f;
        private bool _bFacingRight => _direction.x > 0;
        private bool _bEnableLogic = false;

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, _direction * 3);
        }

        public override void DisableLogicWhenBuilding()
        {
            GetComponent<Rigidbody>().useGravity = false;
            _bEnableLogic = false;
        }

        public override void EnableLogic()
        {
            if (!_bIsStatic)
                GetComponent<Rigidbody>().useGravity = true;
            _bEnableLogic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_bEnableLogic) return;
            Sheep sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                if (!_bdontChange)
                {
                    sheep.ChangeFacing(_bFacingRight);
                }

                sheep.ChangeVelocityTo(_direction * 15);
            }
        }
    }
}