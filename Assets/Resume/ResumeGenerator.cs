// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Resume.Base;
using Resume.Data;
using Resume.Data.Requirements;
using Resume.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Utils;
using Random = UnityEngine.Random;

namespace Resume
{
    public class ResumeGenerator : MonoBehaviour
    {
        [SerializeField] private NamesData _namesData;
        [SerializeField] private LevelArchetypes _levelArchetypes;
        [SerializeField] private LocationsData _locationsData;
        [SerializeField] private EmailsData _emailsData;

        [SerializeField] private Transform _resumeParent;
        [SerializeField] private Transform _textStartTransform;
        [SerializeField] private TextHandler _textHandlerPrefab;

        [SerializeField] private float _shortSpacing = 0.3f;
        [SerializeField] private float _longSpacingRelation = 1.25f;

        [SerializeField] private float _normalFontSize = 9f;
        [SerializeField] private float _shortFontRelation = 0.75f;
        [SerializeField] private float _sectionFontSize = 14f;
        [SerializeField] private float _nameFontSize = 30f;
        [SerializeField] private Vector2 _fontSizeRangeRelation = new Vector2(0.8f, 1.2f);

        [SerializeField] private TMP_FontAsset[] _fonts;
        [SerializeField] private TMP_FontAsset[] _alternativeFonts;

        private TMP_FontAsset _currentFont;

        private float _fontSizeMultiplier = 1f;

        private float LongSpace => _shortSpacing * _longSpacingRelation;

        private float ShortFontSize => _normalFontSize * _shortFontRelation;

        private TextAlignmentOptions _alignment;

        Vector2 _location = new Vector2();

        private int _currentLevel;

        public LevelArchetype GetRandomLevelArchetype(int level)
        {
            _currentLevel = level;
            LevelArchetype archetype = _levelArchetypes.GetRandomArchetype(level);
            return archetype;
        }

        public async Task<ResumeData> GenerateResume(LevelArchetype archetype)
        {
            ResumeData resume = new ResumeData();
            resume.FullName = _namesData.GetRandomName();

            JobArchetype jobArchetype = archetype.GetJobArchetype();
            resume.JobArchetype = jobArchetype;
            /*if (archetype.Level > 1) //although not everybody would add it
            {
            }*/

            resume.Role = UtilsLibrary.RandomElement(jobArchetype.JobTitles);

            resume.Location = await _locationsData.GetRandomLocation();
            resume.Email = UtilsLibrary.RandomElement(_emailsData.Emails);
            resume.MobileNumber = Random.Range(100000000, 999999999);

            resume.Skills = jobArchetype.GetRandomSkills(3);
            resume.SoftSkills = jobArchetype.GetRandomSoftSkills(3);

            List<ResumeSectionType> allSections = new()
            {
                ResumeSectionType.Summary,
                ResumeSectionType.Skills,
                ResumeSectionType.Experience,
                ResumeSectionType.Education,
                ResumeSectionType.Projects
            };

            int minSections =
                Mathf.Clamp(_currentLevel + 2, 2, allSections.Count); // e.g. level 0 = 2 sections, level 4 = 5
            int maxSections = Mathf.Clamp(_currentLevel + 3, minSections, allSections.Count); // Add some variability

            int sectionCount = Random.Range(minSections, maxSections + 1);

            List<ResumeSectionType> sections = allSections
                .OrderBy(_ => Random.value)
                .Take(sectionCount)
                .ToList();

            float sectionShuffleRandom = Random.value;
            if (sectionShuffleRandom > 0.75f)
            {
                UtilsLibrary.ShuffleList(sections);
            }

            resume.SectionTypes = sections;

            //TODO: just add data if it's added in section?
            resume.Summary = UtilsLibrary.RandomElement(jobArchetype.Summary);
            resume.WorkExperiences = GenerateRandomWorkExperiences(jobArchetype);
            resume.FunnyExperience = GenerateRandomFunnyExperience(jobArchetype);
            resume.EducationExperiences = GenerateRandomEducation(jobArchetype);
            resume.Sections = GenerateRandomSections(jobArchetype);
            return resume;
        }

