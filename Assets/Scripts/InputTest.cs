using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class InputTest : MonoBehaviour {

    private DictationRecognizer dictationRecognizer;

    private void Start() {
        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += (text, confidence) => {
            Debug.LogFormat("Dictation result: {0}", text);
            dictationRecognizer.Stop();
        };
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            dictationRecognizer.Start();
        }
    }

}
