using Autobind;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/12/1 5:55:52
namespace GameMain
{

	public partial class GameMainForm
	{

		private Text m_txt_FrameRate;
		private Image m_img_Mode;
		private RectTransform m_rect_BuildItem;
		private RectTransform m_rect_Toolbar;
		private Button m_btn_ExitPlay;
		private Button m_btn_Start;
		private Button m_btn_Undo;
		private Button m_btn_Restart;
		private Button m_btn_Remove;
		private RectTransform m_rect_PauseMenu;
		private Button m_btn_ResumeGame;
		private Button m_btn_ReturnMenu;
		private TextMeshProUGUI m_tmp_ArriveSheep;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_txt_FrameRate = autoBindTool.GetBindComponent<Text>(0);
			m_img_Mode = autoBindTool.GetBindComponent<Image>(1);
			m_rect_BuildItem = autoBindTool.GetBindComponent<RectTransform>(2);
			m_rect_Toolbar = autoBindTool.GetBindComponent<RectTransform>(3);
			m_btn_ExitPlay = autoBindTool.GetBindComponent<Button>(4);
			m_btn_Start = autoBindTool.GetBindComponent<Button>(5);
			m_btn_Undo = autoBindTool.GetBindComponent<Button>(6);
			m_btn_Restart = autoBindTool.GetBindComponent<Button>(7);
			m_btn_Remove = autoBindTool.GetBindComponent<Button>(8);
			m_rect_PauseMenu = autoBindTool.GetBindComponent<RectTransform>(9);
			m_btn_ResumeGame = autoBindTool.GetBindComponent<Button>(10);
			m_btn_ReturnMenu = autoBindTool.GetBindComponent<Button>(11);
			m_tmp_ArriveSheep = autoBindTool.GetBindComponent<TextMeshProUGUI>(12);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
