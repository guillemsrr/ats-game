// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.Localization;

namespace Resume.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Resume/CompanyData")]
    public class CompanyData : ScriptableObject
    {
        public string[] Companies;
        public LocalizedString[] Descriptions;
    }
}