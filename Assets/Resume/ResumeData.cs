// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Resume.Data;
using UnityEngine.Localization;

namespace Resume
{
    
    public class ResumeData
    {
        public string FullName = "";
        public string Role = "";
        public string Location = "";
        public string Email = "";
        public int MobileNumber = 0;
        public LocalizedString Summary = new();
        public string Skills = "";
        public string SoftSkills = "";

        public WorkExperience[] WorkExperiences = new WorkExperience[0];
        public FunnyExperience FunnyExperience = null;
        public EducationExperience[] EducationExperiences = new EducationExperience[0];
        public SectionData[] Sections = new SectionData[0];

        public JobArchetype JobArchetype = null;
        public List<ResumeSectionType> SectionTypes = new();
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

    public class FunnyExperience : Experience
    {
        public LocalizedString KeyElement;
    }
}