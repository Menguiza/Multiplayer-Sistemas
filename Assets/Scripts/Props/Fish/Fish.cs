using UnityEngine;

public class Fish : MonoBehaviour
{
    public void Grabbed()
    {
        Destroy(gameObject);
    }
}
