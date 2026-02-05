using System.Collections.Generic;

namespace Quizlet.Model
{
    public class DemoQuestionService
    {
        public List<Question> GetQuestions()
        {
            return new List<Question>
            {
                new Question
                {
                    Text = "Was ist 2 + 2?",
                    Options = new List<string>{ "3", "4", "5", "6" },
                    CorrectIndex = 1
                },
                new Question
                {
                    Text = "Welche Farbe hat der Himmel?",
                    Options = new List<string>{ "Blau", "Grün", "Rot", "Gelb" },
                    CorrectIndex = 0
                }
            };
        }
    }
}
