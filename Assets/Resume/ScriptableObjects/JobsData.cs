// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;

namespace Resume.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ResumeGen/JobsData")]
    public class JobsData : ScriptableObject
    {
        public string[] JobTitles;
        public string[] Industries;
        public string[] Skills;
    }
}