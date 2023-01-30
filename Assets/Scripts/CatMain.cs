using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using KataokaLib.System;

public class CatMain : MonoBehaviour {

    [SerializeField] private GameAdmin gameAdmin;
    [SerializeField] private Renderer[] catsRenderer;
    [SerializeField] private Renderer myRenderer;      // 猫のRenderer
    [SerializeField] private Transform initPoint;       // 初期位置
    [SerializeField] private Transform teleportPoint;   // テクスチャ設定後の転送先
    [SerializeField] private Transform movePoint;       // 移動先

    private Texture2D createdTexutre;

    // テクスチャを適応することが出来たか
    private bool attachedTexture = false;
    // 絵画の場所に移動したか
    public bool teleported { get; private set; }
    // アニメーションが始まったか
    private bool startAnimation = false;
    public bool finishedAnimation { get; private set; }

    private Sequence sequence;

    private void Awake() {
        for (int i = 0; i < catsRenderer.Length; i++) {
            catsRenderer[i].enabled = false;
        }
    }

    private void Reset() {
        Debug.Log("Reset");
        gameAdmin.EnableCanInput();
        gameAdmin.EnableDictationRecognizer();
    }

    private void Update() {
        Vector3 rotate = transform.eulerAngles;
        rotate.y += 5f * Time.deltaTime;
        transform.eulerAngles = rotate;

        if (gameAdmin.isInput) {
            gameAdmin.DisableIsInput();
            Attach();
        }

        if (attachedTexture) {
            attachedTexture = false;
            Invoke("ForwardToDestination", 5f);
        }

        if (teleported) {
            if (startAnimation) {
                startAnimation = false;
                for (int i = 0; i < catsRenderer.Length; i++) {
                    FadeAnimation(catsRenderer[i]);
                }
                StartCoroutine(Move());
            }
        }

        if (finishedAnimation) {
            finishedAnimation = false;

            // パラメータを初期化
            InitParams();

            // マテリアルを初期化
            InitCatMaterial(myRenderer);

            // 位置を初期化
            transform.position = initPoint.position;

            Invoke("Reset", 1f);
        }
    }

    /*
     * 生成したテクスチャをオブジェクトに付けるメソッド
     */
    private void Attach() {
        createdTexutre = CreateTexture.Create(
            gameAdmin.textureSize,
            gameAdmin.textureSize,
            Brushes.Black,
            gameAdmin.fontSize,
            new FontFamily("游明朝"),
            new SolidBrush(System.Drawing.Color.White),
            FormatText(gameAdmin.inputText, gameAdmin.textLength)
        );

        createdTexutre.filterMode = FilterMode.Point;
        createdTexutre.Apply();

        AttachTexture(myRenderer);
        for (int i = 0; i < catsRenderer.Length; i++) {
            AttachTexture(catsRenderer[i]);
        }

        attachedTexture = true;
    }

    /*
     * テクスチャをアタッチするメソッド
     */
    public void AttachTexture(Renderer renderer) {
        // 白い状態から出ないと文字色を変更できないため、白で塗りつぶす
        renderer.material.color = UnityEngine.Color.white;
        renderer.material.SetTexture("_BaseColorMap", createdTexutre);
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
        int catIndex = -1;
        while (catIndex != 3) {
            yield return new WaitForSeconds(2f);

            if (catIndex != -1)
                catsRenderer[catIndex].enabled = false;
            catIndex = (catIndex + 1) % catsRenderer.Length;
            catsRenderer[catIndex].enabled = true;
            yield return new WaitForSeconds(2f);
        }

        catsRenderer[catIndex].enabled = false;
        finishedAnimation = true;
    }

    /// <summary>
    /// 猫をフェードイン・フェードアウトするメソッド
    /// </summary>
    private void FadeAnimation(Renderer renderer) {
        sequence = DOTween.Sequence();
        sequence.Append(
            renderer.material.DOFade(0f, 1.8f).SetDelay(0.2f)
        ).SetLoops(9, LoopType.Yoyo);
        sequence.Play();
    }

    /// <summary>
    /// イベント処理で使用するパラメータの初期化
    /// </summary>
    private void InitParams() {
        finishedAnimation = false;
        attachedTexture = false;
        teleported = false;
        // 感情を初期化
        gameAdmin.InitSentiment();
        gameAdmin.photoAdmin.EnableShouldSwitchPicture();
    }

    private void InitCatMaterial(Renderer renderer) {
        renderer.material.color = UnityEngine.Color.black;
    }

}
