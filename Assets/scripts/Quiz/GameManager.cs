using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Question[] _questions = null;
    public Question[] Questions { get { return _questions; } }

    [SerializeField] public GameEvents events = null;

    [SerializeField] private Animator timerAnimator = null;
    [SerializeField] private TextMeshProUGUI timerText = null;
    [SerializeField] private Color timerHalfWayOutColor = Color.yellow;
    [SerializeField] private Color timerAlmostOutColor = Color.red;
    private Color timerDefaultColor = Color.white;

    private List<AnswersData> PickedAnswers = new List<AnswersData>();
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;
    private int mistakes;
    //public GameObject resScreen;

    private int timerStateParameterHash = 0;

    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer;

    private string sceneName;

    public bool IsFinished
    {
        get
        {
            return (FinishedQuestions.Count < Questions.Length) ? false : true;
        }
    }

    private void OnEnable()
    {
        events.UpdateQuestionAnswer += UpadateAnswers;
    }

    private void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpadateAnswers;
    }

    private void Awake()
    {
        events.CurrentMistakes = 0;
    }

    private void Start()
    {
        Scene curScene = SceneManager.GetActiveScene();
        sceneName = curScene.name;
        //resScreen = GameObject.FindGameObjectWithTag("res");
        //resScreen.SetActive(true);
        timerDefaultColor = timerText.color;
        LoadQuestion();

        timerStateParameterHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    public void UpadateAnswers(AnswersData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswersData>();
    }

    private void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else { Debug.LogWarning("Something went wrong"); }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);
        UpdateMistakes((isCorrect) ? +Questions[currentQuestion].AddMistakes1 : +Questions[currentQuestion].AddMistakes);

        var type = (IsFinished) ? UIManager.ResolutionScreenType.Finish : (isCorrect) ? UIManager.ResolutionScreenType.Correct : UIManager.ResolutionScreenType.Incorrect;
        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, Questions[currentQuestion].AddMistakes);
        }


        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTilNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    private void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);
                timerAnimator.SetInteger(timerStateParameterHash, 2);
                break;

            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimator.SetInteger(timerStateParameterHash, 1);
                break;
        }
    }

    private IEnumerator StartTimer()
    {
        var totalTime = Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;


            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }

    private IEnumerator WaitTilNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    private Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;
        return Questions[currentQuestion];
    }

    private int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestions.Count < Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    private bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    private bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = Questions[currentQuestion].CorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    private void LoadQuestion()
    {
        Object[] obj = Resources.LoadAll("Questions/NewFolder", typeof(Question));
        _questions = new Question[obj.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            _questions[i] = (Question)obj[i];
        }

        //switch (sceneName)
        //{
        //    case "mathlevel1":
        //        Object[] objs = Resources.LoadAll("Questions/Math/level1", typeof(Question));
        //        _questions = new Question[objs.Length];
        //        for (int i = 0; i < objs.Length; i++)
        //        {
        //            _questions[i] = (Question)objs[i];
        //        }
        //        break;

        //    case "mathlevel2":
        //        Object[] objs2 = Resources.LoadAll("Questions/Math/level2", typeof(Question));
        //        _questions = new Question[objs2.Length];
        //        for (int i = 0; i < objs2.Length; i++)
        //        {
        //            _questions[i] = (Question)objs2[i];
        //        }
        //        break;

        //    case "mathlevel3":
        //        Object[] objs3 = Resources.LoadAll("Questions/Math/level3", typeof(Question));
        //        _questions = new Question[objs3.Length];
        //        for (int i = 0; i < objs3.Length; i++)
        //        {
        //            _questions[i] = (Question)objs3[i];
        //        }
        //        break;

        //    case "mathlevel4":
        //        Object[] objs4 = Resources.LoadAll("Questions/Math/level4", typeof(Question));
        //        _questions = new Question[objs4.Length];
        //        for (int i = 0; i < objs4.Length; i++)
        //        {
        //            _questions[i] = (Question)objs4[i];
        //        }
        //        break;

        //    case "mathlevel5":
        //        Object[] objs5 = Resources.LoadAll("Questions/Math/level5", typeof(Question));
        //        _questions = new Question[objs5.Length];
        //        for (int i = 0; i < objs5.Length; i++)
        //        {
        //            _questions[i] = (Question)objs5[i];
        //        }
        //        break;

        //    case "histlevel1":
        //        Object[] h_objs1 = Resources.LoadAll("Questions/History/History_level1", typeof(Question));
        //        _questions = new Question[h_objs1.Length];
        //        for (int i = 0; i < h_objs1.Length; i++)
        //        {
        //            _questions[i] = (Question)h_objs1[i];
        //        }
        //        break;

        //    case "histlevel2":
        //        Object[] h_objs2 = Resources.LoadAll("Questions/History/History_level2", typeof(Question));
        //        _questions = new Question[h_objs2.Length];
        //        for (int i = 0; i < h_objs2.Length; i++)
        //        {
        //            _questions[i] = (Question)h_objs2[i];
        //        }
        //        break;

        //    case "histlevel3":
        //        Object[] h_objs3 = Resources.LoadAll("Questions/History/History_level3", typeof(Question));
        //        _questions = new Question[h_objs3.Length];
        //        for (int i = 0; i < h_objs3.Length; i++)
        //        {
        //            _questions[i] = (Question)h_objs3[i];
        //        }
        //        break;

        //    case "histlevel4":
        //        Object[] h_objs4 = Resources.LoadAll("Questions/History/History_level4", typeof(Question));
        //        _questions = new Question[h_objs4.Length];
        //        for (int i = 0; i < h_objs4.Length; i++)
        //        {
        //            _questions[i] = (Question)h_objs4[i];
        //        }
        //        break;

        //    case "histlevel5":
        //        Object[] h_objs5 = Resources.LoadAll("Questions/History/History_level5", typeof(Question));
        //        _questions = new Question[h_objs5.Length];
        //        for (int i = 0; i < h_objs5.Length; i++)
        //        {
        //            _questions[i] = (Question)h_objs5[i];
        //        }
        //        break;

        //    case "phylevel1":
        //        Object[] p_objs1 = Resources.LoadAll("Questions/Physics/Physics_level1", typeof(Question));
        //        _questions = new Question[p_objs1.Length];
        //        for (int i = 0; i < p_objs1.Length; i++)
        //        {
        //            _questions[i] = (Question)p_objs1[i];
        //        }
        //        break;

        //    case "phylevel2":
        //        Object[] p_objs2 = Resources.LoadAll("Questions/Physics/Physics_level2", typeof(Question));
        //        _questions = new Question[p_objs2.Length];
        //        for (int i = 0; i < p_objs2.Length; i++)
        //        {
        //            _questions[i] = (Question)p_objs2[i];
        //        }
        //        break;

        //    case "phylevel3":
        //        Object[] p_objs3 = Resources.LoadAll("Questions/Physics/Physics_level3", typeof(Question));
        //        _questions = new Question[p_objs3.Length];
        //        for (int i = 0; i < p_objs3.Length; i++)
        //        {
        //            _questions[i] = (Question)p_objs3[i];
        //        }
        //        break;

        //    case "phylevel4":
        //        Object[] p_objs4 = Resources.LoadAll("Questions/Physics/Physics_level4", typeof(Question));
        //        _questions = new Question[p_objs4.Length];
        //        for (int i = 0; i < p_objs4.Length; i++)
        //        {
        //            _questions[i] = (Question)p_objs4[i];
        //        }
        //        break;

        //    case "phylevel5":
        //        Object[] p_objs5 = Resources.LoadAll("Questions/Physics/Physics_level5", typeof(Question));
        //        _questions = new Question[p_objs5.Length];
        //        for (int i = 0; i < p_objs5.Length; i++)
        //        {
        //            _questions[i] = (Question)p_objs5[i];
        //        }
        //        break;

        //    case "chemlevel1":
        //        Object[] c_objs1 = Resources.LoadAll("Questions/Chemistry/Chemistry_level1", typeof(Question));
        //        _questions = new Question[c_objs1.Length];
        //        for (int i = 0; i < c_objs1.Length; i++)
        //        {
        //            _questions[i] = (Question)c_objs1[i];
        //        }
        //        break;

        //    case "chemlevel2":
        //        Object[] c_objs2 = Resources.LoadAll("Questions/Chemistry/Chemistry_level2", typeof(Question));
        //        _questions = new Question[c_objs2.Length];
        //        for (int i = 0; i < c_objs2.Length; i++)
        //        {
        //            _questions[i] = (Question)c_objs2[i];
        //        }
        //        break;

        //    case "chemlevel3":
        //        Object[] c_objs3 = Resources.LoadAll("Questions/Chemistry/Chemistry_level3", typeof(Question));
        //        _questions = new Question[c_objs3.Length];
        //        for (int i = 0; i < c_objs3.Length; i++)
        //        {
        //            _questions[i] = (Question)c_objs3[i];
        //        }
        //        break;

        //    case "chemlevel4":
        //        Object[] c_objs4 = Resources.LoadAll("Questions/Chemistry/Chemistry_level4", typeof(Question));
        //        _questions = new Question[c_objs4.Length];
        //        for (int i = 0; i < c_objs4.Length; i++)
        //        {
        //            _questions[i] = (Question)c_objs4[i];
        //        }
        //        break;

        //    case "chemlevel5":
        //        Object[] c_objs5 = Resources.LoadAll("Questions/Chemistry/Chemistry_level5", typeof(Question));
        //        _questions = new Question[c_objs5.Length];
        //        for (int i = 0; i < c_objs5.Length; i++)
        //        {
        //            _questions[i] = (Question)c_objs5[i];
        //        }
        //        break;

        //    case "geolevel1":
        //        Object[] g_objs1 = Resources.LoadAll("Questions/Geometry/Geometry_level1", typeof(Question));
        //        _questions = new Question[g_objs1.Length];
        //        for (int i = 0; i < g_objs1.Length; i++)
        //        {
        //            _questions[i] = (Question)g_objs1[i];
        //        }
        //        break;

        //    case "geolevel2":
        //        Object[] g_objs2 = Resources.LoadAll("Questions/Geometry/Geometry_level2", typeof(Question));
        //        _questions = new Question[g_objs2.Length];
        //        for (int i = 0; i < g_objs2.Length; i++)
        //        {
        //            _questions[i] = (Question)g_objs2[i];
        //        }
        //        break;

        //    case "geolevel3":
        //        Object[] g_objs3 = Resources.LoadAll("Questions/Geometry/Geometry_level3", typeof(Question));
        //        _questions = new Question[g_objs3.Length];
        //        for (int i = 0; i < g_objs3.Length; i++)
        //        {
        //            _questions[i] = (Question)g_objs3[i];
        //        }
        //        break;

        //    case "geolevel4":
        //        Object[] g_objs4 = Resources.LoadAll("Questions/Geometry/Geometry_level4", typeof(Question));
        //        _questions = new Question[g_objs4.Length];
        //        for (int i = 0; i < g_objs4.Length; i++)
        //        {
        //            _questions[i] = (Question)g_objs4[i];
        //        }
        //        break;

        //    case "geolevel5":
        //        Object[] g_objs5 = Resources.LoadAll("Questions/Geometry/Geometry_level5", typeof(Question));
        //        _questions = new Question[g_objs5.Length];
        //        for (int i = 0; i < g_objs5.Length; i++)
        //        {
        //            _questions[i] = (Question)g_objs5[i];
        //        }
        //        break;
        //}
    }

    public void Restartgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateMistakes(int add)
    {
        events.CurrentMistakes += add;
        if (events.MistakesUpdated != null)
        {
            events.MistakesUpdated();
        }
    }
}