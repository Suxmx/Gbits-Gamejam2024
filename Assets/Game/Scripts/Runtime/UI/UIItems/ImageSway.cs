using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class ImageSway : MonoBehaviour
    {
        private Vector2 origin;
        public float shakeAmount = 0.1f; // 晃动幅度
        public float shakeSpeed = 2.0f; // 晃动速度
        private RawImage _img;

        private void Awake()
        {
            _img = GetComponent<RawImage>();
            origin = _img.uvRect.position;
        }


        private void Update()
        {
            // 在每一帧应用一个正弦函数来模拟晃动效果
            float shakeOffsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            float shakeOffsetY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;
            var rect = _img.uvRect;
            rect.position = origin + new Vector2(shakeOffsetX, shakeOffsetY) / 10f;
            _img.uvRect = rect;
        }
    }
}