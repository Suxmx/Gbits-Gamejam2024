using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class BuildItemUI : MonoBehaviour
    {
        private Button _btn;
        private EBuildItem _buildItem;

        public void Init(EBuildItem buildItem,Sprite icon)
        {
            _buildItem = buildItem;
            transform.Find("Icon").GetComponent<Image>().sprite = icon;
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
            GameManager.Build.StartBuild(_buildItem);
        }
    }
}