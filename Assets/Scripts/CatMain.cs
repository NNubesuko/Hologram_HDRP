using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KataokaLib.System;

public class CatMain : MonoBehaviour {

    [SerializeField] private GameAdmin gameAdmin;
    [SerializeField] private Renderer catRenderer;      // 猫のRenderer
    [SerializeField] private Transform initPoint;       // 初期位置
    [SerializeField] private Transform teleportPoint;   // テクスチャ設定後の転送先
    [SerializeField] private Transform movePoint;       // 移動先
    [SerializeField] private float moveSpeed;

    private Material material;
    private bool oneTime = false;
    private bool attachedTexture = false;
    private bool teleported = false;

    private Vector3 lastPosition;

    private void Awake() {
        material = catRenderer.material;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            oneTime = true;
        }

        if (oneTime) {
            oneTime = false;
            Attach();
        }

        if (attachedTexture) {
            attachedTexture = false;
            Invoke("ForwardToDestination", 3f);
        }

        if (teleported) {
            Vector3 currentPosition = lastPosition = transform.position;

            currentPosition = Vector3.MoveTowards(
                currentPosition,
                movePoint.position,
                moveSpeed * Time.deltaTime
            );

            Vector3 diff = currentPosition - lastPosition;
            if (diff != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(diff, Vector3.up);
            }

            transform.position = currentPosition;
        }

        if (transform.position == movePoint.position) {
            teleported = false;
            transform.position = initPoint.position;
        }
    }

    /*
     * 生成したテクスチャをオブジェクトに付けるメソッド
     */
    private void Attach() {
        Texture2D attachTexture = CreateTexture.Create(
            gameAdmin.textureSize,
            gameAdmin.textureSize,
            // Brushes.Transparent,
            Brushes.Black,
            gameAdmin.fontSize,
            new FontFamily("游明朝"),
            Brushes.White,
            FormatText(gameAdmin.inputText, gameAdmin.textLength)
        );

        attachTexture.filterMode = FilterMode.Point;
        attachTexture.Apply();

        material.SetTexture("_BaseColorMap", attachTexture);
        attachedTexture = true;
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
    }

}
