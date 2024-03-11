using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class EntryValuePanel : MonoBehaviour
{
    public delegate void ChangeVal(TMP_Text message);
    public event ChangeVal OnChangeVal;

    Regex inReg = new Regex("^-?(\\d){0,3}(,(\\d){0,2})?$");

    [SerializeField] VrButton[] valBtns = new VrButton[10];
    [SerializeField] VrButton minusBtn;
    [SerializeField] VrButton commaBtn;
    [SerializeField] VrButton delBtn;
    [SerializeField] VrButton entrBtn;

    [SerializeField] TMP_Text valText;

    TMP_Text targetField;

    void Start()
    {
        for (int i = 0; i < valBtns.Length; ++i)
        {
            int elem = i;
            valBtns[i].down.AddListener(() =>
            {
                string resStr = valText.text + elem.ToString();
                if (inReg.IsMatch(resStr))
                {
                    valText.text = resStr;
                }                
            });
        }

        minusBtn.down.AddListener(() =>
        {
            if (valText.text.Length == 0)
            {
                valText.text = "-";
            }
        });

        commaBtn.down.AddListener(() =>
        {
            string resStr = valText.text + ",";
            if (inReg.IsMatch(resStr))
            {
                valText.text = resStr;
            }
        });

        delBtn.down.AddListener(() =>
        {
            if (valText.text.Length == 0) return;
            valText.text = valText.text.Substring(0, valText.text.Length - 1);
        });

        entrBtn.down.AddListener(() =>
        {
            if (valText.text.Length == 0 ||
                (valText.text.Length == 1 && valText.text == "-"))
            {
                targetField.text = "0";
            }
            else
            {
                targetField.text = string.Format("{0:f}", double.Parse(valText.text));
            }

            OnChangeVal?.Invoke(targetField);
            gameObject.SetActive(false);
        });

        this.gameObject.SetActive(false);
    }

    public void Open(TMP_Text t)
    {
        targetField = t;
        valText.text = t.text;
        gameObject.SetActive(true);
    }
}
