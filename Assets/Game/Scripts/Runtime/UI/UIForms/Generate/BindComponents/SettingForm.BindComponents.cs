using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/11/4 14:09:14
namespace GameMain
{

	public partial class SettingForm
	{

		private Button m_btn_volumeOff;
		private RectTransform m_trans_volumeOffChose;
		private Button m_btn_volumeOn;
		private RectTransform m_trans_volumeOnChose;
		private Slider m_slider_mainVolume;
		private Slider m_slider_musicVolume;
		private Slider m_slider_sfxVolume;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_volumeOff = autoBindTool.GetBindComponent<Button>(0);
			m_trans_volumeOffChose = autoBindTool.GetBindComponent<RectTransform>(1);
			m_btn_volumeOn = autoBindTool.GetBindComponent<Button>(2);
			m_trans_volumeOnChose = autoBindTool.GetBindComponent<RectTransform>(3);
			m_slider_mainVolume = autoBindTool.GetBindComponent<Slider>(4);
			m_slider_musicVolume = autoBindTool.GetBindComponent<Slider>(5);
			m_slider_sfxVolume = autoBindTool.GetBindComponent<Slider>(6);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
