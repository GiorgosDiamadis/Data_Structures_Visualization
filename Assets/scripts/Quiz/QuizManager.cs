using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private GameObject answer_prefab;
    
    private List<Question> questions;
    private Dictionary<int,int> user_answers;


    private List<int> displayed_questions;
    private int max_index;
    public int selected_btn = -1;
    private int current_question = -1;

    void Start()
    {
        questions = new List<Question>();
        displayed_questions = new List<int>();
        user_answers = new Dictionary<int, int>();

        Load_Questions();
        Select_Next_Question();
    }

    public void Select_Next_Question()
    {
        if (current_question != -1)
        {
            user_answers[current_question] = selected_btn;
        }

        if (displayed_questions.Count < questions.Count)
        {
            int next = Random.Range(0, max_index);

            while (displayed_questions.Contains(next))
            {
                next = Random.Range(0, max_index);
            }
            current_question = next;
            Display_Question(current_question);
        }
        else
        {
            Show_Results();
        }
    }


    private void Display_Question(int index)
    {
        Erase_Previous();

        displayed_questions.Add(index);
        transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = questions[index].Info;
        Question current_question = questions[index];

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

    private void Load_Questions()
    {
        Object[] q = Resources.LoadAll("Questions", typeof(Question));

        foreach(Object obj in q)
        {
            questions.Add((Question)obj);
        }
        max_index = questions.Count;
    }

    private void Show_Results()
    {
        int valid = 0;
        

        for(int i = 0; i < questions.Count; i++)
        {
            int correct = questions[i].CorrectAnswers();

            if (user_answers[i] == correct)
            {
                valid++;
            }
        }

        float result = (valid*(1.0f)/ questions.Count*(1.0f)) * 100.0f;

        print("Result = " + result.ToString("0.##") + "%");
    }
}
