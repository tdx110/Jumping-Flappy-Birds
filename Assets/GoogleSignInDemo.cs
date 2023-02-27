using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoogleSignInDemo : MonoBehaviour
{
    [Header("Pole tekstowe z informacjami o zalogowaniu")]
    [SerializeField]
    private TextMeshProUGUI TMProInfo;
    public string webClientId = "1041847182862-fp9ot0n2f5gitbt9mrcn8dci0ukncc2k.apps.googleusercontent.com";

    private DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private GoogleSignInConfiguration configuration;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        //CheckFirebaseDependencies();
    }
    private void Start()
    {
        InitFirebase();
    }
    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    }
    public void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticationFinished);
    }
    void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            AddToInformation("Fault", true);
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Login Cancel", false);
        }
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    AddToInformation("Login Cancel", false);
                    return;
                }
                if (task.IsFaulted)
                {
                    AddToInformation("Fault", true);
                    return;
                }
                user = auth.CurrentUser;
                //Zapisuje w grze informacje o danych email
                PlayerPrefs.SetString(ConstantData.Email, user.Email);
                //Zapisuje informacje o nazwie gracza
                PlayerPrefs.SetString(ConstantData.UserName, user.DisplayName);
                StartScript.CheckLogin = true;
                StartScript.ShowInfo = false;
                AddToInformation("Login Success",false);
            });
        }
    }

    private void AddToInformation(string str, bool adding)
    {
        if (adding)
        {
            StartScript.ShowInfo = true;
            TMProInfo.text += "\n" + str;
        }
        else
        {
            StartScript.ShowInfo = true;
            TMProInfo.text = "\n" + str;
        }

    }
}
