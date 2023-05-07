using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Play : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (!string.IsNullOrWhiteSpace(username.text) && PhotonNetwork.IsConnected)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
