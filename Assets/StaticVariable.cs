using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Definiowanie sta³ych, takich jak sta³e opcje i miejsca zapisu danych
public static class ConstantData
{
    //Adres serwera
#if UNITY_EDITOR
    //public static readonly string ServerName = "http://89.69.7.39:86/"; //- Adres serwer w komputerze
    public static readonly string ServerName = "http://127.0.0.1/"; //- Adres serwer w komputerze
#else
    public static readonly string ServerName = "http://89.69.7.39:86/";
#endif



    //Miejsce gdzie zapisana jest obecna pozycja gracza w rankingu (GetInt)
    public static readonly string Position = "Position";
    //Miejsce zapisu najlepszego wyniku (GetInt)
    public static readonly string SaveBestScoreSave = "BestScore";
    //Miejsce gdzie zapisana jest informacja o poziomie trudnoœci (GetInt)
    public static readonly string DifficultyLevelSave = "DifficultyLevel";
    // Miejsce zapisu informacji o odtwarzanym dŸwiêku (GetFloat)
    public static readonly string SoundBackground = "BackgroundSound";
    public static readonly string SoundJump = "JumpSound";
    //Miejsce zapisu informacji o rodzaju sterowania
    public static readonly string SaveControl = "Control";
    //Miejsce przechowywania zapisanego adresu email (GetString)
    public static readonly string Email = "Email";
    //Miejsce przechowywania nazwy u¿ytkownika (GetString)
    public static readonly string UserName = "UserName";


    //Opcje mo¿liwego sterowania
    public static readonly string[] ControlOptions = { "Right hand", "Left hand" };
    //Opcje poziomu trudnoœci
    public static readonly string[] DifficultyLevelOptions = { "Easy", "Medium", "Hard" };
    //Lista znaków niedozwolonych w nazwie gracza
    public static char[] BlockChar = { ';', ':', '\\', '/', '\'', '\"'};

}


public static class StaticVariable
{
    public static float PowerJump = 3;
}

public class StaticFunction
{

    /// <summary>
    /// Sprawdza czy jest nowy maksymalny wynik
    /// </summary>
    /// <param name="score">Bierz¹cy wynik do sprawdzenia</param>
    /// <returns>Bool. True jeœli jest nowy maksymalny wynik
    /// False - jeœli nie jest nowy maksymalny wynik</returns>
    public static bool CheckBestScore(int score)
    {
        int bestScore = PlayerPrefs.GetInt(ConstantData.SaveBestScoreSave, -1);
        if (bestScore < score)
        {
            PlayerPrefs.SetInt(ConstantData.SaveBestScoreSave, score);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Pobiera wartoœæ z krzywej
    /// </summary>
    /// <param name="animationCurve">Krzywa z wartoœciami</param>
    /// <param name="position">Pozycja na osi Time</param>
    /// <returns>Zwraca flot z wartoœci¹ z krzywej.
    /// Jeœli Position jest poza zakrezem Time to zwraca ostatni wynik</returns>
    public static float GetValueFromCurve(AnimationCurve animationCurve, float position)
    {
        //Pobiera maksymalny zakres na osi Time
        int maxTime = animationCurve.length - 1;
        float maxValue = animationCurve[maxTime].time;

        //Sprawdza czy Position jest poza osi¹ Time czy nie
        if (position > maxValue)
        {
            //Jeœli jest poza zakresem to zwracaj ostatni wynik
            return animationCurve.Evaluate(maxValue);
        }
        else
        {
            return animationCurve.Evaluate(position);
        }
    }

}
