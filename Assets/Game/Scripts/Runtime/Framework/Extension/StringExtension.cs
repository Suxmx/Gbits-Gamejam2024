using UnityEngine;

namespace GameMain
{
    public static class StringExtension
    {
        public static bool TryParseToVector3(this string vectorString, out Vector3 value)
        {
            // 去掉可能存在的空格并分割字符串
            value = Vector3.zero;
            string[] values = vectorString.Replace(" ", "").Split(','); // 确保分割后有三个值

            if (values.Length != 3)
            {
                return false;
            } 

            if (!float.TryParse(values[0], out var x)) return false;
            if (!float.TryParse(values[1], out var y)) return false;
            if (!float.TryParse(values[2], out var z)) return false;
            value = new Vector3(x, y, z);
            return true;
        }
    }
}