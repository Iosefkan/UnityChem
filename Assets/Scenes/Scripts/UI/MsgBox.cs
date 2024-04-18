using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MsgBox : MonoBehaviour
{
    public enum MsgType
    {
        Ok,
        YesNo
    }

    public enum Answer
    {
        NoAnswer,
        No,
        Yes,
    }

    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text msg;
    [SerializeField] private Button btnL;
    [SerializeField] private Button btnR;

    private Answer answer = Answer.NoAnswer;

    private void Start()
    {
        btnL.onClick.AddListener(() => answer = Answer.No);
        btnR.onClick.AddListener(() => answer = Answer.Yes);
        transform.localScale = Vector3.zero;
    }

    public Answer Show(string header, string msg, MsgType type)
    {
        this.header.text = header;
        this.msg.text = msg;
        if (type == MsgType.Ok)
        {
            btnL.enabled = false;
            btnR.GetComponentInChildren<TMP_Text>().text = "Хорошо";

        }
        else if (type == MsgType.YesNo)
        {
            btnL.enabled = true;
            btnL.GetComponentInChildren<TMP_Text>().text = "Нет";
            btnR.GetComponentInChildren<TMP_Text>().text = "Да";

        }

        transform.localScale = Vector3.one;

        while (answer == Answer.NoAnswer);

        transform.localScale = Vector3.zero;

        return answer;
    }
}
