// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Resume.Data;
using Resume.Data.Requirements;
using UnityEngine;
using UnityEngine.Localization;

namespace Resume.ScriptableObjects
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

        public List<string> GetRandomSkills(int max)
        {
            int count = Random.Range(1, max);
            return Skills.OrderBy(x => Random.value).Take(count).ToList();
        }

        public List<string> GetRandomSoftSkills(int max)
        {
            int count = Random.Range(1, max);
            return SoftSkills.Select(x => x.GetLocalizedString()).OrderBy(x => Random.value).Take(count).ToList();
        }
    }
}