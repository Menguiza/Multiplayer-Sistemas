using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    private Vector3[] redPos = {new Vector3(-5,0,0), new Vector3(-5,1.5f,0), new Vector3(-5,-1.5f,0), new Vector3(-5,3,0), new Vector3(-5,-3,0)};
    private Vector3[] bluePos = {new Vector3(5,0,0), new Vector3(5,1.5f,0), new Vector3(5,-1.5f,0), new Vector3(5,3,0), new Vector3(5,-3,0)};

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("team") && targetPlayer == PhotonNetwork.LocalPlayer)
        {
            int playerTeam = (int)targetPlayer.CustomProperties["team"];
            int index = playerTeam == 0
                ? (int)PhotonNetwork.CurrentRoom.CustomProperties["redIndex"]
                : (int)PhotonNetwork.CurrentRoom.CustomProperties["blueIndex"];
            
            if(playerTeam == 0) 
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefab", "Gameplay", "RedPlayer"), redPos[index],
                quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefab", "Gameplay", "BluePlayer"), bluePos[index],
                    quaternion.identity);
            }
            
            if(playerTeam == 0) 
            {
                Hashtable props = new Hashtable() { { "redIndex", index + 1 } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
            else
            {
                Hashtable props = new Hashtable() { { "blueIndex", index + 1 } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SetTeams();
            }
        }
    }

    private void SetTeams()
    {
        List<Player> players = PhotonNetwork.PlayerList.OrderBy(x=> Guid.NewGuid()).ToList();
        int i = 0;
        
        foreach (Player element in players)
        {
            Hashtable playerProps = element.CustomProperties;
            int teamNumber = i % 2;
            playerProps.Add("team", teamNumber);

            PhotonNetwork.PlayerList[i].SetCustomProperties(playerProps);
            i++;
        }
        
        Hashtable initialProps = new Hashtable() {
            { "redIndex", 0 },
            { "blueIndex", 0 }
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(initialProps);
    }
}
