using UnityEngine;

namespace UnityHelpers.Utils
{
    public static class AlgorithmUtils
    {
        public static void ShuffleArray<T>(T[] arr)
        {
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int r = Random.Range(0, i);
                T tmp = arr[i];
                arr[i] = arr[r];
                arr[r] = tmp;
            }
        }
    }
}