using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

namespace LOP
{
    public class Application
    {
        public static bool IsApplicationQuitting;
        public static bool IsInitialized { get; private set; }

        public static string IP { get; private set; }
        public static string UserId => SystemInfo.deviceUniqueIdentifier;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            GlobalMonoBehavior.StartCoroutine(Initialize());
        }

        private static IEnumerator Initialize()
        {
            //  Target FrameRate
#if UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = 60;
#endif
            MasterDataManager.Instantiate();

            //  Debug Console
            if (Debug.isDebugBuild)
            {
                UnityEngine.Object.Instantiate(Resources.Load("IngameDebugConsole/IngameDebugConsole"));
                DebugCommandRegister.Instantiate();
            }

            UnityEngine.Application.quitting += OnQuitting;

            yield return GlobalMonoBehavior.StartCoroutine(GetPublicIP());

            IsInitialized = true;
        }

        private static void OnQuitting()
        {
            LOPWebAPI.LeaveLobby(new LeaveLobbyRequest { userId = LOP.Application.UserId });
        }

        private static IEnumerator GetPublicIP()
        {
            using (var www = UnityWebRequest.Get("http://ipinfo.io/ip"))
            {
                yield return www.SendWebRequest();

                IP = Regex.Replace(www.downloadHandler.text, @"[^0-9.]", "");
            }
        }
    }
}
