// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.Localization;

namespace Resume.Data
{
    [CreateAssetMenu(menuName = "Resume/FunnyExperiences")]
    public class FunnyExperiencesData: ScriptableObject
    {
        [Tooltip("Funny concepts like 'space raccoons', 'haunted printers'...")]
        public LocalizedString[] KeyElement;

        [Tooltip("Templates like 'For many years I had to deal with {0}'")]
        public LocalizedString[] Descriptions;
    }
}