using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu(fileName = "LevelConfigsSO", menuName = "GameMain/LevelConfigsSO")]
    public class LevelConfigsSO : ScriptableObject
    {
        public List<LevelConfig> LevelConfigs;
    }

    [Serializable]
    public struct LevelConfig
    {
        public int Index;
        public List<LevelBuildItemConfig> AvailableBuildItems;
    }

    [Serializable]
    public struct LevelBuildItemConfig
    {
        public EBuildItem Item;
        public int Count;
    }
}