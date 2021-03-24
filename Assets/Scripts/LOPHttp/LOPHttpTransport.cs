using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO.Compression;
using System.IO;

public class LOPHttpTransport : MonoBehaviour
{
    //  Get
    public void Get(string uri, Dictionary<string, string> requestHeaders, Action<byte[]> onResult, Action<string> onError)
    {
        StartCoroutine(GetRoutine(uri, requestHeaders, onResult, onError));
    }

    private IEnumerator GetRoutine(string uri, Dictionary<string, string> requestHeaders, Action<byte[]> onResult, Action<string> onError)
    {
        using (var www = UnityWebRequest.Get(uri))
        {
            foreach (var headerPair in requestHeaders)
            {
                if (!string.IsNullOrEmpty(headerPair.Key) && !string.IsNullOrEmpty(headerPair.Value))
                {
                    www.SetRequestHeader(headerPair.Key, headerPair.Value);
                }
                else
                {
                    Debug.LogWarning("Null header: " + headerPair.Key + " = " + headerPair.Value);
                }
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                onResult?.Invoke(www.downloadHandler.data);
            }
        }
    }

    //  Post
    public void Post(string uri, string postData, Dictionary<string, string> requestHeaders, Action<byte[]> onResult, Action<string> onError)
    {
        StartCoroutine(PostRoutine(uri, postData, requestHeaders, onResult, onError));
    }

    private IEnumerator PostRoutine(string uri, string postData, Dictionary<string, string> requestHeaders, Action<byte[]> onResult, Action<string> onError)
    {
        using (var www = UnityWebRequest.Post(uri, postData))
        {
            foreach (var headerPair in requestHeaders)
            {
                if (!string.IsNullOrEmpty(headerPair.Key) && !string.IsNullOrEmpty(headerPair.Value))
                {
                    www.SetRequestHeader(headerPair.Key, headerPair.Value);
                }
                else
                {
                    Debug.LogWarning("Null header: " + headerPair.Key + " = " + headerPair.Value);
                }
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                onResult?.Invoke(www.downloadHandler.data);
            }
        }
    }

    //  Put
    public void Put(string uri, byte[] bodyData, Dictionary<string, string> requestHeaders, Action<string> onResult, Action<string> onError)
    {
        StartCoroutine(PutRoutine(uri, bodyData, requestHeaders, onResult, onError));
    }

    public void Put(string uri, string bodyData, Dictionary<string, string> requestHeaders, Action<byte[]> onResult, Action<string> onError)
    {
        StartCoroutine(PutRoutine(uri, bodyData, requestHeaders, onResult, onError));
    }

    private IEnumerator PutRoutine(string uri, byte[] bodyData, Dictionary<string, string> requestHeaders, Action<string> onResult, Action<string> onError)
    {
        using (var www = UnityWebRequest.Put(uri, bodyData))
        {
            foreach (var headerPair in requestHeaders)
            {
                if (!string.IsNullOrEmpty(headerPair.Key) && !string.IsNullOrEmpty(headerPair.Value))
                {
                    www.SetRequestHeader(headerPair.Key, headerPair.Value);
                }
                else
                {
                    Debug.LogWarning("Null header: " + headerPair.Key + " = " + headerPair.Value);
                }
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                try
                {
                    string response = "";
                    if (www.GetResponseHeader("Content-Encoding") == "gzip")
                    {
                        using (var memoryStream = new MemoryStream(www.downloadHandler.data))
                        {
                            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                            {
                                using (var streamReader = new StreamReader(gZipStream))
                                {
                                    response = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                    else
                    {
                        response = www.downloadHandler.text;
                    }

                    onResult?.Invoke(response);
                }
                catch (Exception e)
                {
                    onError?.Invoke(e.Message);
                }
            }
        }
    }

    private IEnumerator PutRoutine(string uri, string bodyData, Dictionary<string, string> requestHeaders, Action<byte[]> onResult, Action<string> onError)
    {
        using (var www = UnityWebRequest.Put(uri, bodyData))
        {
            foreach (var headerPair in requestHeaders)
            {
                if (!string.IsNullOrEmpty(headerPair.Key) && !string.IsNullOrEmpty(headerPair.Value))
                {
                    www.SetRequestHeader(headerPair.Key, headerPair.Value);
                }
                else
                {
                    Debug.LogWarning("Null header: " + headerPair.Key + " = " + headerPair.Value);
                }
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                onResult?.Invoke(www.downloadHandler.data);
            }
        }
    }
}
