using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //Ile punkt�w jest przyznawanych za pokonanie danej przeszkody
    public int Point;

    //Trigery Przeszkody
    [SerializeField]
    private GameObject ObstacleUp;
    [SerializeField]
    private GameObject ObstacleDown;
    //Trigger do naliczania punkt�w
    [SerializeField]
    private GameObject PointTrigger;
    //Czy generowa� pierwsz� przeszkod� w pozycji 0
    private static bool firstObstacle = true;

    //Zmienne w scenie
    [SerializeField]
    private NewGameVariable sceneVariable;

    public void StartObstacle()
    {
        //Ustawia ilo�� punkt�w jaka jest za przej�cie tej przeszkody
        Point = PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave,0)+1;
        //W�acza mo�liwo�� naliczenia punkt�w
        PointTrigger.GetComponent<BoxCollider2D>().enabled = true;
        //Ustawia odleg�o�� mi�dzy przeszkodami
        float distance = setDistance();
        //Ustawia po�o�enie przeszk�d na planszy w osi Y
        setLocation();
    }
    private float setLocation()
    {
        Vector3 position;
        int difLevel = PlayerPrefs.GetInt(ConstantData.DifficultyLevelSave, 0);

        //Generuje pozycj� przeszkody
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

        //Pobiera jaka ma by� odleg�o�� mi�dzy przeszkodami
        float distance = StaticFunction.GetValueFromCurve(sceneVariable.DistanceBetweenObstacleArray[difLevel],(float) sceneVariable.Score);
        //Ustawia odled�o�ci miedzy przeszkodami
        ObstacleUp.transform.localPosition = new Vector3(ObstacleUp.transform.localPosition.x,
    distance, ObstacleUp.transform.localPosition.z);
        ObstacleDown.transform.localPosition = new Vector3(ObstacleDown.transform.localPosition.x,
    (-1) * distance, ObstacleDown.transform.localPosition.z);

        return distance;
    }
}
