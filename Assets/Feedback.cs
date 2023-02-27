using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Feedback : MonoBehaviour
{
    [Header("Pole z canvas-em Setting")]
    [SerializeField] private GameObject canvasStart;
    [Header("Pole tekstowe do napisania i wys³ania wiadomoœci")]
    [SerializeField] private TMP_InputField messeageInputTMP;
    [Header("Tekst z minimaln¹ iloœci¹ znaków")]
    [SerializeField] private GameObject minChar;
    [Header("Tekst z pozosta³¹ iloœci¹ znaków")]
    [SerializeField] private TextMeshProUGUI charLeft;
    public static Feedback instance;

    //Limit znaków
    private const int charLimit = 600;
    void Start()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<Feedback>();
            messeageInputTMP.characterLimit = charLimit;
        }
        else
        {
            Destroy(this.gameObject.GetComponent<Feedback>());
            Debug.LogWarning("Cannot use next Feedback script.");
        }
    }
    private void OnEnable()
    {
            minChar.SetActive(false);
    }
    public async void SendMesseage()
    {
        if (messeageInputTMP.text.Length >30)
        {
            string[] arrayName = { "command","email", "messeage" };
            string[] arrayValue = { "Feedback_Email", PlayerPrefs.GetString(ConstantData.Email, "Not Register"),
                messeageInputTMP.text };
            minChar.SetActive(true);
            minChar.GetComponent<TextMeshProUGUI>().text = "Sending messeage...";
            string webMesseage = await WebScript.instance.GetWeb(ConstantData.ServerName + "one_command.php",
                arrayName, arrayValue);
            minChar.GetComponent<TextMeshProUGUI>().text = "Messeage sended.";
            messeageInputTMP.text = "";
            Debug.Log(webMesseage);
        }
        else
        {
            minChar.SetActive(true);
            minChar.GetComponent<TextMeshProUGUI>().text = "Cannot send: must be minimum 30 char.";
        }

    }
    public void UpdateLimit()
    {
        charLeft.text = (charLimit - messeageInputTMP.text.Length).ToString();
    }
    public void BackToStart()
    {
        canvasStart.SetActive(true);
        gameObject.SetActive(false);
    }
}
