using System;

[Serializable]
public class RequestEmotionalAnalysis {

    public string sentence;

    public RequestEmotionalAnalysis(string sentence) {
        this.sentence = sentence;
    }

}