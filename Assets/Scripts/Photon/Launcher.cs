using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    private List<RoomInfo> roomList;
    private bool firstTime = true;

    private void Awake()
    {
        roomList = new List<RoomInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); 
        Debug.Log("Connecting...");
    }

    #region Actions

    public void SetName()
    {
        PhotonNetwork.NickName = UIManager.instace.Username.ToUpper();
    }

    public void Disconect()
    {
        PhotonNetwork.Disconnect();
    }
    
    public void CreateRoom()
    {
        string roomName = GenerateUniqueRoomName();
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(roomName, options);
        UIManager.instace.DisableCreateButton();
        UIManager.instace.SetError("Creating...");
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrWhiteSpace(UIManager.instace.Code))
        {
            UIManager.instace.SetError("Joining!");
            UIManager.instace.DisableJoinButton();
            PhotonNetwork.JoinRoom(UIManager.instace.Code);
        }
        else UIManager.instace.SetError("Error: Code Required");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        UIManager.instace.OpenMenu(1);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #endregion

    #region Events

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!");

        if (firstTime)
        {
            firstTime = false;
            UIManager.instace.OpenMenu("Main");
        }
        
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause.ToString());
        UIManager.instace.OpenMenu("Main");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        UIManager.instace.OpenMenu(2);
        UIManager.instace.SetRoomCode(PhotonNetwork.CurrentRoom.Name);

        foreach (Player element in PhotonNetwork.PlayerList)
        {
            UIManager.instace.PlayerJoin(element);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo element in roomList)
        {
            if(!this.roomList.Contains(element)) this.roomList.Add(element);
            else
            {
                if (element.RemovedFromList)
                {
                    this.roomList.Remove(element);
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UIManager.instace.PlayerJoin(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UIManager.instace.PlayerLeave(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        UIManager.instace.ClearContent();
    }

    #endregion

    #region Errors

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        UIManager.instace.EnableCreateButton();
        UIManager.instace.SetError($"Error({returnCode}): {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        UIManager.instace.EnableJoinButton();
        UIManager.instace.SetError($"Error({returnCode}): {message}");
    }

    #endregion

    #region Utils

    private string GenerateUniqueRoomName()
    {
        string roomName = "";
        for (int i = 0; i < 4; i++)
        {
            char c = (char)Random.Range(65, 91); // A-Z in ASCII
            roomName += c.ToString();
        }

        foreach (RoomInfo element in roomList)
        {
            if (element.Name == roomName) return GenerateUniqueRoomName();
        }
        
        return roomName.ToUpper();
    }

    #endregion
}
