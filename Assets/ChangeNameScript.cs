using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UDPKing;
using System.Threading.Tasks;

public class ChangeNameScript : MonoBehaviour
{
    [SerializeField]
    private GameObject thisGameObject;

    [Header("Pole tekstowe")]
    [SerializeField]
    private TMP_InputField userNameInputField;
    [Header("Pole z informacjami o ewentualnych b³êdach.")]
    [SerializeField]
    private TextMeshProUGUI information;

    [Header("Przycisk Change")]
    [SerializeField]
    private GameObject changeButton;
    [Header("Przycisk Back")]
    [SerializeField]
    private GameObject backButton;

    [Header("Skrypt Score")]
    [SerializeField]
    private ScoreScript scoreScript;
    [Header("Canvas Score")]
    [SerializeField]
    private GameObject ScoreCanvas;

    private void OnEnable()
    {
        userNameInputField.text = PlayerPrefs.GetString(ConstantData.UserName, "");
        information.text = "";
    }

    /// <summary>
    /// Przycisk zmiany nazwy gracza
    /// Sprawdzanie wszystkich wymagañ
    /// Jeœli s¹ spe³nione to wysy³a informacje o zmianê danych
    /// </summary>
    
    public async void ChangeNameAsync()
    {
        string newName;
        newName = userNameInputField.text;
        if (newName.Length < 3)
        {
            information.text = "Username is to short";
            return;
        }
        else if (newName.Length > 60)
        {
            information.text = "Username is to long";
            return;
        }
        else
        {
            if (newName.IndexOfAny(ConstantData.BlockChar)== -1)
            {
                if (newName == PlayerPrefs.GetString(ConstantData.UserName))
                {
                    information.text = "Username cannot be the same";
                    return;
                }
                else
                {
                    string[] arrayName = {"email","name","command" };
                    string[] arrayValue = {PlayerPrefs.GetString(ConstantData.Email),newName,"ChangeName" };
                    backButton.GetComponent<Button>().interactable = false;
                    changeButton.GetComponent<Button>().interactable = false;
                    string webResult = 
                        await WebScript.instance.GetWeb(ConstantData.ServerName + "one_command.php",arrayName,arrayValue);
                    backButton.GetComponent<Button>().interactable = true;
                    changeButton.GetComponent<Button>().interactable = true;
                    if (webResult == "Correct") information.text = "Name change success.";
                    Debug.Log(webResult);
                }
            }
            else
            {
                string stringBlockList = "";
                for (int i = 0; i < ConstantData.BlockChar.Length-1; i++)
                {
                    stringBlockList += ConstantData.BlockChar[i].ToString() + ",";
                }
                stringBlockList += ConstantData.BlockChar[ConstantData.BlockChar.Length-1].ToString();

                information.text = "In Username characters not allowed:\n" + stringBlockList;

            }
        }
    }
    //Cofa do listy punktów
    public void BackButton()
    {
        thisGameObject.SetActive(false);
        ScoreCanvas.SetActive(true);
    }

}
