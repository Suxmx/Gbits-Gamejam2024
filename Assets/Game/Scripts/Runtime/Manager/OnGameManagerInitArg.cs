//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-24 17:42:53
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace Echo
{
    public class OnGameManagerInitArg : GameEventArgs
    {
        public static readonly int EventId = typeof(OnGameManagerInitArg).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public static OnGameManagerInitArg Create()
        {
            OnGameManagerInitArg args = ReferencePool.Acquire<OnGameManagerInitArg>();
            return args;
        }

        /// <summary>
        /// 清理打开界面成功事件。
        /// </summary>
        public override void Clear()
        {

        }
    }
}