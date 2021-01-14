using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents" , menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswersData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type , int mistakes);
    public DisplayResolutionScreenCallback DisplayResolutionScreen;

    public delegate void MistakesUpdateCallback();
    public MistakesUpdateCallback MistakesUpdated;

    [HideInInspector]
    public int CurrentMistakes;

    [HideInInspector]
    public int StartupMistakes;
}
