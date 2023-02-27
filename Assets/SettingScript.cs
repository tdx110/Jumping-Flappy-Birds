using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingScript : MonoBehaviour
{
    [Header("Pole startowe do którego wraca BACK")]
    [SerializeField] GameObject startCanvas;
    [Header("DŸwiêk skoku do przeniesienia wartoœci g³oœnoœci")]
    [SerializeField] private AudioSource jumpAudio;
    [Header("DŸwiêk T³a do przeniesienia wartoœci g³oœnoœci")]
    [SerializeField] private AudioSource backgroundAudio;
    [Header("Obiekty w polu g³ównym Setting")]
    [SerializeField] private SettingClass settingClass;
    [Header("Obiekty w panelu ustawieñ dŸwiêku.")]
    [SerializeField] private SoundClass soundClass;
    [Header("Miejsce gdzie przechowywane s¹ dŸwiêki")]
    [SerializeField] Animator animatorRotation;

    private Scene scene;
    public static SettingScript instance;

    private void Start()
    {
        //Pobiera dane o aktualnej scenie
        scene = SceneManager.GetActiveScene();
        //Singleton
        if (!instance)
        {
            instance = gameObject.GetComponent<SettingScript>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    //Sprawdza ustawienia przy ka¿dym w³aczeniu
    public void OnEnable()
    {
        //Odczytuje ustawienia g³oœnoœci i ustawia Slidery
        soundClass.JumpSlider.value = PlayerPrefs.GetFloat(ConstantData.SoundJump,1);
        soundClass.BackgroundSlider.value = PlayerPrefs.GetFloat(ConstantData.SoundBackground,1);
        //Odczytuje ustawienia sterowania i wpisuje w przycisk
        settingClass.TMProControlHand.text = ConstantData.ControlOptions[PlayerPrefs.GetInt(ConstantData.SaveControl, 0)];
        settingClass.TMProDifficultyLevel.text = ConstantData.DifficultyLevelOptions[PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave, 0)];
    }

    /// <summary>
    /// Zmienia ustawienie trzymania telefonu
    /// </summary>
    public void ChangeHand()
    {
        if (PlayerPrefs.GetInt(ConstantData.SaveControl, 0) == 0)
        {
            //Zmienia sterowanie z prawej na lew¹ rêkê
            PlayerPrefs.SetInt(ConstantData.SaveControl, 1);
        }
        else
        {
            //Zmienia sterowanie z lewej na praw¹ rêkê
            PlayerPrefs.SetInt(ConstantData.SaveControl, 0);
        }
        settingClass.TMProControlHand.text = ConstantData.ControlOptions[PlayerPrefs.GetInt(ConstantData.SaveControl, 0)];

    }
    public void ChangeDifficultyLevel()
    {
        switch (PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave, 0))
        {
            case 0:
                PlayerPrefs.SetInt(ConstantData.DifficultyLevelSave, 1);
                break;
            case 1:
                PlayerPrefs.SetInt(ConstantData.DifficultyLevelSave, 2);
                break;
            case 2:
                PlayerPrefs.SetInt(ConstantData.DifficultyLevelSave, 0);
                break;
            default:
                break;
        }
        settingClass.TMProDifficultyLevel.text = ConstantData.DifficultyLevelOptions[PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave, 0)];
    }

    //Metody u¿ywane do powrotu
    public void ShowHideSettingCanvas()
    {
        if (startCanvas.activeSelf)
        {
            startCanvas.SetActive(false);
            settingClass.SettingCanvas.SetActive(true);
        }
        else
        {
            startCanvas.SetActive(true);
            settingClass.SettingCanvas.SetActive(false);
        }
    }
    //Pokazuje lub ukrywa panel z ustawieniami dŸwiêku
    public void ShowHideSoundCanvas()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (soundClass.SoundCanvas.activeSelf)
        {
            soundClass.SoundCanvas.SetActive(false);
            settingClass.SettingCanvas.SetActive(true);
            //Zapisuje wartoœci z Slider-ów w prefabach
            if (scene.name != "Start")
            {
                jumpAudio.volume = soundClass.JumpAudio.volume;
                backgroundAudio.volume = soundClass.BackgroundAudio.volume;
            }
            PlayerPrefs.SetFloat(ConstantData.SoundJump, soundClass.JumpAudio.volume);
            PlayerPrefs.SetFloat(ConstantData.SoundBackground, soundClass.BackgroundAudio.volume);
        }
        else
        {
            soundClass.SoundCanvas.SetActive(true);
            settingClass.SettingCanvas.SetActive(false);
        }
    }

    public void RotationScreen()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.Portrait;
            animatorRotation.SetBool("Portrait", false);
            animatorRotation.SetBool("Landspace", true);
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            animatorRotation.SetBool("Portrait", true);
            animatorRotation.SetBool("Landspace", false);
        }
    }

    [Serializable]
    public class SoundClass
    {
        public GameObject SoundCanvas;
        public Slider JumpSlider;
        public AudioSource JumpAudio;
        public Slider BackgroundSlider;
        public AudioSource BackgroundAudio;
    }
    [Serializable]
    public class SettingClass
    {
        public GameObject SettingCanvas;
        [Header("Napis w przycisku odnoœnie rodzaju sterowania")]
        public TextMeshProUGUI TMProControlHand;
        [Header("Napis w przycisku odnoœnie poziomu trudnoœci")]
        public TextMeshProUGUI TMProDifficultyLevel;
    }
}
