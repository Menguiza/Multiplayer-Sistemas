using System.Collections.Generic;
using UnityEngine;

public class FishPos : MonoBehaviour
{
    public static FishPos instance;

    [SerializeField] private List<Transform> fishList = new List<Transform>();

    public List<Transform> FishList
    {
        get => fishList;
    }
    
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
