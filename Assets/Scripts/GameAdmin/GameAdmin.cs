using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using DG.Tweening;

public class GameAdmin : MonoBehaviour {

    [SerializeField] private int length;
    [SerializeField] private int size;
    [SerializeField] private int font;

    [SerializeField] private GameObject catObject;

    [Header("アニメーション位置")]
    [SerializeField] private Transform teleportPoint;
    [SerializeField] private Transform movePoint;
    [SerializeField] private Renderer[] catsRenderer;

    [Header("ポジティブの場合")]
    [SerializeField] private GameObject butterfliesObject;

    [Header("ネガティブの場合")]
    [SerializeField] private Renderer[] grassRenderers;
    [SerializeField] private Light[] lights;

    public PhotoAdmin photoAdmin { get; private set; }

    public string inputText { get; private set; }

    public int textLength { get; private set; }
    public int textureSize { get; private set; }
    public int fontSize { get; private set; }

    public bool canInput;
    // 入力されたか
    public bool isInput { get; private set; }
    private bool isAccessWebapi;

    private CotohaAccessToken cotohaAccessToken;
    private CotohaEmotionalAnalysis cotohaEmotionalAnalysis;
    private CatMain catMain;

    private string sentiment = SentimentType.None;
    private bool positiveInProcess = true;
    private bool negativeInProcess = true;
    private bool neutralInProcess = true;

    private DictationRecognizer dictationRecognizer;

    // * 削除予定
    private void InitField(string text) {
        isInput = true;
        isAccessWebapi = true;

        photoAdmin.DisableShouldSwitchPicture();

        inputText = text;
        textLength = length;
        textureSize = size;
        fontSize = font;
    }

    private void Awake() {
        photoAdmin = GetComponent<PhotoAdmin>();

        cotohaAccessToken = GetComponent<CotohaAccessToken>();
        cotohaEmotionalAnalysis = GetComponent<CotohaEmotionalAnalysis>();
        catMain = catObject.GetComponent<CatMain>();

        canInput = true;
        isInput = false;
        isAccessWebapi = false;
    }

    private void Start() {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds = 10;

        dictationRecognizer.DictationResult += (text, confidence) => {
            Debug.Log(text);
            if (canInput) {
                canInput = false;
                InitField(text);
                dictationRecognizer.Stop();
            }
        };

        dictationRecognizer.DictationComplete += (completeCause) => {
            // 要因がタイムアウトなら再び起動
            if (completeCause == DictationCompletionCause.TimeoutExceeded) {
                dictationRecognizer.Start();
            }
        };

        dictationRecognizer.Start();
    }

    // その他判定はUpdateで行う
    private void Update() {
        // 感情分析をする
        sentiment = AnalyzeEmotional();
        // 感情により行動を分岐させる
        SelectSentimentType(sentiment);
    }

#region WebAPI処理
    // WebAPIの処理はFixedUpdateで行う
    private void FixedUpdate() {
        // アクセストークンの要求
        cotohaAccessToken.RequestAccessToken();

        // WebAPIに短時間で複数回要求してしまうと、無効なやり取りになってしまうため、一度だけ実行させる
        if (cotohaAccessToken.validAccessToken && isAccessWebapi) {
            isAccessWebapi = false;
            // 渡された文章の感情分析
            cotohaEmotionalAnalysis.RequestEmotionalAnalysis(
                cotohaAccessToken,
                inputText
            );
        }
    }

    private string AnalyzeEmotional() {
        string currentSentiment = sentiment;

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
            cotohaEmotionalAnalysis.InitResponceEmotionalAnalysis();

            // 感情値から感情を取得
            currentSentiment = GetEmotional(sentimentCount);
        }

        return currentSentiment;
    }
#endregion

#region 感情分析による処理
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
                    StartCoroutine(PositiveInProcess(butterfliesObject));
                }
                break;
            case SentimentType.Negative:
                if (negativeInProcess) {
                    negativeInProcess = false;
                    StartCoroutine(NegativeInProcess(grassRenderers, lights));
                }
                break;
            case SentimentType.Neutral:
                if (neutralInProcess) {
                    neutralInProcess = false;
                    StartCoroutine(PositiveInProcess(butterfliesObject));
                    StartCoroutine(NegativeInProcess(grassRenderers, lights));
                }
                break;
            default:
                Debug.Log("入力受付中");
                break;
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
#endregion

    private IEnumerator PositiveInProcess(GameObject vfx) {
        vfx.SetActive(true);
        Sequence sequence = DOTween.Sequence()
            .Append(vfx.transform.DOMoveX(5.5f, 19f).SetEase(Ease.Linear).SetRelative(true));
        sequence.Play();
        yield return sequence.WaitForCompletion();

        Vector3 butterfliesPosition = butterfliesObject.transform.position;
        butterfliesPosition.x = teleportPoint.position.x;
        butterfliesObject.transform.position = butterfliesPosition;

        vfx.SetActive(false);
    }

    private IEnumerator NegativeInProcess(Renderer[] renderers, Light[] lights) {
        for (int i = 0; i < lights.Length; i++) {
            lights[i].intensity = 0f;
        }

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < renderers.Length; i++) {
            sequence.Append(renderers[i].material.DOFade(0f, 5f)).SetLoops(2, LoopType.Yoyo);
        }
        sequence.Play();
        yield return sequence.WaitForCompletion();
        
        for (int i = 0; i < lights.Length; i++) {
            lights[i].intensity = 3000f;
        }

    }

#region bool処理
    public void DisableIsInput() {
        isInput = false;
    }

    public void EnableCanInput() {
        canInput = true;
    }
#endregion

    public void EnableDictationRecognizer() {
        dictationRecognizer.Start();
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
