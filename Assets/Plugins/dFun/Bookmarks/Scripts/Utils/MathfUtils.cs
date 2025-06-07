using UnityEngine;

namespace DFun.Bookmarks
{
    public static class MathUtils
    {
        public static float RemapLinear(float input, float inputMin, float inputMax, float resultMin, float resultMax)
        {
            return (resultMax - resultMin) * (Mathf.Max(input - inputMin, 0) / (inputMax - inputMin)) + resultMin;
        }
    }
}