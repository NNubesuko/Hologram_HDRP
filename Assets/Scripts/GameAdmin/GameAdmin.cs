using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KataokaLib.System;

public class GameAdmin : MonoBehaviour {

    [SerializeField] private string input = "";
    [SerializeField] private int length;
    [SerializeField] private int size;
    [SerializeField] private int font;

    public string inputText { get; private set; }
    public int textLength { get; private set; }
    public int textureSize { get; private set; }
    public int fontSize { get; private set; }

    private CotohaAccessToken cotohaAccessToken;
    private CotohaEmotionalAnalysis cotohaEmotionalAnalysis;

    private bool isInput = false;

    private string sentiment = "";

    // 削除予定
    private void InitField() {
        inputText = input;
        textLength = length;
        textureSize = size;
        fontSize = font;
    }

    private void Awake() {
        cotohaAccessToken = GetComponent<CotohaAccessToken>();
        cotohaEmotionalAnalysis = GetComponent<CotohaEmotionalAnalysis>();

        InitField();
    }

    // その他判定はUpdateで行う
    private void Update() {
        // todo: マイク入力完了に変更
        if (Input.GetKeyDown(KeyCode.Return)) {
            isInput = true;

            InitField();
        }

        // 感情分析結果を格納するクラスから、感情を取得
        ResponceEmotionalAnalysis responceEmotionalAnalysis =
            cotohaEmotionalAnalysis.responceEmotionalAnalysis;
        if (responceEmotionalAnalysis != null) {
            EmotionalAnalysisResult emotionalAnalysisResult = responceEmotionalAnalysis.result;
            sentiment = emotionalAnalysisResult.sentiment;
        }

        SelectSentiment(sentiment);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameAdministrator.QuitGame();
        }
    }

    // WebAPIの処理はFixedUpdateで行う
    private void FixedUpdate() {
        // アクセストークンの要求
        cotohaAccessToken.RequestAccessToken();

        // WebAPIに短時間で複数回要求してしまうと、無効なやり取りになってしまうため、一度だけ実行させる
        if (cotohaAccessToken.validAccessToken && isInput) {
            isInput = false;

            // 渡された文章の感情分析
            cotohaEmotionalAnalysis.RequestEmotionalAnalysis(
                cotohaAccessToken,
                inputText
            );
        }
    }

    private void SelectSentiment(string sentiment) {
        switch (sentiment) {
            case Sentiment.Positive:
                Debug.Log("ポジティブな処理");
                break;
            case Sentiment.Negative:
                Debug.Log("ネガティブな処理");
                break;
            case Sentiment.Neutral:
                Debug.Log("中立な処理");
                break;
            default:
                Debug.Log("何もしない");
                break;
        }
    }

}

public class Sentiment {

    public const string Positive = "Positive";
    public const string Negative = "Negative";
    public const string Neutral = "Neutral";

}