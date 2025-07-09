using System.Collections.Generic;
using System.Linq;
using Resume.Base;
using Resume.ScriptableObjects;
using UnityEngine;
using UnityEditor.Localization;

namespace Resume
{
    public class ResumeGenerator : MonoBehaviour
    {
        [SerializeField] private NamesData _namesData;
        [SerializeField] private JobsData _jobsData;
        [SerializeField] private LocationsData _locationsData;

        [SerializeField] private Transform _resumeParent;
        [SerializeField] private Transform _textStartTransform;
        [SerializeField] private TextHandler _textHandlerPrefab;

        [SerializeField] private float _shortSpacing = 0.3f;
        [SerializeField] private float _longSpacingRelation = 1.5f;

        [SerializeField] private StringTableCollection _resumeTableCollection;

        private float LongSpace => _shortSpacing * _longSpacingRelation;

        public void GenerateResume()
        {
            var randomResume = Generate();
            GenerateResume(randomResume);
        }

        private ResumeData Generate()
        {
            var resume = new ResumeData();

            resume.FullName = RandomName();
            resume.Role = RandomElement(_jobsData.JobTitles);
            resume.LocationContact = RandomCity();
            resume.Skills = GetRandomSubset(_jobsData.Skills, 4);
            resume.Summary = "Summary";
            resume.WorkExperiences = GenerateRandomWorkExperiences();
            resume.Sections = GenerateRandomSections();

            return resume;
        }

        string RandomName() =>
            $"{RandomElement(_namesData.FirstNames)} {RandomElement(_namesData.LastNames)}";

        string RandomCity()
        {
            var country = RandomElement(_locationsData.Locations.ToArray());
            var state = RandomElement(country.States);
            return RandomElement(state.Cities) + ", " + state.State + ", " + country.Country;
        }

        T RandomElement<T>(T[] array)
        {
            if (array == null || array.Length == 0)
                throw new System.ArgumentException("Array cannot be null or empty");
            return array[Random.Range(0, array.Length)];
        }

        List<string> GetRandomSubset(string[] source, int count)
        {
            return source.OrderBy(x => Random.value).Take(count).ToList();
        }

        List<WorkExperience> GenerateRandomWorkExperiences()
        {
            var experiences = new List<WorkExperience>();
            int count = Random.Range(2, 5);

            for (int i = 0; i < count; i++)
            {
                experiences.Add(new WorkExperience
                {
                    Position = RandomElement(_jobsData.JobTitles),
                    Company = "Company " + (i + 1),
                    YearRange = $"{2020 + i}-{2021 + i}",
                    Description = "Work description " + (i + 1)
                });
            }

            return experiences;
        }

        List<SectionData> GenerateRandomSections()
        {
            var sections = new List<SectionData>();
            int count = Random.Range(1, 4);

            for (int i = 0; i < count; i++)
            {
                var entries = new List<Experience>();
                int entriesCount = Random.Range(1, 4);

                for (int j = 0; j < entriesCount; j++)
                {
                    entries.Add(new Experience
                    {
                        YearRange = $"{2020 + j}-{2021 + j}",
                        Description = "Experience description " + (j + 1)
                    });
                }

                sections.Add(new SectionData
                {
                    Title = "Section " + (i + 1),
                    Entries = entries
                });
            }

            return sections;
        }

        void GenerateResume(ResumeData resume)
        {
            foreach (Transform child in _resumeParent)
            {
                Destroy(child.gameObject);
            }

            Vector2 location = new Vector2();
            TextHandler nameText = AddText(resume.FullName, location);
            nameText.SetFontSize(30);
            location.y += LongSpace;

            TextHandler roleText = AddText(resume.Role, location);
            nameText.SetFontSize(20);
            location.y += LongSpace;
            TextHandler locationContactText = AddNormalText(resume.LocationContact, ref location);
            TextHandler summaryTitleText = AddSectionText("Summary", ref location);
            TextHandler summaryText = AddNormalText(resume.Summary, ref location);

            AddSectionText("Skills", ref location);
            resume.Skills.ForEach(skill => AddText(skill, location));
            location.y += LongSpace;

            AddSectionText("Experience", ref location);
            resume.WorkExperiences.ForEach(job => AddJob(job, ref location));
            location.y += LongSpace;

            AddSectionText("Projects", ref location);
            location.y += LongSpace;

            foreach (var section in resume.Sections)
            {
                AddSectionText(section.Title, ref location);
                foreach (var experience in section.Entries)
                {
                    AddNormalText($"{experience.YearRange}", ref location);
                    AddNormalText(experience.Description, ref location);
                }
            }
        }

        TextHandler AddText(string textKey, Vector2 location)
        {
            TextHandler textInstance = Instantiate(_textHandlerPrefab, _resumeParent);
            textInstance.transform.localPosition = new Vector3(location.x, 0, -location.y);
            textInstance.SetTextKey(textKey, _resumeTableCollection);

            return textInstance;
        }

        private void AddJob(WorkExperience workExperience, ref Vector2 description)
        {
            AddNormalText($"{workExperience.Position} at {workExperience.Company} ({workExperience.YearRange})",
                ref description);
            AddNormalText(workExperience.Description, ref description);
        }

        TextHandler AddSectionText(string textKey, ref Vector2 location)
        {
            TextHandler sectionText = AddText(textKey, location);
            sectionText.SetFontSize(15);
            //tmp.fontSize = size;

            /*bool bold = false,  bool italic = false,  bool underline = false,
            int size = 12
            if (bold) tmp.fontStyle = FontStyles.Bold;
            else if (italic) tmp.fontStyle = FontStyles.Italic;
            if (underline) tmp.fontStyle |= FontStyles.Underline;*/

            location.y += LongSpace;
            return sectionText;
        }

        TextHandler AddNormalText(string textKey, ref Vector2 location)
        {
            TextHandler text = AddText(textKey, location);
            text.SetFontSize(15);
            //tmp.fontSize = size;

            /*bool bold = false,  bool italic = false,  bool underline = false,
            int size = 12
            if (bold) tmp.fontStyle = FontStyles.Bold;
            else if (italic) tmp.fontStyle = FontStyles.Italic;
            if (underline) tmp.fontStyle |= FontStyles.Underline;*/

            location.y += _shortSpacing;
            return text;
        }
    }
}