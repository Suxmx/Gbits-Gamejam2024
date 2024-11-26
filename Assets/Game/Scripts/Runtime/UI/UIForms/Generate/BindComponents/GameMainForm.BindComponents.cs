using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/11/26 17:21:43
namespace GameMain
{

	public partial class GameMainForm
	{

		private Button m_btn_Start;
		private Button m_btn_Restart;
		private Button m_btn_Pause;
		private Text m_txt_FrameRate;
		private RectTransform m_rect_BuildItem;
		private RectTransform m_rect_Toolbar;
		private Button m_btn_Undo;
		private Button m_btn_Build;
		private Button m_btn_Move;
		private Button m_btn_Remove;
		private RectTransform m_rect_PauseMenu;
		private Button m_btn_ResumeGame;
		private Button m_btn_ReturnMenu;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_Start = autoBindTool.GetBindComponent<Button>(0);
			m_btn_Restart = autoBindTool.GetBindComponent<Button>(1);
			m_btn_Pause = autoBindTool.GetBindComponent<Button>(2);
			m_txt_FrameRate = autoBindTool.GetBindComponent<Text>(3);
			m_rect_BuildItem = autoBindTool.GetBindComponent<RectTransform>(4);
			m_rect_Toolbar = autoBindTool.GetBindComponent<RectTransform>(5);
			m_btn_Undo = autoBindTool.GetBindComponent<Button>(6);
			m_btn_Build = autoBindTool.GetBindComponent<Button>(7);
			m_btn_Move = autoBindTool.GetBindComponent<Button>(8);
			m_btn_Remove = autoBindTool.GetBindComponent<Button>(9);
			m_rect_PauseMenu = autoBindTool.GetBindComponent<RectTransform>(10);
			m_btn_ResumeGame = autoBindTool.GetBindComponent<Button>(11);
			m_btn_ReturnMenu = autoBindTool.GetBindComponent<Button>(12);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
