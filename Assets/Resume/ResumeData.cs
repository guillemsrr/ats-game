// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Resume.ScriptableObjects;
using UnityEngine.Localization;

namespace Resume
{
    public class ResumeData
    {
        public string FullName;
        public string Role;
        public string Location;
        public string Email;
        public int MobileNumber;
        public LocalizedString Summary;
        public string Skills;
        public WorkExperience[] WorkExperiences;
        public EducationExperience[] EducationExperiences;
        public SectionData[] Sections;

        public  JobArchetype JobArchetype;
    }

    public class SectionData
    {
        public LocalizedString Title;
        public List<Experience> Entries;
    }

    public class WorkExperience : Experience
    {
        public string Position;
        public string Company;
    }

    public class EducationExperience : Experience
    {
        public string School;
        public string Degree;
    }

    public class Experience
    {
        public int YearStart;
        public int YearEnd;
        public LocalizedString Description;
    }
}