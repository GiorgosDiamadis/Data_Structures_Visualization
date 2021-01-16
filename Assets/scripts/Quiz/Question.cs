using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Answer
{
    [SerializeField] private string _info;
    public string Info { get { return _info; } }

    [SerializeField] private bool _isCorrect;
    public bool IsCorrect { get { return _isCorrect; } }
}

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/new Question")]
public class Question : ScriptableObject
{
    public enum AnswerType { Multi, Single }

    [SerializeField] private string _info = string.Empty;
    public string Info { get { return _info; } }

    [SerializeField] private Answer[] _answers = null;
    public Answer[] Answers
    {
        get
        {
            return _answers;
        }
    }

    //parameters
    [SerializeField] private bool _useTimer = false;

    public bool UseTimer { get { return _useTimer; } }

    [SerializeField] private int _timer = 0;
    public int Timer { get { return _timer; } }

    [SerializeField] private AnswerType _answerType = AnswerType.Multi;
    public AnswerType GetAnswerType { get { return _answerType; } }

    [SerializeField] private int _addMistakes = 1;
    public int AddMistakes { get { return _addMistakes; } }

    [SerializeField] private int _addMistakes1 = 0;
    public int AddMistakes1 { get { return _addMistakes1; } }

    [SerializeField] private int _mistakes = 0;
    public int Mistakes { get { return _mistakes; } }

    public int CorrectAnswers()
    {
        int CorrectAnswers = -1;
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswers=i;
            }
        }

        return CorrectAnswers;
    }
}