using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using UDPKing;

public class ScoreScript : MonoBehaviour
{
    [Header("Kolory pól tekstowych")]
    [SerializeField]
    private Color whiteColor, playerColor;

    [Header("Pole tekstowe do informowania o stanie pobieania listy punktacji")]
    [Tooltip("Pole w którym bêd¹ umieszczane informacje o tym jak przebiega" +
        "proces pobierania i aktualizacji danych")]
    [SerializeField]
    private TextMeshProUGUI textMeshDownloadInfo;

    [Header("Canvas Score")]
    [SerializeField]
    private GameObject scoreCanvas;
    [Header("StartCanvas")]
    [SerializeField]
    private GameObject startCanvas;
    [Header("Panel ze zmian¹ nazwy gacza.")]
    [SerializeField]
    private GameObject changeUserNamePanel;
    [Header("Pola na dane gracza")]
    [SerializeField]
    private TextMeshProUGUI numberUserTMPro;
    [SerializeField]
    private TextMeshProUGUI nameUserTMPro;
    [SerializeField]
    private TextMeshProUGUI scoreUserTMPro;

    [SerializeField]
    [Header("Obiekty z polami na zajêtym miejscu")]
    private GameObject[] positionList;
    //Lista z polami tekstowymi Position
    private List<TextMeshProUGUI> positionTMProList;
    //Lista z polami tekstowymi Name
    private List<TextMeshProUGUI> nameTMProList;
    //Lista z polami tekstowymi Score
    private List<TextMeshProUGUI> scoreTMProList;

    //[SerializeField]
    //private TextMeshProUGUI textMeshScore;
    //Pobrane dane zamienione na tablice
    private string[] arrayScoreList;
    //Tabele z pobranymi danymi urzytkownika
    //Tablica z pozycjami urzytkownika
    private int[] positionDownloadList;
    //Tablica z nazwami U¿ytkowników
    private string[] nameDownloadList;
    //Tablica z punktami u¿ytknowników
    private int[] scoreDownloadList;

    private void OnEnable()
    {
        GetLeaderboard();
    }

    //Funkcje do obs³ugi przetwarzania stron internetowych


    public async void GetLeaderboard()
    {
        EmptyPosition();

        string[] nameArray = { "email", "name", "score" };
        string[] valueArray = {PlayerPrefs.GetString(ConstantData.Email,""),
            PlayerPrefs.GetString(ConstantData.UserName, "Random"), PlayerPrefs.GetInt(ConstantData.SaveBestScoreSave,0).ToString() };
        textMeshDownloadInfo.text = "Download data";
        string webResult = await WebScript.instance.GetWeb(ConstantData.ServerName + "one_command.php", nameArray, valueArray);
        string[] resultArray = webResult.Split(";");
        //Zapisywanie pobranych wartoœci
        PlayerPrefs.SetString(ConstantData.UserName, resultArray[0]);
        PlayerPrefs.SetInt(ConstantData.Position, int.Parse(resultArray[1]));
        PlayerPrefs.SetInt(ConstantData.SaveBestScoreSave, int.Parse(resultArray[2]));
        string[] rankingArray = { resultArray[3], resultArray[4], resultArray[5] };
        textMeshDownloadInfo.text = "Sorting data.";
        SortingDownloadList(rankingArray);
        ShowScoreList();
        ShowUserData();
        Debug.Log(webResult);
    }


