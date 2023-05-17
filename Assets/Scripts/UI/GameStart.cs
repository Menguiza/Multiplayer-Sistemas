using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject message;
        
    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                button.gameObject.SetActive(true);
                message.gameObject.SetActive(false);
            }
            else
            { 
                button.gameObject.SetActive(false);
                message.gameObject.SetActive(true);
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount % 2 != 0 || PhotonNetwork.CurrentRoom.PlayerCount > PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }
}
