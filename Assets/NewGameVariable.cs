using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameVariable : MonoBehaviour
{
    public static NewGameVariable instance;

    [Header("Kamera g³ówna")]
    [SerializeField] Camera mainCamera;
    [Header("Granica górna")]
    [SerializeField] GameObject upColission;
    [Header("Granica dolna")]
    [SerializeField] GameObject downColission;

    [Header("Czy jest koniec gry")]
    public bool GameOver = false;
    [Header("Zmienna z wynikiem w danej grze")]
    public int Score = 0;
    //Sprawdza czy dopiero zosta³a uruchomiona gra
    public bool StartGame { get;set; }
    //Czy mo¿na przesuwaæ planszê
    public bool Move { get; set; }
    //Z jak¹ prêdkoœci¹ przesuwaæ planszê
    public float MoveSpeed { get; set; }
    //Czy mo¿na stawiaæ ju¿ przeszkody
    public bool GenerateObstacle { get; set; }
    //Czy generuje pierwsz¹ przeszkodê czy kolejne
    public bool GenerateFirstObstacle { get; set; }
    //Krzywe w zale¿noœci od uzyskanego wyniku
    //Krzywa z czasem jaki potrzebny jest do stworzenia nowej przeszkody
    [Header("Czas jaki jest potrzedny do stworzenia przeszkody", order = 2)]
    public AnimationCurve[] TimeRespawnArray;
    //Krzywa z odleg³oœci¹ miêdzy przeszkodami
    [Header("Odleg³oœæ miêdzy filarami przeszkodami")]
    [Tooltip("Nie mo¿e byæ za ma³a aby gracz móg³ trafiæ w ni¹.")]
    public AnimationCurve[] DistanceBetweenObstacleArray;
    //Krzywa z zakresem maksymalnych pozycji w jakich mo¿e byæ przeszkoda
    [Header("Zakres pocycji w jakiej mo¿e byæ przeszkoda")]
    [Tooltip("Przeszkoda bêdzie znajdowaæ siê od tej wartoœci tylko ¿e ujemnej do dodatniej.")]
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
            Debug.Log("Istnieje wiêcej plików z t¹ instancj¹.\n" +
                "Zosta³y automatycznie usuniête ze sceny", this);
        }
        #endregion
    }

}