    /// <summary>
    /// Zamyka ranking i otwiera pole startowe
    /// </summary>
    public void BackButton()
    {
        scoreCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }
    /// <summary>
    /// Czyœci wszystkie wyœwietlane pozycje
    /// </summary>
    public void EmptyPosition()
    {
        //Czyœci tablice przed w³o¿eniem do nich elementów
        positionTMProList = new List<TextMeshProUGUI>();
        nameTMProList = new List<TextMeshProUGUI>();
        scoreTMProList = new List<TextMeshProUGUI>();
        //Rozdziela wszystkie elementy w wyœwietlanej tablicy na posortowane na Miejsce, Imie i Punkty
        //Oraz czyszczenie ich aby nic nie wyœwietla³y
        foreach (GameObject item in positionList)
        {
            positionTMProList.Add(item.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            nameTMProList.Add(item.transform.GetChild(1).GetComponent<TextMeshProUGUI>());
            scoreTMProList.Add(item.transform.GetChild(2).GetComponent<TextMeshProUGUI>());
            item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            item.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }
    }
    /// <summary>
    /// Pokazuje pole do zmiany imienia
    /// </summary>
    public void SchowChangeName()
    {
        changeUserNamePanel.SetActive(true);
        scoreCanvas.SetActive(false);
    }
    /// <summary>
    /// Uk³ada do odpowiednich tabeli pobrane dane
    /// </summary>
    public void SortingDownloadList(string downloadDATA)
    {
        string[] TEMPArray, TEMPArrayString;
        positionDownloadList = new int[0];
        nameDownloadList = new string[0];
        scoreDownloadList = new int[0];

        textMeshDownloadInfo.text = "Sorting list.";

        TEMPArray = downloadDATA.Split(";");
        TEMPArrayString = TEMPArray[0].Split(",");

        foreach (string item in TEMPArrayString)
        {
            if (item != "")
            {
                Array.Resize(ref positionDownloadList, positionDownloadList.Length + 1);
                positionDownloadList[positionDownloadList.Length - 1] = Convert.ToInt32(item);
            }
        }
        TEMPArrayString = TEMPArray[1].Split(",");
        foreach (string item in TEMPArrayString)
        {
            if (item != "")
            {
                Array.Resize(ref nameDownloadList, nameDownloadList.Length + 1);
                nameDownloadList[nameDownloadList.Length - 1] = item;
            }
        }
        TEMPArrayString = TEMPArray[2].Split(",");
        foreach (string item in TEMPArrayString)
        {
            if (item != "")
            {
                Array.Resize(ref scoreDownloadList, scoreDownloadList.Length + 1);
                scoreDownloadList[scoreDownloadList.Length - 1] = Convert.ToInt32(item);
            }
        }
    }
    public void SortingDownloadList(string[] downloadDATA)
    {
        string[] TEMPArrayString;
        positionDownloadList = new int[0];
        nameDownloadList = new string[0];
        scoreDownloadList = new int[0];

        textMeshDownloadInfo.text = "Sorting list.";

        TEMPArrayString = downloadDATA[0].Split(",");

        foreach (string item in TEMPArrayString)
        {
            if (item != "")
            {
                Array.Resize(ref positionDownloadList, positionDownloadList.Length + 1);
                positionDownloadList[positionDownloadList.Length - 1] = Convert.ToInt32(item);
            }
        }
        TEMPArrayString = downloadDATA[1].Split(",");
        foreach (string item in TEMPArrayString)
        {
            if (item != "")
            {
                Array.Resize(ref nameDownloadList, nameDownloadList.Length + 1);
                nameDownloadList[nameDownloadList.Length - 1] = item;
            }
        }
        TEMPArrayString = downloadDATA[2].Split(",");
        foreach (string item in TEMPArrayString)
        {
            if (item != "")
            {
                Array.Resize(ref scoreDownloadList, scoreDownloadList.Length + 1);
                scoreDownloadList[scoreDownloadList.Length - 1] = Convert.ToInt32(item);
            }
        }
    }
    /// <summary>
    /// Wype³nia pola pozycji.
    /// </summary>
    public void ShowScoreList()
    {

        for (int i = 0; i < positionDownloadList.Length; i++)
        {
            positionTMProList[i].text = positionDownloadList[i].ToString();
            if (positionDownloadList[i] == PlayerPrefs.GetInt(ConstantData.Position, 0))
            {
                positionTMProList[i].color = playerColor;
                nameTMProList[i].color = playerColor;
                scoreTMProList[i].color = playerColor;
                positionTMProList[i].fontStyle = FontStyles.Bold;
                nameTMProList[i].fontStyle = FontStyles.Bold;
                scoreTMProList[i].fontStyle = FontStyles.Bold;

            }
            else
            {
                positionTMProList[i].color = whiteColor;
                nameTMProList[i].color = whiteColor;
                scoreTMProList[i].color = whiteColor;
                positionTMProList[i].fontStyle = FontStyles.Normal;
                nameTMProList[i].fontStyle = FontStyles.Normal;
                scoreTMProList[i].fontStyle = FontStyles.Normal;
            }
        }
        for (int i = 0; i < nameDownloadList.Length; i++)
        {
            nameTMProList[i].text = nameDownloadList[i];
        }
        for (int i = 0; i < scoreDownloadList.Length; i++)
        {
            scoreTMProList[i].text = scoreDownloadList[i].ToString();
        }
    }
    public void ShowUserData()
    {
        numberUserTMPro.text = PlayerPrefs.GetInt(ConstantData.Position).ToString();
        nameUserTMPro.text = PlayerPrefs.GetString(ConstantData.UserName);
        scoreUserTMPro.text = PlayerPrefs.GetInt(ConstantData.SaveBestScoreSave).ToString();
    }

}