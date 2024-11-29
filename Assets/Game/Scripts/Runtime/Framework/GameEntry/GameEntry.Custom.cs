//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Game.Scripts.Runtime.Audio;
using Game.Scripts.Runtime.Cutscene;
using Game.Scripts.Runtime.Input;
using JSAM;
using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 获取声音组件。
        /// </summary>
        public static AudioComponent Audio { get; private set; }
        public static InputComponent Input { get;private set; }
        public static CutsceneComponent Cutscene { get; private set; }

        private void InitCustomComponents()
        {
            Audio = UnityGameFramework.Runtime.GameEntry.GetComponent<AudioComponent>();
            Input = UnityGameFramework.Runtime.GameEntry.GetComponent<InputComponent>();
            Cutscene = UnityGameFramework.Runtime.GameEntry.GetComponent<CutsceneComponent>();
        }
    }
}