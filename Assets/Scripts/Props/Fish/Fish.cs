using UnityEngine;
using Photon.Pun;

public class Fish : MonoBehaviour
{
    [SerializeField] private PhotonView pv;
    
    public void Grabbed()
    {
        pv.RPC("DestroyFish", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void DestroyFish()
    {
        if(pv.Owner == PhotonNetwork.LocalPlayer) PhotonNetwork.Destroy(pv);
        else gameObject.SetActive(false);
    }
}
