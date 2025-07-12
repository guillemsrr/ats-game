// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Resume.Data.Requirements;
using Resume.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace Resume.Data
{
    [CreateAssetMenu(menuName = "Resume/JobArchetype")]
    public class JobArchetype : ScriptableObject
    {
        public string[] JobTitles;
        public string[] Skills;

        public LocalizedString[] SoftSkills;
        public LocalizedString[] Summary;
        public RequirementData[] Requirements;

        public CompanyData[] CompaniesData;
        public EducationData[] EducationData;
        public FunnyExperiencesData FunnyExperiences;

        public string[] GetRandomSkills(int max)
        {
            int count = Random.Range(1, max);
            return Skills.OrderBy(x => Random.value).Take(count).ToArray();
        }

        public LocalizedString[] GetRandomSoftSkills(int max)
        {
            int count = Random.Range(1, max);
            return SoftSkills.Select(x => x).OrderBy(x => Random.value).Take(count).ToArray();
        }
    }
}