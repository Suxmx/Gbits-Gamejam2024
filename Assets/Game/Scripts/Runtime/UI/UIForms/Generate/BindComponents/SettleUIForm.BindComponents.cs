using Autobind;
using TMPro;
using UnityEngine;

//自动生成于：2024/12/1 7:14:21
namespace GameMain
{

	public partial class SettleUIForm
	{

		private RectTransform m_rect_Circle;
		private TextMeshProUGUI m_tmp_Settle;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_rect_Circle = autoBindTool.GetBindComponent<RectTransform>(0);
			m_tmp_Settle = autoBindTool.GetBindComponent<TextMeshProUGUI>(1);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
