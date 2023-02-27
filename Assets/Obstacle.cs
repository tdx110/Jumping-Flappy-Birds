using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //Ile punktów jest przyznawanych za pokonanie danej przeszkody
    public int Point;

    //Trigery Przeszkody
    [SerializeField]
    private GameObject ObstacleUp;
    [SerializeField]
    private GameObject ObstacleDown;
    //Trigger do naliczania punktów
    [SerializeField]
    private GameObject PointTrigger;
    //Czy generowaæ pierwsz¹ przeszkodê w pozycji 0
    private static bool firstObstacle = true;

    //Zmienne w scenie
    [SerializeField]
    private NewGameVariable sceneVariable;

    public void StartObstacle()
    {
        //Ustawia iloœæ punktów jaka jest za przejœcie tej przeszkody
        Point = PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave,0)+1;
        //W³acza mo¿liwoœæ naliczenia punktów
        PointTrigger.GetComponent<BoxCollider2D>().enabled = true;
        //Ustawia odleg³oœæ miêdzy przeszkodami
        float distance = setDistance();
        //Ustawia po³o¿enie przeszkód na planszy w osi Y
        setLocation();
    }
    private float setLocation()
    {
        Vector3 position;
        int difLevel = PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave, 0);

        //Generuje pozycjê przeszkody
        if (firstObstacle)
        {
            firstObstacle = false;
            position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        }
        else
        {
            float deltaPosition = StaticFunction.GetValueFromCurve(sceneVariable.PositionObstaclelArray[difLevel], sceneVariable.Score);
            position = new Vector3(gameObject.transform.position.x, Random.Range((-1) * deltaPosition, deltaPosition),
                gameObject.transform.position.z);
        }
        gameObject.transform.position = position;
        return 0;
    }
    private float setDistance()
    {
        int difLevel = PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave, 0);

        //Pobiera jaka ma byæ odleg³oœæ miêdzy przeszkodami
        float distance = StaticFunction.GetValueFromCurve(sceneVariable.DistanceBetweenObstacleArray[difLevel],(float) sceneVariable.Score);
        //Ustawia odled³oœci miedzy przeszkodami
        ObstacleUp.transform.localPosition = new Vector3(ObstacleUp.transform.localPosition.x,
    distance, ObstacleUp.transform.localPosition.z);
        ObstacleDown.transform.localPosition = new Vector3(ObstacleDown.transform.localPosition.x,
    (-1) * distance, ObstacleDown.transform.localPosition.z);

        return distance;
    }
}
