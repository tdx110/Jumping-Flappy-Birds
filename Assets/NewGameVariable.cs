using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameVariable : MonoBehaviour
{
    public static NewGameVariable instance;

    [Header("Kamera g��wna")]
    [SerializeField] Camera mainCamera;
    [Header("Granica g�rna")]
    [SerializeField] GameObject upColission;
    [Header("Granica dolna")]
    [SerializeField] GameObject downColission;

    [Header("Czy jest koniec gry")]
    public bool GameOver = false;
    [Header("Zmienna z wynikiem w danej grze")]
    public int Score = 0;
    //Sprawdza czy dopiero zosta�a uruchomiona gra
    public bool StartGame { get;set; }
    //Czy mo�na przesuwa� plansz�
    public bool Move { get; set; }
    //Z jak� pr�dko�ci� przesuwa� plansz�
    public float MoveSpeed { get; set; }
    //Czy mo�na stawia� ju� przeszkody
    public bool GenerateObstacle { get; set; }
    //Czy generuje pierwsz� przeszkod� czy kolejne
    public bool GenerateFirstObstacle { get; set; }
    //Krzywe w zale�no�ci od uzyskanego wyniku
    //Krzywa z czasem jaki potrzebny jest do stworzenia nowej przeszkody
    [Header("Czas jaki jest potrzedny do stworzenia przeszkody", order = 2)]
    public AnimationCurve[] TimeRespawnArray;
    //Krzywa z odleg�o�ci� mi�dzy przeszkodami
    [Header("Odleg�o�� mi�dzy filarami przeszkodami")]
    [Tooltip("Nie mo�e by� za ma�a aby gracz m�g� trafi� w ni�.")]
    public AnimationCurve[] DistanceBetweenObstacleArray;
    //Krzywa z zakresem maksymalnych pozycji w jakich mo�e by� przeszkoda
    [Header("Zakres pocycji w jakiej mo�e by� przeszkoda")]
    [Tooltip("Przeszkoda b�dzie znajdowa� si� od tej warto�ci tylko �e ujemnej do dodatniej.")]
    public AnimationCurve[] PositionObstaclelArray;
    private void Awake()
    {
        Time.timeScale = 0;
        MoveSpeed = 1f;
        StartGame = true;
        Move = false;
        GenerateObstacle = true;
        #region Singletin
        if (!instance)
        {
            instance = this.gameObject.GetComponent<NewGameVariable>();
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log("Istnieje wi�cej plik�w z t� instancj�.\n" +
                "Zosta�y automatycznie usuni�te ze sceny", this);
        }
        #endregion
    }

}
