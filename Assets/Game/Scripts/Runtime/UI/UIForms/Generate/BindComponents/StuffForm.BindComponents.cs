using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/12/1 16:30:12
namespace GameMain
{

	public partial class StuffForm
	{

		private Button m_btn_ReturnMenu;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_ReturnMenu = autoBindTool.GetBindComponent<Button>(0);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
