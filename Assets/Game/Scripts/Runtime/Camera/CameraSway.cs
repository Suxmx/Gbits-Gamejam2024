using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CameraSway : MonoBehaviour
    {
        private Vector3 originalPosition;
        public float shakeAmount = 0.1f; // 晃动幅度
        public float shakeSpeed = 2.0f; // 晃动速度

        private void Start()
        {
            originalPosition = transform.localPosition;
        }

        private void Update()
        {
            // 在每一帧应用一个正弦函数来模拟晃动效果
            float shakeOffsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            float shakeOffsetY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;

            transform.localPosition = originalPosition + new Vector3(shakeOffsetX, shakeOffsetY, 0);
        }
    }
}