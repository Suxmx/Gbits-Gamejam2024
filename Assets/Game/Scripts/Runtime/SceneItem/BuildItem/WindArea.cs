using System;
using UnityEngine;

namespace GameMain
{
    public class WindArea : BuildItemBase
    {
        [SerializeField] private Vector3 _boxSize;
        [SerializeField] private Vector3 _offset;
        private Vector3 _direction => transform.rotation * Vector3.up;
        private BoxCollider _windArea;

        protected override void OnInit()
        {
            _windArea = transform.Find("TriggerArea").GetComponent<BoxCollider>();
        }

        public override void EnableLogic()
        {
            if (_windArea is null)
            {
                _windArea = transform.Find("TriggerArea").GetComponent<BoxCollider>();
            }

            Rigid.useGravity = true;
            Rigid.linearVelocity = Vector3.zero;
            _windArea.enabled = true;
        }

        public override void DisableLogicWhenBuilding()
        {
            if (_windArea is null)
            {
                _windArea = transform.Find("TriggerArea").GetComponent<BoxCollider>();
            }

            Rigid.useGravity = false;
            _windArea.enabled = false;
        }

        private void OnDrawGizmos()
        {
            var boxSize = _boxSize;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + _offset, boxSize);
        }

        public override bool DetectBuildable()
        {
            var boxSize = _boxSize;
            boxSize.x *= transform.localScale.x;
            boxSize.y *= transform.localScale.y;
            boxSize.z *= transform.localScale.z;
            var size = Physics.OverlapBoxNonAlloc(transform.position + _offset, boxSize / 2, _tmpColliders,
                transform.rotation, _cantBuildLayer);
            for (int i = 0; i < Mathf.Min(size, 10); i++)
            {
                if (_tmpColliders[i].transform != transform && !_tmpColliders[i].transform.IsChildOf(transform))
                {
                    return false;
                }
            }

            return true;
        }

        private const float windForce = 40;
        private float k => (-Physics.gravity.y - windForce) / _windArea.size.y;
        private float k2 => (-Physics.gravity.y - windForce) / Mathf.Pow(_windArea.size.y, 2);

        private void OnTriggerStay(Collider other)
        {
            // Debug.Log(Physics.gravity);
            var sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                var dist = Mathf.Abs(sheep.transform.position.y - transform.position.y);
                var force = k * dist + windForce;
                // var force = k2*dist*dist + windForce;
                sheep.AddForce(force * _direction, ForceMode.Acceleration);
                // Debug.Log($"add force:{force * _direction} dist:{dist} ");
            }
        }
    }
}