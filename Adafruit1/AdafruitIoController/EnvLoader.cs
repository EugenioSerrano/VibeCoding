using System;
using System.IO;
using System.Collections.Generic;

namespace AdafruitIoController
{
    public static class EnvLoader
    {
        public static Dictionary<string, string> Load(string path)
        {
            var dict = new Dictionary<string, string>();
            if (!File.Exists(path)) return dict;
            foreach (var line in File.ReadAllLines(path))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#")) continue;
                var idx = trimmed.IndexOf('=');
                if (idx < 0) continue;
                var key = trimmed.Substring(0, idx).Trim();
                var value = trimmed.Substring(idx + 1).Trim();
                dict[key] = value;
            }
            return dict;
        }
    }
}
