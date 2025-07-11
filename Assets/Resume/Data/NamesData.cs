// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;

namespace Resume.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Resume/NamesData")]
    public class NamesData : ScriptableObject
    {
        public string[] FirstNames;
        public string[] LastNames;

        public string GetRandomName()
        {
            return FirstNames[Random.Range(0, FirstNames.Length)] + " " + LastNames[Random.Range(0, LastNames.Length)];
        }
    }
}