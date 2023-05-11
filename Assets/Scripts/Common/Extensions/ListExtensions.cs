using System.Collections.Generic;

namespace Common.Extensions
{
    public static class ListExtensions
    {
        public static void RemoveRandomElement<T>(this List<T> list)
        {
            var randomIndex = UnityEngine.Random.Range(0, list.Count);
            list.RemoveAt(randomIndex);
        }
        
        public static T GetRandomElement<T>(this List<T> list)
        {
            var randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
        
        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            while (n > 1) 
            {
                n--;
                var k = UnityEngine.Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}