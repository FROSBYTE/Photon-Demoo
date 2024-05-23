using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInputField;
    [SerializeField] TMP_InputField joinInputField;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Joinned Server Succesfully");
        UIManager.Instance.connectingPanel.SetActive(false);
        UIManager.Instance.createjoinPanel.SetActive(true);
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public void CreateLobby()
    {
        PhotonNetwork.CreateRoom(createInputField.text);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }
}
