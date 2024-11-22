using Autobind;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/11/6 23:42:34
namespace GameMain
{

	public partial class DialogueForm
	{

		private TextMeshProUGUI m_tmp_content;
		private Button m_btn_Next;
		private TextMeshProUGUI m_tmp_title;
		private RectTransform m_rect_Choices;
		private RectTransform m_rect_Actor;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_tmp_content = autoBindTool.GetBindComponent<TextMeshProUGUI>(0);
			m_btn_Next = autoBindTool.GetBindComponent<Button>(1);
			m_tmp_title = autoBindTool.GetBindComponent<TextMeshProUGUI>(2);
			m_rect_Choices = autoBindTool.GetBindComponent<RectTransform>(3);
			m_rect_Actor = autoBindTool.GetBindComponent<RectTransform>(4);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
