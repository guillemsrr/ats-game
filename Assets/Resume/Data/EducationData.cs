// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.Localization;

namespace Resume.Data
{
    [CreateAssetMenu(menuName = "Resume/EducationData")]
    public class EducationData : ScriptableObject
    {
        public string[] Degrees;
        public string[] Schools;
        public LocalizedString[] Description;
    }
}