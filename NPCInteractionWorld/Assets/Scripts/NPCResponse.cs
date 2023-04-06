using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using Unity.Collections;
using System;


public class NPCResponse : MonoBehaviour
{
    string baseUrl = "http://localhost:5000/";
    public string lastQuery;
    public string lastResponse;
    public string currentEmotion;
    public string bio;
    public string name;
    public string welcomeMessage;
    public bool loading = false;
    //Predefine
    public List<string> predefinedQuestions;


    public IEnumerator GenerateResponse(string query)
    {
        lastQuery = query;
        loading = true;
        string json = "{\"bio\": \"" + bio + "\",\"question\":\"" + query + "\"}";
        UnityWebRequest request = UnityWebRequest.Post(baseUrl, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding(true).GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string responseJson = request.downloadHandler.text;
            var responseData = JsonUtility.FromJson<APIReponse>(responseJson);
            Debug.Log(responseData);
            lastResponse = responseData.answer;
            currentEmotion = responseData.emotion;
        }
        request.Dispose();
        loading = false;
    }

    //Method
    public void PredefinedUserInputs(int index)
    {
        if (index >= 0 && index < predefinedQuestions.Count)
        {
            string selectedQuestion = predefinedQuestions[index];
            StartCoroutine(GenerateResponse(selectedQuestion));
        }
    }
}

[Serializable]
public class APIReponse
{
    public string answer = "sad";
    public string emotion = "Sorry, Something is wrong with my brain";
}

