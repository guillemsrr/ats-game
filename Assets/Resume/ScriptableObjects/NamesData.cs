// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;

namespace Resume.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ResumeGen/NamesData")]
    public class NamesData : ScriptableObject
    {
        public string[] FirstNames;
        public string[] LastNames;
    }
}