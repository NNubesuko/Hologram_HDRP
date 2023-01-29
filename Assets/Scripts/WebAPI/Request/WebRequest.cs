using System.Text;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebRequestMethod
{
    public const string GET = UnityWebRequest.kHttpVerbGET;
    public const string POST = UnityWebRequest.kHttpVerbPOST;
}

public class WebRequest
{
    public delegate void OnComplete(UnityWebRequest request);

    /// <summary>
    /// WebAPIにリクエストするメソッド（コルーチン使用）
    /// </summary>
    /// <param name="url">リクエスト先のURL</param>
    /// <param name="method">リクエスト方法<br>
    /// GET: WebRequestMethod.GET<br>
    /// POST: WebRequestMethod.POST</param>
    /// <param name="sendJsonData"></param>
    /// <param name="callback"></param>
    /// <param name="headers"></param>
    /// <returns>IEnumerator</returns>
    public static IEnumerator Request(
        string url,
        string method,
        string sendJsonData,
        UnityAction<UnityWebRequest> callback,
        params RequestHeader[] headers
    ) {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(sendJsonData);

        // UnityWebRequestが使用されなくなったら、自動的にリソースをクリーンアップする
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 渡されたリクエストヘッダーを全て設定する
            for (int i = 0; i < headers.Length; i++)
            {
                request.SetRequestHeader(headers[i].Name, headers[i].Value);
            }

            // 結果が返却されるまで待機する
            yield return request.SendWebRequest();
            // コールバックを実行する
            callback?.Invoke(request);
        }
    }

    /// <summary>
    /// WebAPIにリクエストするメソッド（UniTask使用）
    /// </summary>
    /// <param name="url">リクエスト先のURL</param>
    /// <param name="method">リクエスト方法<br>
    /// GET: WebRequestMethod.GET<br>
    /// POST: WebRequestMethod.POST</param>
    /// <param name="sendJsonData"></param>
    /// <param name="callback"></param>
    /// <param name="headers"></param>
    /// <returns>UniTask</returns>
    public static async UniTask UniTaskRequest(
        string url,
        string method,
        string sendJsonData,
        OnComplete callback,
        params RequestHeader[] headers
    ) {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(sendJsonData);

        // UnityWebRequestが使用されなくなったら、自動的にリソースをクリーンアップする
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 渡されたリクエストヘッダーを全て設定する
            for (int i = 0; i < headers.Length; i++)
            {
                request.SetRequestHeader(headers[i].Name, headers[i].Value);
            }

            // 結果が返却されるまで待機する
            await request.SendWebRequest();
            // コールバックを実行する
            callback?.Invoke(request);
        }
    }
}
