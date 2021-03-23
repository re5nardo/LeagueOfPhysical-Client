using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;
using System.IO.Compression;

public partial class LOPHttp
{
    private void Put(LOPHttpRequestContainer reqContainer)
    {
        StartCoroutine(PutRoutine(reqContainer));
    }
    
    private IEnumerator PutRoutine(LOPHttpRequestContainer reqContainer)
    {
        using (var www = UnityWebRequest.Put(reqContainer.FullUrl, reqContainer.Payload))
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            foreach (var headerPair in reqContainer.RequestHeaders)
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

            if (www.isNetworkError || www.isHttpError)
            {
                OnError(www.error, reqContainer);
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

                    OnResponse(response, reqContainer);
                }
                catch (Exception e)
                {
                    OnError("Unhandled error in PlayFabUnityHttp: " + e, reqContainer);
                }
            }
        }
    }

    private void Post(LOPHttpRequestContainer reqContainer)
    {
        StartCoroutine(PostRoutine(reqContainer));
    }

    private IEnumerator PostRoutine(LOPHttpRequestContainer reqContainer)
    {
        using (var www = new UnityWebRequest(reqContainer.FullUrl))
        {
            www.uploadHandler = new UploadHandlerRaw(reqContainer.Payload);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.method = "POST";

            foreach (var headerPair in reqContainer.RequestHeaders)
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

            if (www.isNetworkError || www.isHttpError)
            {
                OnError(www.error, reqContainer);
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

                    OnResponse(response, reqContainer);
                }
                catch (Exception e)
                {
                    OnError("Unhandled error in PlayFabUnityHttp: " + e, reqContainer);
                }
            }
        }
    }
}
