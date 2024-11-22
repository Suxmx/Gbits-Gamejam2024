using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/11/4 1:18:29
namespace GameMain
{

	public partial class MenuForm
	{

		private Button m_btn_StartGame;
		private Button m_btn_Setting;
		private Button m_btn_ExitGame;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_StartGame = autoBindTool.GetBindComponent<Button>(0);
			m_btn_Setting = autoBindTool.GetBindComponent<Button>(1);
			m_btn_ExitGame = autoBindTool.GetBindComponent<Button>(2);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
