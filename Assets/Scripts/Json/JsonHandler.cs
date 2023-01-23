using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHandler {

    public static string ToJson(object serializableClass) {
        return JsonUtility.ToJson(serializableClass);
    }

}
