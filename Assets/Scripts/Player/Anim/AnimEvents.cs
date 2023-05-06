using UnityEngine;
using UnityEngine.Events;

public class AnimEvents : MonoBehaviour
{
    public UnityEvent Carrying;

    public void CarryInvoke()
    {
        Carrying.Invoke();
    }
}
