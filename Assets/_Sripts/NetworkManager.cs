using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    [SerializeField] GameObject connectingPanel;
    [SerializeField] GameObject createjoinPanel;
    [SerializeField] GameObject waitingRoomPanel;

    [SerializeField] TMP_InputField createInputField;
    [SerializeField] TMP_InputField joinInputField;
    [SerializeField] TMP_InputField playerNameInputField;

    [SerializeField] TextMeshProUGUI waitingRoomDebugText;

    [SerializeField] Transform playerListParent;

    [SerializeField] GameObject playerNamePrefab;

    [Header("Photon Values and References")]
    //[SerializeField] int minPlayersToStart = 2;
    [SerializeField] List<GameObject> playerNameObjects = new List<GameObject>();


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Joinned Server Succesfully");
        connectingPanel.SetActive(false);
        createjoinPanel.SetActive(true);
    }

    public void JoinLobby()
    {
        SetPlayerNickname();
        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public void CreateLobby()
    {
        SetPlayerNickname();
        PhotonNetwork.CreateRoom(createInputField.text);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void UpdatePlayerList()
    {
        // Clear existing player name objects
        foreach (GameObject obj in playerNameObjects)
        {
            Destroy(obj);
        }
        playerNameObjects.Clear();

        // Instantiate new player name objects
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerNameObj = Instantiate(playerNamePrefab, playerListParent);
            playerNameObj.GetComponent<TextMeshProUGUI>().text = player.NickName;
            playerNameObjects.Add(playerNameObj);
        }
    }

    void SetPlayerNickname()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            PhotonNetwork.NickName = playerNameInputField.text;
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(1000, 9999); // Set a default nickname if none is provided
        }
    }

    public void StartSession()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2)
        {
            PhotonNetwork.LoadLevel(1);
        }
        else
        {
            waitingRoomDebugText.text = "You need 2 players to start ";
            StartCoroutine(ShowDebugTextCoroutine(waitingRoomDebugText));
        }
    }

    #region Override Functions

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room Successfully");
        UpdatePlayerList();
        waitingRoomPanel.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player joined the room. Updating player list...");
        UpdatePlayerList();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the room successfully");
        waitingRoomPanel.SetActive(false);
        createjoinPanel.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left the room. Updating player list...");
        UpdatePlayerList();
    }

    #endregion

    private IEnumerator ShowDebugTextCoroutine(TextMeshProUGUI _text)
    {
        yield return new WaitForSeconds(2);
        _text.text = "";
    }
}