        private FunnyExperience GenerateRandomFunnyExperience(JobArchetype jobArchetype)
        {
            int startYear = Random.Range(2000, 2025);
            int duration = Random.Range(1, 4);
            int endYear = startYear + duration;

            FunnyExperience experience = new FunnyExperience();
            experience.YearStart = startYear;
            experience.YearEnd = endYear;
            experience.KeyElement = UtilsLibrary.RandomElement(jobArchetype.FunnyExperiences.KeyElement);
            experience.Description = UtilsLibrary.RandomElement(jobArchetype.FunnyExperiences.Descriptions);
            return experience;
        }

        private static string GetSkillsJoined(string[] skills)
        {
            string skillsJoined = "";
            for (int i = 0; i < skills.Length - 1; i++)
            {
                skillsJoined += skills[i] + ", ";
            }

            skillsJoined += skills[^1];
            return skillsJoined;
        }

        private EducationExperience[] GenerateRandomEducation(JobArchetype jobArchetype)
        {
            int count = Random.Range(1, 3);
            var experiences = new EducationExperience[count];

            for (int i = 0; i < count; i++)
            {
                int startYear = Random.Range(1980, 2025);
                int endYear = startYear + Random.Range(2, 8);

                var educationData = UtilsLibrary.RandomElement(jobArchetype.EducationData);

                experiences[i] = new EducationExperience
                {
                    YearStart = startYear,
                    YearEnd = endYear,
                    School = UtilsLibrary.RandomElement(educationData.Schools),
                    Degree = UtilsLibrary.RandomElement(educationData.Degrees),
                    Description = UtilsLibrary.RandomElement(educationData.Description)
                };
            }

            return experiences;
        }

        public void GenerateVisualResume(ResumeData resumeData)
        {
            foreach (Transform child in _resumeParent)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(GenerateResume(resumeData));
        }

        private WorkExperience[] GenerateRandomWorkExperiences(JobArchetype jobArchetype)
        {
            int count = Random.Range(1, 4);
            var experiences = new WorkExperience[count];

            for (int i = 0; i < count; i++)
            {
                int startYear = Random.Range(2012, 2022);
                int duration = Random.Range(1, 4);
                int endYear = startYear + duration;

                var companyData = UtilsLibrary.RandomElement(jobArchetype.CompaniesData);

                WorkExperience experience = new WorkExperience();
                experience.YearStart = startYear;
                experience.YearEnd = endYear;
                experience.Position = UtilsLibrary.RandomElement(jobArchetype.JobTitles);
                experience.Company = UtilsLibrary.RandomElement(companyData.Companies);
                experience.Description = UtilsLibrary.RandomElement(companyData.Descriptions);
                experiences[i] = experience;
            }

            return experiences;
        }

        private SectionData[] GenerateRandomSections(JobArchetype jobArchetype)
        {
            var sections = new List<SectionData>();
            //TODO
            return sections.ToArray();
        }

        IEnumerator GenerateResume(ResumeData resume)
        {
            bool isDifficult = Random.value < 0.25;
            TMP_FontAsset[] fonts = isDifficult ? _alternativeFonts : _fonts;
            _currentFont = UtilsLibrary.RandomElement(fonts);

            _fontSizeMultiplier = Random.Range(_fontSizeRangeRelation.x, _fontSizeRangeRelation.y);

            _alignment = TextAlignmentOptions.Left;
            SetRandomAlignment();

            _location = new Vector2();
            TextHandler nameText = AddText(resume.FullName);
            SetFontSize(nameText, _nameFontSize);
            yield return nameText.DelayedSizeUpdate();
            _location.y += nameText.TextHeight;

            if (!string.IsNullOrEmpty(resume.Role))
            {
                TextHandler roleText = AddText(resume.Role);
                SetFontSize(roleText, _sectionFontSize * 1.1f);
                yield return roleText.DelayedSizeUpdate();
                _location.y += roleText.TextHeight;
            }

            if (!string.IsNullOrEmpty(resume.Location))
            {
                AddNormalText(resume.Location);
            }

            if (!string.IsNullOrEmpty(resume.Email))
            {
                AddNormalText(resume.Email);
            }

            if (resume.MobileNumber != 0)
            {
                string mobileNumber = resume.MobileNumber.ToString();
                AddNormalText(mobileNumber);
            }

            SetRandomAlignment();

            foreach (ResumeSectionType resumeSectionType in resume.SectionTypes)
            {
                switch (resumeSectionType)
                {
                    case ResumeSectionType.Summary:
                        yield return GenerateSummarySection(resume);
                        break;
                    case ResumeSectionType.Skills:
                        yield return GenerateSkillsSection(resume);
                        break;
                    case ResumeSectionType.Experience:
                        yield return GenerateExperienceSection(resume);
                        break;
                    case ResumeSectionType.Education:
                        yield return GenerateEducationSection(resume);
                        break;
                    case ResumeSectionType.Projects:
                        yield return GenerateProjectsSection(resume);
                        break;
                }
            }
        }

