using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField code;
    [SerializeField] private TMP_Text error;
    private List<RoomInfo> roomList;

    private void Awake()
    {
        roomList = new List<RoomInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        print("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Connected!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Joined!");
    }

    public void CreateRoom()
    {
        string roomName = GenerateUniqueRoomName();
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(code.text);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        error.text = $"Error({returnCode}): {message}";
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
        
        return roomName;
    }
}
