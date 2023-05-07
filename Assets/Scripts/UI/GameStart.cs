using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Button button;

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                button.gameObject.SetActive(true);
            }
            else
            { 
                button.gameObject.SetActive(false);
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
