using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class StartScript : MonoBehaviour
{
    public static bool CheckLogin = false;
    public static bool ShowInfo = false;

    [Header("Canvas Score")]
    [SerializeField] GameObject canvasScore;
    [Header("Canvas Feedback")]
    [SerializeField] GameObject canvasFeedback;
    [Header("ButtonLogin")]
    [SerializeField] GameObject loginButton;
    [Header("ButtonLogin dla PC")]
    [SerializeField] GameObject loginButtonPC;
    [Header("LoginPCCanvas")]
    [SerializeField] GameObject loginPCCanvas;
    [Header("Score")]
    [SerializeField] GameObject scoreButton;
    [Header("Pole informacyjne")]
    [SerializeField] GameObject TMProInfo;
    [Header("Pole z werwsj¹ aplikacji")]
    [SerializeField] TextMeshProUGUI TMProVersion;

    public void Start()
    {
#if UNITY_EDITOR
        //if (PlayerPrefs.GetString(ConstantData.Email, "") == "")
        //{
        //    PlayerPrefs.SetString(ConstantData.Email, "damian.owczarek@o2.pl");
        //    PlayerPrefs.SetInt(ConstantData.SaveBestScoreSave, 2);
        //    PlayerPrefs.SetString(ConstantData.UserName, "Damian");
        //}

#endif
        //Wykonywane na ka¿dej platformie Windows, Linux, Mac


        //Dla innych platform ni¿ Android
        //#if !PLATFORM_ANDROID && !UNITY_EDITOR
        //        TMProVersion.text ="Windows";
        //#endif
        //Tylko dla platformy Androida
        //#if PLATFORM_ANDROID && !UNITY_EDITOR
        //        TMProVersion.text ="Android";
        //#endif
        if (PlayerPrefs.GetString(ConstantData.Email, "") == "")
        {
            //Jeœli u¿ytkownik nie jest jeszcze zalogowany
#if PLATFORM_STANDALONE && !UNITY_EDITOR
        scoreButton.SetActive(false);
        loginButton.SetActive(false);
        loginButtonPC.SetActive(true);
#endif
#if PLATFORM_ANDROID && !UNITY_EDITOR
        scoreButton.SetActive(false);
        loginButton.SetActive(true);
        loginButtonPC.SetActive(true);
#endif
#if UNITY_EDITOR
        scoreButton.SetActive(false);
        loginButton.SetActive(true);
        loginButtonPC.SetActive(true);
#endif
        }
        else
        {
            //Jeœli urzytkowanik jest ju¿ zalogowany
            //Wy³acza informacje o potrzebie zalogowania
            TMProInfo.SetActive(false);
            loginButton.SetActive(false);
            loginButtonPC.SetActive(false);
            scoreButton.SetActive(true);
            ShowInfo = false;
            CheckLogin = false;
        }
        //TMProVersion.text = "Version: " + PlayerPrefs.GetString(ConstantData.Email,"None");
        TMProVersion.text = "Version: " + Application.version;

    }
    private void Update()
    {
        if (CheckLogin) Start();
        if (ShowInfo) TMProInfo.SetActive(true);
        else TMProInfo.SetActive(false);
    }

    /// <summary>
    /// Pojazkuje listê punktacji
    /// </summary>
    public void ShowScoreCanvas()
    {
        gameObject.SetActive(false);
        canvasScore.SetActive(true);
    }
    public void ShowFeedbackCanvas()
    {
        gameObject.SetActive(false);
        canvasFeedback.SetActive(true);
    }

    /// <summary>
    /// Wczutanie sceny o konkretnej nnazwie
    /// </summary>
    /// <param name="sceneName">Nazwa sceny</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Wczytanie sceny o konkretnym indeksie
    /// </summary>
    /// <param name="sceneIndex">Indeks sceny</param>
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    /// <summary>
    /// Wyjœcie z gry
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}