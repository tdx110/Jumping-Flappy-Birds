using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginPC : MonoBehaviour
{
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject buttonScore;
    [SerializeField] GameObject buttonLogin;
    [SerializeField] GameObject buttonLoginInLogin;
    [SerializeField] GameObject buttonRegister;
    [SerializeField] GameObject buttonLeaderboard;
    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField emailRegisterField;
    [SerializeField] GameObject loginInformation;
    [SerializeField] GameObject registerInformation;

    public void ShowHideStartCavas(bool show)
    {
        startCanvas.SetActive(show);
        if (PlayerPrefs.GetString(ConstantData.Email, "") == "")
        {
            buttonScore.SetActive(false);
        }
        else
        {
            buttonScore.SetActive(true);
            buttonLogin.SetActive(false);
        }
    }
    public void ShowHideLoginPanel(bool show)
    {
        loginPanel.SetActive(show);
        loginInformation.SetActive(false);
        //Sprawdza czy gracz zosta³ ju¿ zalogowany czy nie
        if (PlayerPrefs.GetString(ConstantData.Email, "") == "")
        {
            buttonLoginInLogin.SetActive(true);
            loginInformation.GetComponent<TextMeshProUGUI>().text = "Wrong e-mail or password";
        }
        else
        {
            buttonLoginInLogin.SetActive(false);
            buttonRegister.SetActive(false);
        }
    }
    public void ShowHideRegisterPanel(bool show)
    {
        registerPanel.SetActive(show);
        registerInformation.SetActive(false);
    }
    public void ShowHideLoginButton(bool show) { loginPanel.SetActive(show); }
    public void ShowHideLeaderboard(bool show) { buttonLeaderboard.SetActive(show); }


    public async void LoginToAccount()
    {
        string email = emailField.text;
        string pass = passwordField.text;

        string[] nameString = { "email", "password" };
        string[] valueString = { email, pass };
        string webResult = await WebScript.instance.GetWeb(ConstantData.ServerName + "login.php", nameString, valueString);
        if (webResult == "false")
        {
            loginInformation.SetActive(true);

        }
        else if (webResult == "Correct")
        {
            loginInformation.SetActive(false);
            PlayerPrefs.SetString(ConstantData.Email, email);
            PlayerPrefs.SetString(ConstantData.UserName, "Random");
            PlayerPrefs.SetInt(ConstantData.SaveBestScoreSave, 0);
            buttonLoginInLogin.SetActive(false);
            buttonRegister.SetActive(false);
            loginInformation.GetComponent<TextMeshProUGUI>().text = "Login Success";
            loginInformation.SetActive(true);
            //Wysy³a informacje aby jeszcze raz sprawdzi³ czy u¿ytkownik jest zalogowany
            StartScript.CheckLogin = true;
        }
    }
    public async void Register()
    {
        string email = emailRegisterField.text;
        registerInformation.GetComponent<TextMeshProUGUI>().text = "Sending messeage...";
        registerInformation.SetActive(true);

        string webString = await WebScript.instance.GetWeb(ConstantData.ServerName + "activationLink.php", "email", email);
        if (webString == "Correct")
        {
            registerInformation.GetComponent<TextMeshProUGUI>().text =
                "Registration link has been sent to your email address.";
            emailRegisterField.text = "";
        }
        else
        {
            registerInformation.GetComponent<TextMeshProUGUI>().text =
                "Something go wrong. Please try again, or later.";
        }
    }
}
