using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void PlayAgain()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
