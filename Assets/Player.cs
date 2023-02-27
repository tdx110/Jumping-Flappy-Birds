using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    Vector3 startPosition;
    int actualHeight;

    [Header("Cia�o do sterowania")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [Header("Canvas z Game Over, jak przeciwnik przegra")]
    [SerializeField] private GameObject canvasGameOver;
    [Header("Canvas podczas grania")]
    [SerializeField] private GameObject canvasControl;
    [Header("Przycisk Pauza")]
    [SerializeField] private GameObject buttonPause;
    [Header("Pole tekstowe z liczb� punk�w")]
    [SerializeField] private TextMeshProUGUI tMProPoint;
    [Header("Pole tekstowe z najlepszym wynikiem")]
    [SerializeField] private TextMeshProUGUI tMProBestScore;
    [Header("Pole tekstowe z aktualnym wynikiem gracza")]
    [SerializeField] private TextMeshProUGUI tMProPlayerScore;
    [Header("Pole z tutorialem w przycisku")]
    [SerializeField] private GameObject textTutorial;

    //napisy do pola tekstowego
    private string textScore = "Your score:\n";
    private string textBestScore = "Best score: ";
    private string textScoreInGame = "Score: ";
    private bool showTutorial = true;

    private void Awake()
    {
        startPosition = this.gameObject.transform.position;
        actualHeight = Display.main.systemHeight;
        CorrectPosition();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Display.main.systemHeight != actualHeight)
        {
            CorrectPosition();
            actualHeight = Display.main.systemHeight;
        }
    }
    //Na samym pocz�tku poprawia pozycj�gracza wzgl�dem ekranu
    private void CorrectPosition() { this.gameObject.transform.position = CorrectPosition(startPosition); }
    private Vector3 CorrectPosition(Vector3 position)
    {
        //Pocz�tkowa rozdzielczo�� przy edycji jest zawsze 1080x1920
        float size = Camera.main.orthographicSize * 2; //Ile jednostek jest na wysoko�� ekranu
        float standargRatio = 1920 / size; //Ile pixeli jest na jedn� jednostk� w Unity przy programowaniu
        float newRatio = Display.main.systemHeight / size; //Ile jest bierz�co pixeli na jedn� jednostk� Unity
        float unityWidthStandard = 1080 / standargRatio; //Ile jednostek Unity jest w szeroko�ci
        float unityWidthNew = Display.main.systemWidth / newRatio; // Ile jednostek jednostek Unity mie�ci si� w nowej szeroko�ci
        float multiplier = standargRatio / newRatio; // Mno�nik do zmiany pozycji (je�li ma by� odleg�o�� proporcjonalna)
        float delta = (unityWidthStandard / 2) + position.x;

        //Vector3 newPosition = position * multiplier;
        Vector3 TEMPPos;
        TEMPPos = new Vector3(-(unityWidthNew / 2) + delta, position.y, position.z);
        return TEMPPos;
    }

    public void Jump()
    {
        if (showTutorial)
        {
            textTutorial.SetActive(false);
            buttonPause.SetActive(true);
            showTutorial = false;
        }
        //Sprawdza czy dopiero co uruchomiony zosta� poziom
        if (NewGameVariable.instance.StartGame)
        {
            NewGameMenuSctript.instance.PauseApplication();
            NewGameVariable.instance.StartGame = false;
            NewGameVariable.instance.Move = true;
            buttonPause.SetActive(true);
        }
        else
        {
            //Zabezpieczenie przed mo�liwo�ci� naciskania skoku podczas pauzy
            if (Time.timeScale != 0)
            {
                float jumpPower;
                if (playerRigidbody.velocity.y <= 0)
                {
                    //Je�li gracz opada� to od razu podskakuje do g�ry
                    jumpPower = StaticVariable.PowerJump;
                }
                else
                {
                    //Wi�ksza vektor poruszania si� w g�r� o warto�� skoku
                    jumpPower = StaticVariable.PowerJump + playerRigidbody.velocity.y;
                }
                playerRigidbody.velocity = new Vector2(0, jumpPower);
                //playerRigidbody.AddForce(Vector2.up * StaticVariable.PowerJump, ForceMode2D.Impulse);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Point")
        {
            //Dodanie punktu za przej�cie przeszkody
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            NewGameVariable.instance.Score += collision.gameObject.transform.parent.GetComponent<Obstacle>().Point;
            tMProPoint.text = textScoreInGame + NewGameVariable.instance.Score.ToString();
        }
        else if (collision.gameObject.tag == "Collision")
        {
            //Informuje �e gracz uderzy� w przeszkod�
            NewGameVariable.instance.GameOver = true;
            Time.timeScale = 0;
            //Pokazuje Canvas GameOver
            canvasControl.SetActive(false);
            canvasGameOver.SetActive(true);
            ///Pokazuje punktacje po zako�czonej grze
            ScoreFunction();
        }
    }

    /// <summary>
    /// Funkcja odpowiadaj�ca za obs�ug� punktacji po zako�czeniu gry
    /// </summary>
    private void ScoreFunction()
    {
        if (StaticFunction.CheckBestScore(NewGameVariable.instance.Score))
        {
            PlayerPrefs.SetInt(ConstantData.SaveBestScoreSave, NewGameVariable.instance.Score);
        }
        tMProBestScore.text = textBestScore + (PlayerPrefs.GetInt(ConstantData.SaveBestScoreSave, 0)).ToString();
        tMProPlayerScore.text = textScore + NewGameVariable.instance.Score.ToString();
    }
}
