using System;
using UnityEngine;

namespace GameMain
{
    public class BuildCube : BuildItemBase
    {
        private bool _bOnGround = false;
        public override void EnableLogic()
        {
            if (!_bIsStatic)
                GetComponent<Rigidbody>().useGravity = true;
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = true;
            }
        }

        public override void DisableLogicWhenBuilding()
        {
            GetComponent<Rigidbody>().useGravity = false;
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if(_bOnGround) return;
            Sheep sheep = other.gameObject.GetComponentInParent<Sheep>();
            if (sheep)
            {
                Debug.Log(Vector3.Dot(other.contacts[0].normal, Vector3.up));
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