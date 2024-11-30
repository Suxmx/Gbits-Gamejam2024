using System;
using UnityEngine;

namespace GameMain
{
    public class Bouncer : BuildItemBase
    {
        [SerializeField] private Vector3 _boxSize;
        [SerializeField] private Vector3 _offset;
        private Vector3 _direction => transform.rotation * Vector3.up;
        private bool _bdontChange => Mathf.Abs(_direction.x) < 1e-2f;
        private bool _bFacingRight => _direction.x > 0;
        private bool _bEnableLogic = false;
        private Collider _boxCollider;

        private void OnDrawGizmos()
        {
            var boxSize = _boxSize;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + _offset, boxSize);
        }

        public override void EnableLogic()
        {
            _boxCollider??=GetComponent<Collider>();
            _boxCollider.enabled = true;
            Rigid.linearVelocity = Vector3.zero;
            
            if (!_bIsStatic)
                Rigid.useGravity = true;
            _bEnableLogic = true;
        }

        public override void DisableLogicWhenBuilding()
        {
            _boxCollider??=GetComponent<Collider>();
            _boxCollider.enabled = false;
            Rigid.useGravity = false;
            _bEnableLogic = false;
        }


        public override bool DetectBuildable()
        {
            var boxSize = _boxSize;
            boxSize.x *= transform.localScale.x;
            boxSize.y *= transform.localScale.y;
            boxSize.z *= transform.localScale.z;
            var size = Physics.OverlapBoxNonAlloc(transform.position + _offset, boxSize / 2, _tmpColliders,
                transform.rotation, _cantBuildLayer,QueryTriggerInteraction.Ignore);
            for (int i = 0; i < Mathf.Min(size, 10); i++)
            {
                if (_tmpColliders[i].transform != transform && !_tmpColliders[i].transform.IsChildOf(transform))
                {
                    return false;
                }
            }

            return true;
        }


        private void OnTriggerStay(Collider other)
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
                // Debug.Log("trigger");
            }
        }
    }
}