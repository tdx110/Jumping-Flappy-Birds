using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebScript : MonoBehaviour
{
    public static WebScript instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<WebScript>();

        }
    }


    public async Task<string> GetWeb(string Url, Dictionary<string, string> postDictionary)
    {
        WWWForm form = new WWWForm();
        //Do³¹cza dane POST do formularza
        foreach (KeyValuePair<string, string> item in postDictionary)
        {
            form.AddField(item.Key, item.Value, System.Text.Encoding.UTF8);
        }
        if (postDictionary.Count == 0)
        {
            Debug.LogWarning("Tablice nie s¹ tej samej d³ugoœci.");
            return "ArrayError";
        }
        //ustawia na jak¹ stronê maj¹ zostaæ jakie wys³ane dane POST
        UnityWebRequest request = UnityWebRequest.Post(Url, form);
        request.timeout = 20;
        request.SendWebRequest();
        while (!request.isDone)
        { await Task.Yield(); }
        //Sprawdza czy wszystko jes OK

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            //Informowanie o b³edzie w komunikacji
            Debug.LogWarning("B³¹d przy komunikacji: " + request.result);
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();
            return "ComunicationError";
        }
        else
        {
            string resend = request.downloadHandler.text;
            //Zwraca wynik, poniewa¿ jest wszystko OK
            //(Zwraca zawatroœæ pobranej strony internetowej)
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();

            return resend;
        }
    }
    public async Task<string> GetWeb(string Url, string[] NamePost, string[] ValuePost)
    {
        WWWForm form = new WWWForm();
        //Do³¹cza dane POST do formularza
        if (NamePost.Length != 0 & ValuePost.Length != 0)
        {
            if (NamePost.Length == ValuePost.Length)
            {
                for (int i = 0; i < NamePost.Length; i++)
                {
                    form.AddField(NamePost[i], ValuePost[i], System.Text.Encoding.UTF8);
                }
            }
            else
            {
                Debug.LogWarning("Tablice nie s¹ tej samej d³ugoœci.");
                return "ArrayError";
            }
        }
        //ustawia na jak¹ stronê maj¹ zostaæ jakie wys³ane dane POST
        UnityWebRequest request = UnityWebRequest.Post(Url, form);
        request.timeout = 20;
        request.SendWebRequest();
        while (!request.isDone)
        { await Task.Yield(); }
        //Sprawdza czy wszystko jes OK

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            //Informowanie o b³edzie w komunikacji
            Debug.LogWarning("B³¹d przy komunikacji: " + request.result);
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();
            return "ComunicationError";
        }
        else
        {
            string resend = request.downloadHandler.text;
            //Zwraca wynik, poniewa¿ jest wszystko OK
            //(Zwraca zawatroœæ pobranej strony internetowej)
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();

            return resend;
        }
    }
    public async Task<string> GetWeb(string Url, string NamePost, string ValuePost)
    {
        WWWForm form = new WWWForm();
        //Do³¹cza dane POST do formularza
        form.AddField(NamePost, ValuePost, System.Text.Encoding.UTF8);
        //ustawia na jak¹ stronê maj¹ zostaæ jakie wys³ane dane POST
        UnityWebRequest request = UnityWebRequest.Post(Url, form);
        request.timeout = 20;
        request.SendWebRequest();
        while (!request.isDone)
        { await Task.Yield(); }
        //Sprawdza czy wszystko jes OK

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            //Informowanie o b³edzie w komunikacji
            Debug.LogWarning("B³¹d przy komunikacji: " + request.result);
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();
            return "ComunicationError";
        }
        else
        {
            string resend = request.downloadHandler.text;
            //Zwraca wynik, poniewa¿ jest wszystko OK
            //(Zwraca zawatroœæ pobranej strony internetowej)
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();

            return resend;
        }
    }
    public async Task<string> GetWeb(string Url)
    {
        //ustawia na jak¹ stronê maj¹ zostaæ jakie wys³ane dane POST
        UnityWebRequest request = UnityWebRequest.Get(Url);
        request.timeout = 20;
        request.SendWebRequest();
        while (!request.isDone)
        { await Task.Yield(); }
        //Sprawdza czy wszystko jes OK

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            //Informowanie o b³edzie w komunikacji
            Debug.LogWarning("B³¹d przy komunikacji: " + request.result);
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();
            return "ComunicationError";
        }
        else
        {
            string resend = request.downloadHandler.text;
            //Zwraca wynik, poniewa¿ jest wszystko OK
            //(Zwraca zawatroœæ pobranej strony internetowej)
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.Dispose();

            return resend;
        }
    }
}
