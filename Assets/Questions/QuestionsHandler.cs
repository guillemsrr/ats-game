using System;
using UnityEngine;

namespace Questions
{
    public class QuestionsHandler : MonoBehaviour
    {
        [SerializeField] private QuestionBase _questionModel;
        [SerializeField] private float _questionSeparation = 5;

        private float _currentHeight;

        private void Start()
        {
            /*QuestionData questionDataTest = new QuestionData();
            CreateQuestion(questionDataTest);
            CreateQuestion(questionDataTest);*/
        }

        public void Initialize()
        {
            _currentHeight = 0f;
        }
        
        public void CreateQuestion(QuestionData questionData)
        {
            QuestionBase question = Instantiate(_questionModel, transform);
            question.transform.localPosition = new Vector3(0, 0, _currentHeight);
            question.SetQuestion(questionData);

            _currentHeight -= _questionSeparation;
        }
    }
}