using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[Serializable()]
public struct UIManagerParameters
{
    [Header("Answers Options")]
    [SerializeField] float maergins;
    public float Margins { get { return maergins; } }

    [Header("Resolution Screen options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get{ return correctBGColor; } }
    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }


}

[Serializable()]
public struct UIElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }
    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }
    [SerializeField] TextMeshProUGUI mistakesText;
    public TextMeshProUGUI MistakesText { get { return mistakesText; } }
    [Space]
    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }
    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }
    [SerializeField] TextMeshProUGUI resolutionMistakesText;
    public TextMeshProUGUI ResolutionMistakesText { get { return resolutionMistakesText; } }
    [Space]
    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }
}

public class UIManager : MonoBehaviour
{
   public enum ResolutionScreenType { Correct , Incorrect , Finish}

    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] AnswersData answerPrefab;

    [SerializeField] UIElements uiElements;

    [Space]
    [SerializeField] UIManagerParameters parameters;

    List<AnswersData> currentAnswers = new List<AnswersData>();

    public GameObject resScreen;
    private IEnumerator IE_DisplayTimeResolution;

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.MistakesUpdated += UpdateMistakesUI;
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.MistakesUpdated -= UpdateMistakesUI;
    }

    void Start()
    {
        UpdateMistakesUI();
    }

    void UpdateQuestionUI(Question question)
    {
        uiElements.QuestionInfoTextObject.text = question.Info;
        CreateAnswers(question);
    }

    void DisplayResolution(ResolutionScreenType type, int mistakes)
    {
        UpdateResUI(type,mistakes);
        resScreen.SetActive(true);

        if(type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimeResolution != null)
            {
                StopCoroutine(IE_DisplayTimeResolution);
            }
            IE_DisplayTimeResolution = DispalyTimeReslution();
            StartCoroutine(IE_DisplayTimeResolution);
        }

    }

    IEnumerator DispalyTimeReslution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        resScreen.SetActive(false);
    }

    void UpdateResUI(ResolutionScreenType type, int mistakes)
    {
        var Mistakes = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uiElements.ResolutionBG.color = parameters.CorrectBGColor;
                uiElements.ResolutionStateInfoText.text = "CORRECT";
                break;
            case ResolutionScreenType.Incorrect:
                uiElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uiElements.ResolutionStateInfoText.text = "WRONG";
                uiElements.ResolutionMistakesText.text = "+" + mistakes;
                break;
            case ResolutionScreenType.Finish:
                uiElements.ResolutionBG.color = parameters.FinalBGColor;
                uiElements.ResolutionStateInfoText.text = "FINISH!";
                StartCoroutine(CalculateMistakes());
                uiElements.FinishUIElements.gameObject.SetActive(true);
                uiElements.MistakesText.gameObject.SetActive(true);
                uiElements.MistakesText.text = ((Mistakes > events.StartupMistakes) ? "<color=yellow>new </color>" : string.Empty) + "Mistakes" + Mistakes;
                break;
        }
    }

    IEnumerator CalculateMistakes()
    {
        var mistakesValue = 0;
        while (mistakesValue < events.CurrentMistakes)
        {
            mistakesValue++;
            uiElements.ResolutionMistakesText.text = mistakesValue.ToString();
            yield return null;
        }
    }

    void CreateAnswers(Question question)
    {
        EraseAnswers();
        float offset = 0 - parameters.Margins;

        for(int i=0; i<question.Answers.Length; i++)
        {
            AnswersData newAnswer = (AnswersData)Instantiate(answerPrefab,uiElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Info,i);

            newAnswer.Rect.anchoredPosition = new Vector2(0,offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uiElements.AnswersContentArea.sizeDelta = new Vector2(uiElements.AnswersContentArea.sizeDelta.x, offset* -1);
            currentAnswers.Add(newAnswer);
        }
    }

    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }

    void UpdateMistakesUI()
    {
        uiElements.MistakesText.text = "Mistakes" + events.CurrentMistakes;
    }
}
