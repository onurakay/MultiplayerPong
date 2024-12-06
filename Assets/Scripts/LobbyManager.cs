using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI statusText;

    void Start()
    {
        InitializeLobby();
    }

    private void InitializeLobby()
    {
        PhotonNetwork.ConnectUsingSettings();
        statusText.text = "Connecting to servers...";
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void SetPlayerName()
    {
        if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
        {
            PhotonNetwork.NickName = playerNameInput.text;
        }
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        }
    }

    public void OnCreateRoomButtonClicked()
    {
        SetPlayerName();
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        SetPlayerName();
        PhotonNetwork.JoinRandomRoom();
    }

        private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "Connected to servers. Ready to create or join a room.";
    }


    public override void OnJoinedRoom()
    {
        statusText.text = $"Joined Room: {PhotonNetwork.CurrentRoom.Name} (ID: {PhotonNetwork.CurrentRoom.Name})";
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            statusText.text += "\nWaiting for another player...";
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            statusText.text = $"Both players are ready!\nRoom ID: {PhotonNetwork.CurrentRoom.Name}";
            StartGame();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        statusText.text = "No rooms available, creating a new room.";
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }
}
