using TMPro;
using UnityEngine;

public class PlayerListInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text username;
    private string playerID;
    
    public string GetPlayerID { get => playerID; }

    public void SetUsername(string username)
    {
        this.username.text = username;
    }

    public void SetID(string id)
    {
        if (playerID == "") playerID = id;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
