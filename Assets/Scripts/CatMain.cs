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
    [SerializeField] private float rotateMagnification;

    private Material material;
    private bool oneTime = true;

    private float rotateY = 0f;

    private void Awake() {
        material = GetComponent<Renderer>().material;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            oneTime = true;
        }

        if (oneTime) {
            oneTime = false;
            Attach();
        }

        transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
        rotateY += rotateMagnification * Time.deltaTime;
    }

    /*
     * 生成したテクスチャをオブジェクトに付けるメソッド
     */
    private void Attach() {
        Texture2D attachTexture = CreateTexture.Create(
            gameAdmin.textureSize,
            gameAdmin.textureSize,
            Brushes.Transparent,
            gameAdmin.fontSize,
            new FontFamily("游明朝"),
            Brushes.White,
            FormatText(gameAdmin.inputText, gameAdmin.textLength)
        );

        attachTexture.filterMode = FilterMode.Point;
        attachTexture.Apply();

        material.SetTexture("_BaseMap", attachTexture);
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

}
