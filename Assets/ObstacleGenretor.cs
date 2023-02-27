using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenretor : MonoBehaviour
{
    public static ObstacleGenretor instance;
    //Pozycja startowa przeszkody
    [SerializeField]
    private float positionStartObstacle;

    //Lista przeszk�d
    [SerializeField]
    private GameObject[] gameObjectsList;

    //Lista z przeszkodami pouk�adana
    private Queue<GameObject> gameObjects = new Queue<GameObject>();
    //Licznik czasu do generowania przeszkody
    private float Timer = 0f;

    private void Awake()
    {
        #region Singletin
        if (!instance)
        {
            instance = this.gameObject.GetComponent<ObstacleGenretor>();
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log("Istnieje wi�cej plik�w z t� instancj�.\n" +
                "Zosta�y automatycznie usuni�te ze sceny", this);
        }
        #endregion
    }
    void Start()
    {
        //Dodaje wszystkie obiekty do listy Quene
        foreach (GameObject item in gameObjectsList)
        {
            gameObjects.Enqueue(item);
        }
    }
    private void FixedUpdate()
    {
        //Przesuwa wszystkie przeszkody w lewo
        moveObstacle();
    }
    void Update()
    {
        //Sprawdza o tworzy przeszkody (je�li mo�e)
        generateObstacle();
    }
    private void generateObstacle()
    {
        if (NewGameVariable.instance.GenerateObstacle)
        {
            int difLevel = PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave, 0);
            //Pobiera czas potrzebny do stworzenia przeszkody przed przeciwnikiem
            float resTime = StaticFunction.GetValueFromCurve(NewGameVariable.instance.TimeRespawnArray[difLevel],
                (float)NewGameVariable.instance.Score);

            //Generuje natychmiast pierwsz� przeszkod�
            if (NewGameVariable.instance.GenerateFirstObstacle)
            {
                NewGameVariable.instance.GenerateFirstObstacle = false;
                //Ustawia od razu bez czekania pierwsz� przeszkod�
                GameObject obstacle = gameObjects.Dequeue();
                obstacle.transform.position = new Vector3(positionStartObstacle, obstacle.transform.position.y,
                    obstacle.transform.position.z);
                gameObjects.Enqueue(obstacle);
                obstacle.GetComponent<Obstacle>().StartObstacle();
                return;
            }
            if (Timer >= resTime)
            {
                Timer = 0;
                //Pobiera przeszkod�, zmienia jej pozycj� aby by�a przed graczem
                //i odk�ada ja ostatnie miejsce
                GameObject obstacle = gameObjects.Dequeue();
                obstacle.transform.position = new Vector3(positionStartObstacle, obstacle.transform.position.y,
                    obstacle.transform.position.z);
                gameObjects.Enqueue(obstacle);
                obstacle.GetComponent<Obstacle>().StartObstacle();
            }
            else
            {
                Timer += Time.deltaTime;
            }
        }
    }

    //Przemieszczanie przeszk�d
    private void moveObstacle()
    {
        if (NewGameVariable.instance.Move)
        {
            //Przemieszcza wszystkie przeszkody w lewo
            foreach (GameObject item in gameObjectsList)
            {
                item.transform.position += Vector3.left * Time.deltaTime;
            }
        }
    }
}
