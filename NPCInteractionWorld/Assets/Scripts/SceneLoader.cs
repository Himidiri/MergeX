using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System.Text;

public class SceneLoader : MonoBehaviour
{
    public GameObject loaderUI;
    public GameObject errorMsg;
    public Slider progressSlider;

    private const string apiUrl = "https://backend-ghumo23ctq-as.a.run.app/api/login/";

    // Reference to the input fields for username and password
    private TMP_InputField usernameInputField;
    private TMP_InputField passwordInputField;

    public GameObject userName;
    public GameObject password;

    public void Start()
    {
        usernameInputField = userName.GetComponent<TMP_InputField>();
        passwordInputField = password.GetComponent<TMP_InputField>();
        errorMsg.SetActive(false);
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadScene_Coroutine(index));
    }

    public IEnumerator LoadScene_Coroutine(int index)
    {
        progressSlider.value = 0;
        loaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;

        float progress = 0;

        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

    }

    public void Login()
    {
        StartCoroutine(sendHttpRequest());
    }

    IEnumerator sendHttpRequest()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

  

        var request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"email\":\"" + username + "\",\"password\":\"" + password + "\"}");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);

        if (request.responseCode == 200)
        {
            errorMsg.SetActive(false);
            LoadScene(1);
        }
        else
        {
            errorMsg.SetActive(true);
        }

        request.downloadHandler.Dispose();
        request.uploadHandler.Dispose();
        request.Dispose();
    }
}
