using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PhotoAdmin : MonoBehaviour {

    [SerializeField] private GameObject[] photos;
    [SerializeField] private float photoChangeTime;

    private GameAdmin gameAdmin;

    private float currentTime;
    private int currentPhotoIndex;
    private GameObject currentPhotoObject;
    private bool shouldSwitchPicture = true;    // 写真を切り替える処理をするか 処理をしない場合は、Updateが処理されない

    private void Awake() {
        gameAdmin = GetComponent<GameAdmin>();

        currentTime = 0f;
        currentPhotoIndex = 0;

        for (int i = 1; i < photos.Length; i++) {
            FadePhotoMaterial(photos[i], 0f, 0f);
        }
    }

    private void Update() {
        if (!shouldSwitchPicture) return;

        // 何も入力がなければ、一定時間ごとに絵を切り替える
        currentTime += Time.deltaTime;
        if (currentTime >= photoChangeTime) {
            currentTime = 0f;

            FadePhotoMaterial(photos[currentPhotoIndex], 0f, 1f);
            photos[currentPhotoIndex].transform.DOMoveZ(10f, 1f);

            currentPhotoIndex = Normalize(currentPhotoIndex + 1, 0, photos.Length);

            FadePhotoMaterial(photos[currentPhotoIndex], 1f, 1f);
            photos[currentPhotoIndex].transform.DOMoveZ(6f, 1f);
        }
    }

    private void FadePhotoMaterial(GameObject photo, float target, float time) {
        photo.GetComponent<Renderer>().material.DOFade(target, time);
        photo.transform.GetChild(0).GetComponent<Renderer>().material.DOFade(target, time);
    }

    private int Normalize(int x, int min, int max) {
        int cycle = max - min;
        x = (x - min) % cycle + min;

        if (x < min)
            x += cycle;

        return x;
    }

    public void EnableShouldSwitchPicture() {
        shouldSwitchPicture = true;
    }

    public void DisableShouldSwitchPicture() {
        shouldSwitchPicture = false;
    }

}
