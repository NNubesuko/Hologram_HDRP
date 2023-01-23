using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ResponceEmotionalAnalysis {

    public EmotionalAnalysisResult result;
    public int status;
    public string message;

}

[Serializable]
public class EmotionalAnalysisResult {

    public string sentiment;
    public float score;
    public List<EmotionalAnalysisPhrase> emotional_phrase;

}

[Serializable]
public class EmotionalAnalysisPhrase {

    public string form;
    public string emotion;

}