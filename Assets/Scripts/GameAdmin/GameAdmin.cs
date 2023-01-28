using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class GameAdmin : MonoBehaviour {

    [SerializeField] private string input = "";
    [SerializeField] private int length;
    [SerializeField] private int size;
    [SerializeField] private int font;

    [SerializeField] private GameObject catObject;

    [Header("アニメーション位置")]
    [SerializeField] private Transform teleportPoint;
    [SerializeField] private Transform movePoint;

    [Header("ポジティブの場合")]
    [SerializeField] private GameObject butterfliesObject;
    private VisualEffect butterfliesVFX;

    public string inputText { get; private set; }
    public int textLength { get; private set; }
    public int textureSize { get; private set; }
    public int fontSize { get; private set; }

    // 入力されたか
    public bool isInput { get; set; }
    // テクスチャを適応することが出来たか
    public bool attachedTexture { get; private set; }
    // 絵画の場所に移動したか
    public bool teleported { get; private set; }
    // アニメーションが始まったか
    public bool startAnimation { get; private set; }

    private CotohaAccessToken cotohaAccessToken;
    private CotohaEmotionalAnalysis cotohaEmotionalAnalysis;
    private CatMain catMain;

    private string sentiment = SentimentType.None;
    private bool positiveInProcess = true;
    private bool negativeInProcess = true;
    private bool neutralInProcess = true;

    // * 削除予定
    private void InitField() {
        inputText = input;
        textLength = length;
        textureSize = size;
        fontSize = font;
    }

    private void Awake() {
        cotohaAccessToken = GetComponent<CotohaAccessToken>();
        cotohaEmotionalAnalysis = GetComponent<CotohaEmotionalAnalysis>();
        catMain = catObject.GetComponent<CatMain>();

        butterfliesVFX = butterfliesObject.GetComponent<VisualEffect>();

        isInput = false;

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
            cotohaEmotionalAnalysis.InitResponceEmotionalAnalysis();
        }

        // 感情により行動を分岐させる
        SelectSentimentType(sentiment);
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

    /// <summary>
    /// 感情により行動を分岐するメソッド
    /// </summary>
    /// <param name="sentiment"></param>
    private void SelectSentimentType(string sentiment) {
        if (!catMain.teleported) return;

        switch (sentiment) {
            case SentimentType.Positive:
                if (positiveInProcess) {
                    positiveInProcess = false;
                    StartCoroutine(PositiveInProcess(butterfliesObject.transform, 3));
                }
                break;
            case SentimentType.Negative:
                if (negativeInProcess) {
                    negativeInProcess = false;
                    // コルーチン
                }
                break;
            case SentimentType.Neutral:
                if (neutralInProcess) {
                    neutralInProcess = false;
                    // コルーチン
                }
                break;
            default:
                Debug.Log("入力受付中");
                break;
        }
    }

    private IEnumerator PositiveInProcess(Transform vfx, int repeatCount) {
        vfx.DOMove(new Vector3(10f, 0f, 0f), 24f).SetEase(Ease.Linear).SetRelative(true);
        vfx.GetComponent<VisualEffect>().enabled = true;
        for (int i = 0; i < repeatCount; i++) {
            vfx.GetComponent<VisualEffect>().Play();
            yield return null;
        }
    }

    /// <summary>
    /// 渡されたフレーズが、ポジティブなら1、ネガティブなら-1、中立なら0を返却するメソッド
    /// </summary>
    /// <param name="emotional"></param>
    /// <returns></returns>
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

    /// <summary>
    /// どの感情に相当するか判定するメソッド
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string GetEmotional(int value) {
        if (value >= 1) {
            return SentimentType.Positive;
        } else if (value <= -1) {
            return SentimentType.Negative;
        } else {
            return SentimentType.Neutral;
        }
    }

    /// <summary>
    /// パラメータを初期化するメソッド
    /// </summary>
    public void InitSentiment() {
        sentiment = SentimentType.None;
        positiveInProcess = true;
        negativeInProcess = true;
        neutralInProcess = true;
    }

}

public class SentimentType {

    public const string None = "None";
    public const string Positive = "Positive";
    public const string Negative = "Negative";
    public const string Neutral = "Neutral";

}
