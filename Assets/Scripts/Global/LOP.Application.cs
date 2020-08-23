using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOP
{
    public class Application
    {
        private static bool isInitialized = false;
        public static bool IsInitialized { get { return isInitialized; } }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            Initialize();
        }

        private static void Initialize()
        {
            //  Target FrameRate
#if UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = 30;
#endif

            //  PhotonType Register
            PhotonTypeRegister.Register();

            //  Debug Console
            if (Debug.isDebugBuild)
            {
                UnityEngine.Object.Instantiate(Resources.Load("IngameDebugConsole/IngameDebugConsole"));
                DebugCommandRegister.Instantiate();
            }

            //            CatalogData.Instance.UpdateData();
            //            UserProfileData.Instance.UpdateData();

            //            yield return new WaitUntil(() => CatalogData.Instance.IsCached());
            //            yield return new WaitUntil(() => UserProfileData.Instance.IsCached());

            isInitialized = true;
        }
    }
}
