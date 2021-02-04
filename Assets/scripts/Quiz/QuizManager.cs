using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private GameObject answer_prefab;
    [SerializeField] private GameObject scoring_panel;
    [SerializeField] private GameObject category_prefab;
    [SerializeField] private TMPro.TextMeshProUGUI final_score;
    [SerializeField] private TMPro.TextMeshProUGUI question_index;

    private List<Question> questions;
    private Dictionary<int,int> user_answers;
    private bool results_shown = false;

    private List<int> displayed_questions;
    public int selected_btn = -1;
    private int current_question = -1;
    private int next = 0;

    void Start()
    {
        questions = new List<Question>();
        displayed_questions = new List<int>();
        user_answers = new Dictionary<int, int>();

        Load_Questions("Lists");
    }

    public void Select_Next_Question()
    {
        if (current_question != -1)
        {
            user_answers[current_question] = selected_btn;
        }

        if (displayed_questions.Count < questions.Count)
        {
            current_question = next++;
            Display_Question(current_question);
        }
        else
        {
            print("show");
            Show_Results();
        }
    }


    private void Display_Question(int index)
    {
        Erase_Previous();

        displayed_questions.Add(index);
        transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = questions[index].Info;
        Question current_question = questions[index];

        question_index.text = "Question " + (index+1) + "/" + questions.Count;


        foreach(Answer answer in current_question.Answers)
        {
            GameObject ans = Instantiate(answer_prefab, transform.Get_Child_Transform(1));
            ans.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0).text = answer.Info;
        }
    }

    private void Erase_Previous()
    {
        transform.Get_Child_Transform(1).Destroy_All_Children();
    }

    public void Reset_Quiz()
    {
        scoring_panel.transform.Destroy_All_Children();
        final_score.text = "";
        user_answers.Clear();
        displayed_questions.Clear();
        selected_btn = -1;
        current_question = -1;
        results_shown = false;
        next = 0;
        Erase_Previous();
        Select_Next_Question();
    }

    public void Load_Questions(string structure)
    {
        questions.Clear();
        
        Object[] q = Resources.LoadAll("Questions/" + structure, typeof(Question));

        foreach(Object obj in q)
        {
            questions.Add((Question)obj);
        }
        Reset_Quiz();
    }

    private void Show_Results()
    {
        if (results_shown == true)
            return;

        int valid = 0;
        List<int> valid_questions = new List<int>();

        for(int i = 0; i < questions.Count; i++)
        {
            int correct = questions[i].CorrectAnswers();

            if (user_answers[i] == correct)
            {
                valid++;
                valid_questions.Add(1);
            }
            else
            {
                valid_questions.Add(-1);
            }
        }

        for(int i=0; i< valid_questions.Count; i++)
        {
            GameObject g = Instantiate(category_prefab, scoring_panel.transform);
            if (valid_questions[i] == 1)
            {
                g.transform.Get_Component_In_Child<Image>(0).color = Color.green;
            }
            else
            {
                g.transform.Get_Component_In_Child<Image>(0).color = Color.red;

            }


            g.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(1).text = "Question " + (i + 1).ToString();

        }

        float result = (valid*(1.0f)/ questions.Count*(1.0f)) * 100.0f;

        final_score.text = result.ToString("0.##") + "%";
        results_shown = true;
    }
}
