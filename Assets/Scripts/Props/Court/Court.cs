using UnityEngine;

public class Court : MonoBehaviour
{
    public static Court instance;
    
    [SerializeField] private Transform gate1, gate2;

    public Transform Gate1
    {
        get => gate1;
    }
    
    public Transform Gate2
    {
        get => gate2;
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
