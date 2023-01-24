using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTest : MonoBehaviour {

    [SerializeField] private int loops;

    private Renderer childrenRenderer;

    private void Awake() {
        childrenRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start() {
        transform.DOMove(new Vector3(1f, 0f, 0f), 2f)
            .SetEase(Ease.InOutQuart)
            .SetLoops(loops, LoopType.Incremental)
            .SetRelative(true);
        
        // renderer.material.DOFade(0.2f, 1f)
        //     .SetLoops(loops * 2, LoopType.Yoyo);
    }

    private void Update() {
    }

}
