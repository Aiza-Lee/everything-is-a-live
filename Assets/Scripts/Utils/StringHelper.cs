using System.Collections.Generic;

namespace GameLogic.Utils {
    /// <summary>
    /// 字符串分割与通用工具类
    /// </summary>
    public static class StringHelper {
        /// <summary>
        /// 将逗号分割的字符串转为List<string>，自动去除空项和首尾空格
        /// </summary>
        public static List<string> SplitCommaList(string str) {
            if (string.IsNullOrWhiteSpace(str)) return new List<string>();
            var arr = str.Split(',');
            var list = new List<string>();
            foreach (var s in arr) {
                var trimmed = s.Trim();
                if (!string.IsNullOrEmpty(trimmed)) list.Add(trimmed);
            }
            return list;
        }
    }
}
