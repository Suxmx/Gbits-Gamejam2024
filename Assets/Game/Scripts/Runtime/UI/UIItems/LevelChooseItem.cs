using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class LevelChooseItem : MonoBehaviour
    {
        private Button _btn;

        private void Awake()
        {
            _btn = transform.Find("Mask").GetComponent<Button>();
        }

        public void OnOpen()
        {
            // Debug.Log("onopen");
            _btn.onClick.AddListener(OnClickEnter);
        }

        public void OnClose()
        {
            _btn.onClick.RemoveListener(OnClickEnter);
        }

        private void OnClickEnter()
        {
            // Debug.Log("click");
            GameEntry.Event.Fire(this, ChooseLevelArgs.Create());
        }
    }
}