        private void SetRandomAlignment()
        {
            float alignmentRandom = Random.value;
            if (alignmentRandom > 0.75f)
            {
                _alignment = TextAlignmentOptions.Right;
            }
            else if (alignmentRandom > 0.5f)
            {
                _alignment = TextAlignmentOptions.Center;
            }
        }

        private IEnumerator GenerateEducationSection(ResumeData resume)
        {
            if (resume.EducationExperiences.Length > 0)
            {
                AddSectionText("Education");
                foreach (var education in resume.EducationExperiences)
                {
                    TextHandler text = AddText(
                        $"{education.Degree} at {education.School} ({education.YearStart} - {education.YearEnd})");
                    SetFontSize(text, _normalFontSize);

                    yield return text.DelayedSizeUpdate();
                    _location.y += text.TextHeight;

                    if (IsValid(education.Description))
                    {
                        var educationDescription = AddNormalText(education.Description);
                        yield return educationDescription.DelayedSizeUpdate();
                        _location.y += educationDescription.TextHeight;
                    }
                }
            }
        }

        private IEnumerator GenerateProjectsSection(ResumeData resume)
        {
            if (resume == null || resume.Sections == null || resume.Sections.Length == 0)
            {
                yield break;
            }

            AddSectionText("Projects");

            foreach (var section in resume.Sections)
            {
                AddSectionText(section.Title);
                foreach (var experience in section.Entries)
                {
                    AddNormalText($"{experience.YearStart} - {experience.YearEnd}");

                    if (IsValid(experience.Description))
                    {
                        var projectDescription = AddNormalText(experience.Description);
                        yield return projectDescription.DelayedSizeUpdate();
                        _location.y += projectDescription.TextHeight;
                    }
                }
            }
        }

        private IEnumerator GenerateExperienceSection(ResumeData resume)
        {
            if (resume.WorkExperiences.Length > 0)
            {
                AddSectionText("Experience");
                foreach (var workExperience in resume.WorkExperiences)
                {
                    string textString =
                        $"{workExperience.Position} at {workExperience.Company} ({workExperience.YearStart} - {workExperience.YearEnd})";
                    TextHandler text = AddText(textString);
                    SetFontSize(text, _normalFontSize);
                    yield return text.DelayedSizeUpdate();
                    _location.y += text.TextHeight;

                    if (IsValid(workExperience.Description))
                    {
                        var jobdescription = AddNormalText(workExperience.Description);
                        yield return jobdescription.DelayedSizeUpdate();
                        _location.y += jobdescription.TextHeight;
                    }
                }

                if (resume.FunnyExperience != null)
                {
                    var keyElementOp = resume.FunnyExperience.KeyElement.GetLocalizedStringAsync();
                    yield return keyElementOp;
                    string keyElement = keyElementOp.Result;

                    var descriptionOp = resume.FunnyExperience.Description.GetLocalizedStringAsync(keyElement);
                    yield return descriptionOp;
                    string funnyText = descriptionOp.Result;

                    funnyText += $" ({resume.FunnyExperience.YearStart} - {resume.FunnyExperience.YearEnd})";

                    TextHandler text = AddText(funnyText);
                    SetFontSize(text, _normalFontSize);
                    yield return text.DelayedSizeUpdate();
                    _location.y += text.TextHeight;
                }
            }
        }

