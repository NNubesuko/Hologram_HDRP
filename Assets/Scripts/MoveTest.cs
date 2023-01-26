using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTest : MonoBehaviour {

    [SerializeField] private int loops;
    [SerializeField] private float movingTime;
    [SerializeField] private Transform movePoint;

    private Renderer childrenRenderer;

    private bool oneTime = true;

    private void Awake() {
        childrenRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start() {
        StartCoroutine(Move());
    }

    private void Update() {
        if (oneTime) {
            oneTime = false;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(
                childrenRenderer.material.DOFade(0f, 0.8f).SetDelay(0.2f)
            ).SetLoops(-1, LoopType.Yoyo);
            sequence.Play();
        }
    }

    private IEnumerator Move() {
        while (transform.position.x <= movePoint.position.x) {
            transform.DOMove(new Vector3(4f, 0f, 0f), 2f).SetEase(Ease.Linear).SetRelative(true);
            yield return new WaitForSeconds(1f);
            
            transform.DOPause();
            yield return new WaitForSeconds(1f);
            
            transform.DOPlay();

            yield return null;
        }
    }

}
