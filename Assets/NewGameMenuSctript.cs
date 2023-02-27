using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameMenuSctript : MonoBehaviour
{
    [Header("Canvas pokazuj�cy si� podczas grania")]
    [SerializeField]private GameObject canvasControl;
    [Header("Canvas z Pauz�")]
    [SerializeField] private GameObject canvasPause;
    [Header("Miejsce przechowywania ustawie� sceny")]
    [SerializeField]private NewGameVariable sceneVariable;
    [Header("D�wi�k skoku")]
    [SerializeField] private AudioSource audioSourceJump;
    [Header("D�wi�k t�a")]
    [SerializeField] private AudioSource audioSourceBackground;

    public static NewGameMenuSctript instance;

    private void Start()
    {
        if (!instance)
        {
            instance = this.gameObject.GetComponent<NewGameMenuSctript>();
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("Nie mo�e by� dw�ch takich skrypt�w");
        }
        audioSourceBackground.volume= PlayerPrefs.GetFloat(ConstantData.SoundBackground,0.5f);
        audioSourceJump.volume= PlayerPrefs.GetFloat(ConstantData.SoundBackground,0.5f);
    }
    /// <summary>
    ///Metoda zatrzymania aplikacji przyciskiem Pause
    /// </summary>
    public void PauseApplication()
    {
        if (Time.timeScale == 0 && !sceneVariable.GameOver)
        {
            Time.timeScale = 1;
            audioSourceBackground.Play();
        }
        else
        {
            Time.timeScale = 0;
            audioSourceBackground.Pause();
        }
    }
    public void ShowHidePauseCanvas()
    {
        if (canvasControl.activeSelf)
        {
            canvasControl.SetActive(false);
            canvasPause.SetActive(true);
        }
        else
        {
            canvasControl.SetActive(true);
            canvasPause.SetActive(false);
        }
    }
    /// <summary>
    /// Wczytanie sceny
    /// </summary>
    /// <param name="sceneNumber">Numer sceny</param>
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
