using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CotohaEmotionalAnalysis : MonoBehaviour {

    public ResponceEmotionalAnalysis responceEmotionalAnalysis { get; private set; }

    private const string url = "https://api.ce-cotoha.com/api/dev/nlp/v1/sentiment";

    private void Awake() {
        responceEmotionalAnalysis = null;
    }

    /*
     * 文章の感情分析結果をWebAPIに要求するメソッド
     */
    public async void RequestEmotionalAnalysis(
        CotohaAccessToken cotohaAccessToken,
        string textToAnalyze
    ) {

        string sendJsonData = CreateRequestEmotionalAnalysisJson(textToAnalyze);
        string accessToken = cotohaAccessToken.responceAccessToken.access_token;

        await WebRequest.UniTaskRequest(
            url,
            WebRequestMethod.POST,
            sendJsonData,
            ResponceEmotionalAnalysis,
            new RequestHeader("Content-Type", "application/json;charset=UTF-8"),
            new RequestHeader("Authorization", "Bearer " + accessToken)
        );

    }

    /*
     * 感情分析を要求する際のJsonを作成するメソッド
     */
    private string CreateRequestEmotionalAnalysisJson(string textToAnalyze) {
        RequestEmotionalAnalysis requestEmotionalAnalysis =
            new RequestEmotionalAnalysis(textToAnalyze);
        return JsonUtility.ToJson(requestEmotionalAnalysis);
    }

    /*
     * 要求した感情分析結果のJsonをプログラムで扱えるようにクラスに変換するメソッド
     */
    private void ResponceEmotionalAnalysis(UnityWebRequest request) {
        responceEmotionalAnalysis =
            JsonUtility.FromJson<ResponceEmotionalAnalysis>(request.downloadHandler.text);
    }

    public void InitResponceEmotionalAnalysis() {
        responceEmotionalAnalysis = null;
    }

}