        private bool IsValid(LocalizedString localizedString)
        {
            return localizedString != null && !localizedString.IsEmpty;
        }

        private IEnumerator GenerateSkillsSection(ResumeData resume)
        {
            AddSectionText("Skills");

            string skills = GetSkillsJoined(resume.Skills);
            TextHandler skillsText = AddText(skills);
            SetFontSize(skillsText, ShortFontSize);
            _location.y += skillsText.TextHeight;

            string[] localizedSkills =
                resume.SoftSkills.Select(skill => skill.GetLocalizedStringAsync().Result).ToArray();
            skills = GetSkillsJoined(localizedSkills);

            TextHandler softskillsText = AddText(skills);
            SetFontSize(softskillsText, ShortFontSize);
            _location.y += softskillsText.TextHeight;

            yield break;
        }

        private IEnumerator GenerateSummarySection(ResumeData resume)
        {
            TextHandler summaryTitleText = AddSectionText("Summary");
            TextHandler summaryText = AddText(resume.Summary, _location);
            SetFontSize(summaryText, _normalFontSize);
            yield return summaryText.DelayedSizeUpdate();
            _location.y += summaryText.TextHeight;
        }

        TextHandler AddText(string textKey)
        {
            TextHandler textInstance = CreateTextInstance(_location);
            textInstance.SetText(textKey, "Resume");

            return textInstance;
        }

        TextHandler AddText(LocalizedString localizedString, Vector2 location)
        {
            TextHandler textInstance = CreateTextInstance(location);
            textInstance.SetText(localizedString);

            return textInstance;
        }

        private TextHandler CreateTextInstance(Vector2 location)
        {
            TextHandler textInstance = Instantiate(_textHandlerPrefab, _resumeParent);
            textInstance.transform.localPosition = new Vector3(location.x, 0, -location.y);
            textInstance.SetFont(_currentFont);
            textInstance.SetAlignment(_alignment);

            return textInstance;
        }

        TextHandler AddSectionText(LocalizedString title)
        {
            _location.y += LongSpace;

            TextHandler sectionText = AddText(title, _location);
            SetFontSize(sectionText, _sectionFontSize);
            _location.y += _shortSpacing;

            return sectionText;
        }

        TextHandler AddSectionText(string title)
        {
            _location.y += LongSpace;

            TextHandler sectionText = AddText(title);
            SetFontSize(sectionText, _sectionFontSize);

            _location.y += _shortSpacing;

            return sectionText;
        }

        TextHandler AddNormalText(LocalizedString textKey)
        {
            TextHandler text = AddText(textKey, _location);
            SetFontSize(text, _normalFontSize);

            _location.y += _shortSpacing;
            return text;
        }

        TextHandler AddNormalText(string textKey)
        {
            TextHandler text = AddText(textKey);
            SetFontSize(text, _normalFontSize);
            _location.y += _shortSpacing;
            return text;
        }

        public async Task<List<RequirementPoco>> GetRandomRequirements(ResumeData resumeData, LevelArchetype archetype,
            float percentageMet)
        {
            bool atLeastOneIsNotMet = percentageMet < 1f;

            List<RequirementData> requirementData =
                archetype.GetRandomRequirements(resumeData, Mathf.Approximately(percentageMet, 1f));

            List<RequirementPoco> requirements = new List<RequirementPoco>();

            int forceUnmetIndex = -1;
            if (atLeastOneIsNotMet && requirementData.Count > 0)
            {
                forceUnmetIndex = Random.Range(0, requirementData.Count);
            }

            for (int i = 0; i < requirementData.Count; i++)
            {
                RequirementData requirement = requirementData[i];

                bool isMet;
                if (i == forceUnmetIndex)
                {
                    isMet = false;
                }
                else
                {
                    isMet = Random.value < percentageMet;
                }

                string description = await requirement.GetDescription(resumeData, isMet);
                requirements.Add(new RequirementPoco(description));
            }

            return requirements;
        }

        void SetFontSize(TextHandler text, float fontSize)
        {
            text.SetFontSize(fontSize * _fontSizeMultiplier);
        }
    }
}