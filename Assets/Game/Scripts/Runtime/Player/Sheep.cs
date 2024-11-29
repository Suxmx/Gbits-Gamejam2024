using System;
using ImprovedTimers;
using UnityEngine;

namespace GameMain
{
    public class Sheep : GameEntityBase
    {
        public Rigidbody Rigid => _rigidbody;
        public bool Arrival = false;
        [SerializeField] private float speed = 5;

        private Rigidbody _rigidbody;
        private Transform _graphics;

        private bool _bFacingRight;
        private CountdownTimer _dieTimer,_portalTimer;
        private CapsuleCollider _capsuleCollider;
        private bool _canTeleport=true;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _dieTimer?.Dispose();
        }

        protected override void OnInit()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponentInChildren<CapsuleCollider>();
            _graphics = transform.Find("Graphics");
            _bFacingRight = true;
            _dieTimer = new CountdownTimer(1f, false);
            _dieTimer.OnCompleted += Die;
            
            _portalTimer = new CountdownTimer(0.5f, false);
            _portalTimer.OnCompleted += () => _canTeleport = true;
        }

        protected override void OnAfterInit()
        {
        }

        protected override void OnFixedUpdate()
        {
            var targetSpeed = _bFacingRight ? speed : -speed;
            float vxDelta = (targetSpeed - _rigidbody.linearVelocity.x);
            _rigidbody.AddForce(Vector3.right * (vxDelta * 5), ForceMode.Acceleration);
            //判断是否被卡住
            if (CheckIfStuck() && !_dieTimer.IsRunning)
            {
                _dieTimer.Restart();
                // Debug.Log("Stuck timer start");
            }
            else
            {
                // Debug.Log("Stuck timer stop");
                _dieTimer.Stop();
            }
        }

        private float _lastChangeFacingTime = -1f;

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Wall") && Time.fixedTime - _lastChangeFacingTime > 0.04f)
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
                _lastChangeFacingTime = Time.fixedTime;
                ChangeFacing(!_bFacingRight);
            }
        }

        private Collider[] _tmpColliders = new Collider[10];

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (_capsuleCollider is null) return;
            // 设置 Gizmos 的颜色
            Gizmos.color = Color.green;

            // 绘制胶囊体
            float offset = 0.2f;
            var radius = _capsuleCollider.radius - offset;
            Vector3 point1 = transform.TransformPoint(_capsuleCollider.center +
                                                      Vector3.right * (_capsuleCollider.height / 2 - offset -
                                                                       radius));
            Vector3 point2 = transform.TransformPoint(_capsuleCollider.center -
                                                      Vector3.right * (_capsuleCollider.height / 2 - offset -
                                                                       radius));
            DrawWireCapsule(point1, point2, radius);
        }

        private void DrawWireCapsule(Vector3 point1, Vector3 point2, float radius)
        {
            // 计算两个点的方向和长度
            Vector3 direction = point2 - point1;
            float height = direction.magnitude;

            // 如果两个点重合，直接画球体
            if (Mathf.Approximately(height, 0f))
            {
                Gizmos.DrawWireSphere(point1, radius);
                return;
            }

            // 计算胶囊体方向
            Vector3 upDirection = direction.normalized;

            // 绘制两个球体端点
            Gizmos.DrawWireSphere(point1, radius);
            Gizmos.DrawWireSphere(point2, radius);

            // 绘制中间的圆柱体部分
            float cylinderHeight = height - (2 * radius); // 圆柱高度
            if (cylinderHeight > 0)
            {
                Quaternion rotation = Quaternion.LookRotation(upDirection); // 朝向
                Matrix4x4 oldMatrix = Gizmos.matrix;

                Gizmos.matrix = Matrix4x4.TRS((point1 + point2) * 0.5f, rotation, Vector3.one); // 设置变换矩阵
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(2 * radius, cylinderHeight, 2 * radius)); // 绘制圆柱部分的包围盒

                Gizmos.matrix = oldMatrix; // 恢复原始矩阵
            }
        }

        #endregion

        private bool CheckIfStuck()
        {
            float offset = 0.1f;
            var radius = _capsuleCollider.radius - offset;
            Vector3 point1 = transform.TransformPoint(_capsuleCollider.center +
                                                      Vector3.right * (_capsuleCollider.height / 2 - offset -
                                                                       radius));
            Vector3 point2 = transform.TransformPoint(_capsuleCollider.center -
                                                      Vector3.right * (_capsuleCollider.height / 2 - offset -
                                                                       radius));

            int size = Physics.OverlapCapsuleNonAlloc(point1, point2, radius, _tmpColliders,
                LayerMask.GetMask("SheepInteract"));
            for (int i = 0; i < size; i++)
            {
                if (_tmpColliders[i].transform != transform && !_tmpColliders[i].transform.IsChildOf(transform))
                {
                    return true;
                }
            }

            return false;
        }


        #region 公开

        public void ChangeFacing(bool bRight)
        {
            if (bRight != _bFacingRight)
            {
                var v = _rigidbody.linearVelocity;
                v.x = -v.x;
                _rigidbody.linearVelocity = v;
            }

            _bFacingRight = bRight;
        }

        public void ChangeVelocityTo(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        }

        public void AddForce(Vector3 force, ForceMode mode)
        {
            _rigidbody.AddForce(force, mode);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void Teleport(Vector3 pos)
        {
            if (!_canTeleport) return;
            _canTeleport = false;
            _portalTimer.Restart();
            transform.position = pos;
        }

        #endregion
    }
}