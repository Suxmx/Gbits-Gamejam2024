using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/11/30 15:16:51
namespace GameMain
{

	public partial class LevelChooseForm
	{

		private Button m_btn_ReturnMenu;
		private RectTransform m_rect_LevelGroup;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_ReturnMenu = autoBindTool.GetBindComponent<Button>(0);
			m_rect_LevelGroup = autoBindTool.GetBindComponent<RectTransform>(1);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
