using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text userTag;
    [SerializeField] private LookAt court;
    [SerializeField] private PhotonView pv;
    [SerializeField] private List<GameObject> playerObjects;

    public TMP_Text Tag {
        get => userTag;
        set => userTag = value;
    }

    private void Start()
    {
        SetTag();
        SetCourt();
        if(pv.Owner == PhotonNetwork.LocalPlayer) ActivateObjects();
    }

    #region Methods

    private void SetCourt()
    {
        court.SetTarget((int)pv.Owner.CustomProperties["team"] == 0 ? Court.instance.Gate1 : Court.instance.Gate2);
    }

    private void SetTag()
    {
        Tag.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
    }

    private void ActivateObjects()
    {
        foreach (GameObject element in playerObjects)
        {
            element.SetActive(true);
        }
    }

    #endregion
}
