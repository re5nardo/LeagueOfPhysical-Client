using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using System.Collections;
using UnityEngine.UI;

public class Entrance : PunBehaviour
{
    [SerializeField] private GameObject goCI;
    [SerializeField] private GameObject goIntro;
    [SerializeField] private LoginComponent loginComponent;
	[SerializeField] private Text textState;
    [SerializeField] private GameObject goBtnJoinLobby;

    #region MonoBehaviour
    private IEnumerator Start()
    {
        //yield return StartCoroutine(PlayCI());

        //yield return StartCoroutine(PlayIntro());

        goBtnJoinLobby.SetActive(false);
        textState.gameObject.SetActive(true);

        textState.text = "Login중입니다.";

        loginComponent.successCallback = () =>
        {
            Debug.Log("Congratulations, you made your first successful API call!");

            ConnectToMasterServer();
        };

        loginComponent.errorCallback = (error) =>
        {
            Debug.LogWarning("Something went wrong with your first API call.  :(");
            Debug.LogError("Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        };

        loginComponent.StartLogin();

        yield break;
    }
    #endregion  

    private IEnumerator PlayCI()
    {
        goCI.SetActive(true);

        yield return new WaitForSeconds(2f);

        goCI.SetActive(false);
    }

    private IEnumerator PlayIntro()
    {
        goIntro.SetActive(true);

        yield return new WaitForSeconds(2f);

        goIntro.SetActive(false);
    }

    private IEnumerator WaitForInitialize()
    {
        textState.text = "Application 초기화중입니다.";

        yield return new WaitUntil(() => LOP.Application.IsInitialized);

        CheckMatchState();
    }

    private void CheckMatchState()
    {
        if (IsInvoking("CheckMatchState"))
        {
            CancelInvoke("CheckMatchState");
        }

        MatchStateManager.Instance.GetMatchState(
            result =>
            {
                switch((string)result["state"])
                {
                    case "Matching":
                        MatchStateManager.Instance.CancelMatchRequest();
                        Invoke("CheckMatchState", 1f);
                        break;
                    case "Matched":
                        RoomConnector.TryToEnterRoom((string)result["gameId"]);
                        break;
                    default:
                        textState.gameObject.SetActive(false);
                        goBtnJoinLobby.SetActive(true);
                        break;
                }
            },
            error =>
            {
                Debug.LogError(error);
            }
        );
    }

    private void ConnectToMasterServer()
    {
        textState.text = "마스터 서버에 접속중입니다.";

		PhotonNetwork.ConnectUsingSettings("v1.0");
    }

	private void ConnectToLobby()
    {
        textState.text = "로비에 접속중입니다.";

		PhotonNetwork.JoinLobby();
    }

    #region MonoBehaviourPunCallbacks
	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        textState.text = string.Format("마스터 서버 접속에 실패했습니다, cause : {0}", cause.ToString());
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
		Debug.LogError(string.Format("[Photon OnConnectionFail] cause : {0}", cause.ToString()));
    }

	public override void OnConnectedToMaster ()
	{
        textState.text = "마스터 서버에 접속하였습니다.";

        Debug.Log("PhotonNetwork.player.UserId : " + PhotonNetwork.player.UserId);

        foreach (TypedLobbyInfo lobbyInfo in PhotonNetwork.LobbyStatistics)
		{
			Debug.Log(string.Format("[Lobby Info] Type : {0}, IsDefault : {1}, Name : {2}, RoomCount : {3}, PlayerCount : {4}", lobbyInfo.Type, lobbyInfo.IsDefault, lobbyInfo.Name, lobbyInfo.RoomCount, lobbyInfo.PlayerCount));
		}

        StartCoroutine(WaitForInitialize());
    }

	public override void OnJoinedLobby ()
	{
        textState.text = "로비에 접속하였습니다.";

		SceneManager.LoadScene("Lobby");
	}
    #endregion

    #region Event Handler
    public void OnJoinLobbyBtnClicked()
    {
        ConnectToLobby();
    }
    #endregion
}