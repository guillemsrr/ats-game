// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class UtilsLibrary
    {
        public static T RandomElement<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                return default;
                //throw new System.ArgumentException("Array cannot be null or empty");
            }

            return array[Random.Range(0, array.Length)];
        }

        public static T RandomElement<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default;
                //throw new System.ArgumentException("List cannot be null or empty");
            }

            return list[Random.Range(0, list.Count)];
        }

        public static void ShuffleList<T>(List<T> list)
        {
            if (list == null || list.Count <= 1)
            {
                return;
            }

            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}