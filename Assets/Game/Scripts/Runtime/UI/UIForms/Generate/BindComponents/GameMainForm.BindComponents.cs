using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/11/22 13:55:32
namespace GameMain
{

	public partial class GameMainForm
	{

		private Text m_txt_FrameRate;
		private Button m_btn_ExitGame;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_txt_FrameRate = autoBindTool.GetBindComponent<Text>(0);
			m_btn_ExitGame = autoBindTool.GetBindComponent<Button>(1);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
