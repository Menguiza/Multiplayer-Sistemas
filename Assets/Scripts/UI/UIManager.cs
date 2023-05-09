using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instace;

    [SerializeField] private TMP_Text error, roomCode;
    [SerializeField] private TMP_InputField code, username;
    [SerializeField] private Button createRoom, joinRoom;
    [SerializeField] private PlayerListInfo prefab;
    [SerializeField] private RectTransform content;

    public List<Menu> menus = new List<Menu>();

    public string Code { get=> code.text.ToUpper(); }
    public string Username { get=> username.text; }

    private List<PlayerListInfo> currentPlayers = new List<PlayerListInfo>();

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Methods

    public void SetError(string error)
    {
        this.error.text = error;
    }
    
    public void SetRoomCode(string code)
    {
        this.roomCode.text = code;
    }

    public void DisableCreateButton()
    {
        createRoom.interactable = false;
    }

    public void EnableCreateButton()
    {
        createRoom.interactable = true;
    }
    
    public void DisableJoinButton()
    {
        joinRoom.interactable = false;
    }

    public void EnableJoinButton()
    {
        joinRoom.interactable = true;
    }

    public void ClearContent()
    {
        foreach (Transform element in content)
        {
            Destroy(element.gameObject);
        }
    }
    
    public void PlayerJoin(Player player)
    {
        PlayerListInfo info = Instantiate(prefab, content);

        info.SetUsername(player.NickName);
        info.SetID(player.UserId);
        
        currentPlayers.Add(info);
    }

    public void PlayerLeave(Player player)
    {
        foreach (PlayerListInfo element in currentPlayers)
        {
            if (element.GetPlayerID == player.UserId)
            {
                currentPlayers.Remove(element);
                element.DestroyItem();
                return;
            }
        }
    }
    
    public void OpenMenu(int index)
    {
        StartCoroutine(OpenMenuCoroutine(index));
    }

    public void OpenMenu(string menuName)
    {
        StartCoroutine(OpenMenuCoroutine(menuName));
    }

    public void CloseAllMenus()
    {
        code.text = "";
        error.text = "";
        roomCode.text = "";
        username.text = "";

        EnableCreateButton();
        EnableJoinButton();
        
        foreach (Menu element in menus)
        {
            element.gameObject.SetActive(false);
        }
    }

        #endregion

    #region Coroutines

    IEnumerator OpenMenuCoroutine(int index)
    {
        if (menus.Count > index)
        {
            CloseAllMenus();

            if(!PhotonNetwork.IsConnectedAndReady) yield return new WaitUntil(()=>PhotonNetwork.IsConnectedAndReady);
            
            yield return new WaitForSeconds(0.5f);
            
            menus.ElementAt(index).gameObject.SetActive(true);
        }
    }
    
    IEnumerator OpenMenuCoroutine(string menuName)
    {
        foreach (Menu element in menus)
        {
            if (element.MenuName == menuName)
            {
                CloseAllMenus();
                
                if(!PhotonNetwork.IsConnectedAndReady) yield return new WaitUntil(()=>PhotonNetwork.IsConnectedAndReady);

                yield return new WaitForSeconds(0.5f);
                
                element.gameObject.SetActive(true);
                
                yield return null;
            }
        }
    }

    #endregion
}
