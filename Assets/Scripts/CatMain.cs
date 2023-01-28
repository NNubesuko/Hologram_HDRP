using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CatMain : MonoBehaviour {

    [SerializeField] private GameAdmin gameAdmin;
    [SerializeField] private Renderer catRenderer;      // 猫のRenderer
    [SerializeField] private Transform initPoint;       // 初期位置
    [SerializeField] private Transform teleportPoint;   // テクスチャ設定後の転送先
    [SerializeField] private Transform movePoint;       // 移動先

    public UnityAction texturedCallback;

    private Material material;
    // 入力されたか
    private bool inputed = false;
    // テクスチャを適応することが出来たか
    private bool attachedTexture = false;
    // 絵画の場所に移動したか
    public bool teleported { get; private set; }
    // アニメーションが始まったか
    private bool startAnimation = false;

    private Vector3 lastPosition;

    private void Awake() {
        material = catRenderer.material;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            inputed = true;
        }

        if (inputed) {
            inputed = false;
            Attach();
        }

        if (attachedTexture) {
            attachedTexture = false;
            Invoke("ForwardToDestination", 5f);
        }

        if (teleported) {
            if (startAnimation) {
                startAnimation = false;

                FadeAnimation();
                StartCoroutine(Move());
            }
        }

        if (!(teleported | startAnimation)) {
            transform.position = initPoint.position;
            gameAdmin.ResetButterflies();
        }
    }

    /*
     * 生成したテクスチャをオブジェクトに付けるメソッド
     */
    private void Attach() {
        Texture2D attachTexture = CreateTexture.Create(
            gameAdmin.textureSize,
            gameAdmin.textureSize,
            Brushes.Black,
            gameAdmin.fontSize,
            new FontFamily("游明朝"),
            new SolidBrush(System.Drawing.Color.White),
            FormatText(gameAdmin.inputText, gameAdmin.textLength)
        );

        attachTexture.filterMode = FilterMode.Point;
        attachTexture.Apply();

        // 白い状態から出ないと文字色を変更できないため、白で塗りつぶす
        material.color = UnityEngine.Color.white;
        material.SetTexture("_BaseColorMap", attachTexture);
        
        attachedTexture = true;
        // コールバック実行
        texturedCallback?.Invoke();
    }

    /*
     * 文字列をテクスチャ用にフォーマットするメソッド
     */
    private string FormatText(string text, int length) {
        StringBuilder sb = new StringBuilder();

        while (sb.Length <= length) {
            sb.Append(text);
        }

        return sb.ToString();
    }

    private void ForwardToDestination() {
        transform.position = teleportPoint.position;
        teleported = true;
        startAnimation = true;
    }

    /// <summary>
    /// 絵の前を移動する際のメソッド
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move() {
        while (transform.position.x <= movePoint.position.x) {
            transform.DOMove(new Vector3(4f, 0f, 0f), 4f).SetEase(Ease.Linear).SetRelative(true);
            yield return new WaitForSeconds(2f);
            
            transform.DOPause();
            yield return new WaitForSeconds(2f);
            
            transform.DOPlay();

            yield return null;
        }

        InitParams();
    }

    /// <summary>
    /// 猫をフェードイン・フェードアウトするメソッド
    /// </summary>
    private void FadeAnimation() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(
            material.DOFade(0f, 1.8f).SetDelay(0.2f)
        ).SetLoops(10, LoopType.Yoyo);
        sequence.Play();
    }

    /// <summary>
    /// イベント処理で使用するパラメータの初期化
    /// </summary>
    private void InitParams() {
        inputed = false;
        attachedTexture = false;
        teleported = false;
        material.color = UnityEngine.Color.black;
        // 感情を初期化
        gameAdmin.InitSentiment();
    }

}
