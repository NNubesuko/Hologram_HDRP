using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CotohaAccessToken : MonoBehaviour {

    public bool validAccessToken { get; private set; } = false;
    public ResponceAccessToken responceAccessToken { get; private set; } = null;
    public TimeSpan effectiveDate { get; private set; } = default;

    private const string accessTokenURL = "https://api.ce-cotoha.com/v1/oauth/accesstokens";
    private const string grantType = "client_credentials";
    private const string clientId = "3Tu4k9hZoI0m6gHyYkHjJAXpyInTcrxN";
    private const string clientSecret = "NoKKCOb72cfOIH9l";

    /*
     * アクセストークンをWebAPIに要求するメソッド
     */
    public void RequestAccessToken() {
        TimeSpan now = DateTime.Now.TimeOfDay;
        // 現在時刻が有効期限日に達していなければ、メソッドの処理を終了する
        if (now < effectiveDate) return;

        // アクセストークンを更新する必要があるため、現在のアクセストークンを無効であることにする
        validAccessToken = false;

        string sendJsonData = CreateRequestAccessTokenJson(grantType, clientId, clientSecret);

        StartCoroutine(
            WebAPIHandler.WebRequest(
                accessTokenURL,
                UnityWebRequest.kHttpVerbPOST,
                sendJsonData,
                ResponceAccessToken,
                new RequestHeader("Content-Type", "application/json")
            )
        );
    }

    /*
     * アクセストークンを要求する際のJsonを作成するメソッド
     */
    private string CreateRequestAccessTokenJson(
        string grantType,
        string clientId,
        string clientSecret
    ) {
        RequestAccessToken accessTokenJson = new RequestAccessToken(
            grantType,
            clientId,
            clientSecret
        );
        return JsonUtility.ToJson(accessTokenJson);
    }

    /*
     * 要求したアクセストークンのJsonをプログラムで扱えるように、クラスに変換するメソッド
     */
    private void ResponceAccessToken(UnityWebRequest request) {
        validAccessToken = request.result == UnityWebRequest.Result.Success;

        if (validAccessToken) {
            responceAccessToken =
                JsonUtility.FromJson<ResponceAccessToken>(request.downloadHandler.text);
            
            // 有効期限日を更新
            UpdateEffectiveDate(responceAccessToken);
        }
    }

    /*
     * 有効期限日を更新するメソッド
     */
    private void UpdateEffectiveDate(ResponceAccessToken responceAccessToken) {
        // 発行日時と有効期限をTimeSpanに変換し、計算しやすくする
        TimeSpan issuedAt = responceAccessToken.issued_at.MillisecondsToTimeSpan();
        TimeSpan expiresIn = responceAccessToken.expires_in.SecondsToTimeSpan();
        // 有効期限日を算出
        effectiveDate = issuedAt + expiresIn;
    }

}
