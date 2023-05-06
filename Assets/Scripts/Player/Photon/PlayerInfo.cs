using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text userTag;
    [SerializeField] private LookAt court;

    public TMP_Text Tag {
        get => userTag;
        set => userTag = value;
    }

    public void SetCourt(Transform target)
    {
        court.SetTarget(target);
    }
}
