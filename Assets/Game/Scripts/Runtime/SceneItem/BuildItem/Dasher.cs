using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class Dasher : BuildItemBase
    {
        [SerializeField] private Vector3 _boxSize;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _force = 30;
        private Vector3 _direction => transform.rotation * Vector3.right;
        private bool _bEnableLogic = false;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _direction * 1.5f);
            Gizmos.color = Color.white;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + _offset, _boxSize);
        }

        public override void EnableLogic()
        {
            Rigid.linearVelocity = Vector3.zero;
            _bEnableLogic = true;
        }

        public override void DisableLogicWhenBuilding()
        {
            _bEnableLogic = false;
        }

        public override bool DetectBuildable()
        {
            var size = Physics.OverlapBoxNonAlloc(transform.position + _offset, _boxSize / 2, _tmpColliders,
                transform.rotation, _cantBuildLayer, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < Mathf.Min(size, 10); i++)
            {
                if (_tmpColliders[i].transform != transform && !_tmpColliders[i].transform.IsChildOf(transform))
                {
                    return false;
                }
            }

            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_bEnableLogic) return;
            Sheep sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                var v = sheep.Rigid.linearVelocity;
                if (Vector3.Dot(_direction, Vector3.up) < 0.5 && Vector3.Dot(v, _direction) < -0.2f)
                {
                    var magnitude = v.magnitude;
                    Rigid.linearVelocity = _direction * magnitude;
                    sheep.ChangeFacing(_direction.x > 0);
                }

                sheep.AddForce(_direction * _force, ForceMode.Impulse);
                // _dashSheepMap[sheep] = Time.fixedTime;
            }
        }
    }
}