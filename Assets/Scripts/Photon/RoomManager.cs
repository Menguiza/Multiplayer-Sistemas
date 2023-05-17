using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    private Vector3[] redPos = {new Vector3(-5,0,0), new Vector3(-5,1.5f,0), new Vector3(-5,-1.5f,0), new Vector3(-5,3,0), new Vector3(-5,-3,0)};
    private Vector3[] bluePos = {new Vector3(5,0,0), new Vector3(5,1.5f,0), new Vector3(5,-1.5f,0), new Vector3(5,3,0), new Vector3(5,-3,0)};

    private int redIndex, blueIndex;
    private bool allPlayersLoaded, called;
    private float timeRemaining = 150f;

    public UnityEvent GameOver, GameStarted;

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

    public void Update()
    {
        if (allPlayersLoaded && !called)
        {
            timeRemaining -= Time.deltaTime; // Subtract the elapsed time since the last frame from the time remaining
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                called = true;
                photonView.RPC("GameEnded",RpcTarget.AllBuffered);
            }
            else
            {
                Timer.instance.TimerValue = FormatTime(timeRemaining);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("team") && targetPlayer == PhotonNetwork.LocalPlayer)
        {
            int playerTeam = (int)targetPlayer.CustomProperties["team"];

            if(playerTeam == 0)
            {
                PenguinMovement movement = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Gameplay", "RedPlayer"),
                    redPos[(int)targetPlayer.CustomProperties["index"]],
                    quaternion.identity).GetComponentInChildren<PenguinMovement>();
                GameStarted.AddListener(movement.EnableMovement);
                GameOver.AddListener(movement.DisableMovment);
                movement.DisableMovment();
            }
            else
            {
                PenguinMovement movement = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Gameplay", "BluePlayer"),
                    bluePos[(int)targetPlayer.CustomProperties["index"]],
                    quaternion.identity).GetComponentInChildren<PenguinMovement>();
                GameStarted.AddListener(movement.EnableMovement);
                GameOver.AddListener(movement.DisableMovment);
                movement.DisableMovment();
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
                StartCoroutine(WaitForPlayers());
            }
            else
            {
                PlayerLoadedScene();
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
            playerProps.Add("LoadedScene", false);

            switch (teamNumber)
            {
                case 0:
                    playerProps.Add("index", redIndex);
                    redIndex++;
                    break;
                case 1:
                    playerProps.Add("index", blueIndex);
                    blueIndex++;
                    break;
            }

            PhotonNetwork.PlayerList[i].SetCustomProperties(playerProps);
            i++;
        }
    }

    private void SetFishes()
    {
        foreach (Transform element in FishPos.instance.FishList)
        {
            PhotonNetwork.InstantiateRoomObject(Path.Combine("Prefab", "Gameplay", "Fish"), element.position,
                element.rotation);
        }
    }
    
    private void PlayerLoadedScene()
    {
        Hashtable playerProps = PhotonNetwork.LocalPlayer.CustomProperties;
        
        playerProps.Add("LoadedScene", true);

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
    }

    [PunRPC]
    private void PlayersLoaded()
    {
        allPlayersLoaded = true;
        Timer.instance.DeactivateWait();
        GameStarted.Invoke();
    }

    [PunRPC]
    private void GameEnded()
    {
        GameOver.Invoke();
        CheckResult();
    }

    private void CheckResult()
    {
        if (Timer.instance.Points.x > Timer.instance.Points.y)
        {
            Timer.instance.WinResult = "Red Team Wins!";
        }
        else if (Timer.instance.Points.x < Timer.instance.Points.y)
        {
            Timer.instance.WinResult = "Blue Team Wins!";
        }
        else
        {
            Timer.instance.WinResult = "It's a Draw!";
        }
        
        Timer.instance.SetActiveWin();
    }
    
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    private IEnumerator WaitForPlayers()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before checking

        Player[] otherPlayers = PhotonNetwork.PlayerListOthers;

        foreach (Player player in otherPlayers)
        {
            if (!(bool)player.CustomProperties["LoadedScene"])
            {
                yield return new WaitForSeconds(2f); // Wait for 2 more seconds before checking again
                StartCoroutine(WaitForPlayers());
                yield break;
            }
        }

        allPlayersLoaded = true;
        SetFishes();
        photonView.RPC("PlayersLoaded", RpcTarget.AllBuffered);
    }
}
