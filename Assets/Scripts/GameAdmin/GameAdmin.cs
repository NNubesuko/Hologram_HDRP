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

    private string sentiment = string.Empty;

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
            // WebAPIのレスポンスから感情のリストを取得
            EmotionalAnalysisResult emotionalAnalysisResult = responceEmotionalAnalysis.result;
            List<EmotionalAnalysisPhrase> emotionalList = emotionalAnalysisResult.emotional_phrase;

            // 初期値をNeutralの0に設定
            int sentimentCount = 0;
            // 各フレーズごとに感情を分ける
            for (int i = 0; i < emotionalList.Count; i++) {
                sentimentCount += GetEmotionalNumber(emotionalList[i].emotion);
            }
            // 感情値から感情を取得
            sentiment = GetEmotional(sentimentCount);
        }

        // 感情により行動を分岐させる
        SelectSentiment(sentiment);
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
                break;
        }
    }

    private int GetEmotionalNumber(string emotional) {
        switch (emotional) {
            case "喜ぶ":
                return 1;
            case "怒る":
                return -1;
            case "悲しい":
                return -1;
            case "不安":
                return -1;
            case "恥ずかしい":
                return -1;
            case "好ましい":
                return 1;
            case "嫌":
                return -1;
            case "興奮":
                return 1;
            case "安心":
                return 1;
            case "驚く":
                return 0;
            case "切ない":
                return -1;
            case "願望":
                return 0;
            case "P":
                return 1;
            case "N":
                return -1;
            case "PN":
                return 0;
            default:
                return 0;
        }
    }

    private string GetEmotional(int value) {
        if (value >= 1) {
            return Sentiment.Positive;
        } else if (value <= -1) {
            return Sentiment.Negative;
        } else {
            return Sentiment.Neutral;
        }
    }

}

public class Sentiment {

    public const string Positive = "Positive";
    public const string Negative = "Negative";
    public const string Neutral = "Neutral";

}