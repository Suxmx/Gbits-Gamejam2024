using System;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class BuildItemUI : MonoBehaviour
    {
        private Button _btn;
        private EBuildItem _buildItem;
        private TextMeshProUGUI _countTmp;

        public void Init(EBuildItem buildItem,int count)
        {
            _buildItem = buildItem;
            _countTmp = GetComponentInChildren<TextMeshProUGUI>();
            _countTmp.text = count.ToString();
            GameEntry.Event.Subscribe(OnBuildItemCountChangeArgs.EventId, OnBuildItem);
        }

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log($"start build {_buildItem}");
            GameManager.Build.StartBuild(_buildItem);
        }
        
        private void OnBuildItem(object sender, GameEventArgs e)
        {
            var args = (OnBuildItemCountChangeArgs)e;
            if (args.BuildItem == _buildItem)
            {
                _countTmp.text = args.Count.ToString();
            }
        }
    }
}