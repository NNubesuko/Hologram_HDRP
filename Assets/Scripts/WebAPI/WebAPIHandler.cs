using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebAPIHandler {

    public static IEnumerator WebRequest(
        string url,
        string method,
        string sendJsonData,
        UnityAction<UnityWebRequest> callback,
        params RequestHeader[] headers
    ) {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(sendJsonData);

        // UnityWebRequestが使用されなくなったら、自動的にリソースをクリーンアップする
        using UnityWebRequest request = new UnityWebRequest(url, method);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        foreach (RequestHeader requestHeader in headers) {
            request.SetRequestHeader(requestHeader.Name, requestHeader.Value);
        }

        yield return request.SendWebRequest();
        callback(request);
    }

}