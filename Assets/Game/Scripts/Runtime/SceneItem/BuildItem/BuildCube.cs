using System;
using UnityEngine;

namespace GameMain
{
    public class BuildCube : BuildItemBase
    {
        [SerializeField] private Vector3 _boxSize;
        [SerializeField] private Vector3 _offset;
        private bool _bOnGround = false;

        public override void EnableLogic()
        {
            Rigid.linearVelocity = Vector3.zero;
            if (!_bIsStatic)
                Rigid.useGravity = true;
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = true;
            }
        }

        public override void DisableLogicWhenBuilding()
        {
            Rigid.useGravity = false;
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + _offset, _boxSize);
        }

        public override bool DetectBuildable()
        {
            var size = Physics.OverlapBoxNonAlloc(transform.position + _offset, _boxSize / 2, _tmpColliders,
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

        private void OnCollisionEnter(Collision other)
        {
            if (_bOnGround) return;
            Sheep sheep = other.gameObject.GetComponentInParent<Sheep>();
            if (sheep)
            {
                if (Vector3.Dot(other.contacts[0].normal, Vector3.up) > 0.9)
                {
                    Destroy(sheep.gameObject);
                }
            }
            else
            {
                _bOnGround = true;
            }
        }
    }
}