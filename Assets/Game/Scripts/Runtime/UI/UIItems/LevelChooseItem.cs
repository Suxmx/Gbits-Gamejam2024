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

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnClickEnter);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveListener(OnClickEnter);
        }


        private void OnClickEnter()
        {
            GameEntry.Event.Fire(this,ChooseLevelArgs.Create());
        }
    }
}