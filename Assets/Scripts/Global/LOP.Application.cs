using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using GameFramework;
using System.Threading.Tasks;

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
            Initialize();
        }

        private static async void Initialize()
        {
#if UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = 60;
#endif
            MasterDataManager.Instantiate();

            if (Debug.isDebugBuild)
            {
                UnityEngine.Object.Instantiate(Resources.Load("IngameDebugConsole/IngameDebugConsole"));
                DebugCommandRegister.Instantiate();
            }

            UnityEngine.Application.quitting += OnQuitting;

            await GetPublicIP();

            IsInitialized = true;
        }

        private static void OnQuitting()
        {
            LOPWebAPI.LeaveLobby(new LeaveLobbyRequest { userId = LOP.Application.UserId });
        }

        private static async Task GetPublicIP()
        {
            using (var www = UnityWebRequest.Get("http://ipinfo.io/ip"))
            {
                await www.SendWebRequest();

                IP = Regex.Replace(www.downloadHandler.text, @"[^0-9.]", "");
            }
        }
    }
}
