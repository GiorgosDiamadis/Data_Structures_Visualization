using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    private List<Question> questions;
    private List<int> displayed_questions;
    [SerializeField] private GameObject answer_prefab;
    private int max_index;


    void Start()
    {
        questions = new List<Question>();
        displayed_questions = new List<int>();
        Load_Questions();
        Select_Next_Question();
    }

    public void Select_Next_Question()
    {
        
        if(displayed_questions.Count < questions.Count)
        {
            int next = Random.Range(0, max_index);

            while (displayed_questions.Contains(next))
            {
                next = Random.Range(0, max_index);
            }

            Display_Question(next);
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

    }
}
