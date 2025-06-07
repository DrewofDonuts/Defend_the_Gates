using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

namespace DFun.UnityDataTypes
{
    /// <summary>
    /// Based on UnityEditor.ClipboardParser to keep compatibility with editor Copy/Paste actions
    /// </summary>
    public static class ContextMenuHelper
    {
        public static string CopyBuffer
        {
            get => EditorGUIUtility.systemCopyBuffer;
            set => EditorGUIUtility.systemCopyBuffer = value;
        }
        
        public static float[] ParseFloats(string text, string prefix, int count)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            // build a regex that matches "Prefix(a,b,c,...)" at start of text
            StringBuilder sb = new StringBuilder();
            sb.Append('^');
            sb.Append(prefix);
            sb.Append("\\(");
            for (var i = 0; i < count; ++i)
            {
                if (i != 0)
                    sb.Append(',');
                sb.Append("([^,]+)");
            }
            sb.Append("\\)");

            Match match = Regex.Match(text, sb.ToString());
            if (!match.Success || match.Groups.Count <= count)
                return null;

            float[] res = new float[count];
            for (var i = 0; i < count; ++i)
            {
                if (float.TryParse(match.Groups[i + 1].Value, NumberStyles.Float, CultureInfo.InvariantCulture,
                        out var f))
                    res[i] = f;
                else
                    return null;
            }
            return res;
        }
    }
}