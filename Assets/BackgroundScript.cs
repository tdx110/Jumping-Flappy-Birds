using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScript : MonoBehaviour
{
    //Tablica z dost�pnymi t�ami
    [SerializeField]
    private Sprite[] backgroundArray;
    [SerializeField] float backgroundDistance; // Odleg�o�� przy jakiej resetuje
    [SerializeField] float backgroundSpeed; // Pr�dko�� z jak� b�dzie si� przemieszcza�
    private int numberBackground;//Numer generowanego t�a
    private Image backgroundImage;//Obrazek t�a
    private RectTransform backgroundRectTransform; //Dane z wymiarami w�a

    void Start()
    {
        //Losowanie numeru t�a z dost�pnych w tablicy
        System.Random rnd = new System.Random();
        numberBackground = rnd.Next(0, backgroundArray.Length - 1);
        //Pobiera i ustawia dane
        backgroundImage = this.gameObject.GetComponent<Image>(); //Pobiera dane o obrazku
        backgroundImage.sprite = backgroundArray[numberBackground]; //Ustawia konkretne t�o
        backgroundRectTransform = this.gameObject.GetComponent<RectTransform>(); //Pobiera wymiary t�a
        backgroundRectTransform.sizeDelta = new Vector2(5000, 1750);
        backgroundRectTransform.anchoredPosition = new Vector2(0, 0);

    }
    private void FixedUpdate()
    {
        moveBackgroundVoid();
    }
    private void moveBackgroundVoid()
    {

        if (backgroundRectTransform.sizeDelta.x <backgroundDistance)
        {
            backgroundRectTransform.sizeDelta += new Vector2(backgroundSpeed * Time.fixedDeltaTime, 0);
        }
        else
        {
            backgroundRectTransform.sizeDelta =new Vector2(5000, backgroundRectTransform.sizeDelta.y);
        }
        //    float time = Time.fixedDeltaTime ;
        //    if (NewGameVariable.instance.Move)
        //    {

        //    }
        //    if (sceneVariable.Move)
        //    {
        //        foreach (GameObject item in gameObjectBackground)
        //        {
        //            item.transform.position += Vector3.left * time;
        //            //Sprawdza czy jedno z te� dosz�o do ganicy
        //            if (item.transform.localPosition.x <= -1 * moveBackground)
        //            {
        //                //Przesuwa z lewej na praw� stron� t�o, aby zap�tli�
        //                item.transform.localPosition = new Vector3(((gameObjectBackground.Length - 1) * moveBackground),
        //                    item.transform.localPosition.y, item.transform.localPosition.z);
        //            }
        //        }
        //    }
    }
}
