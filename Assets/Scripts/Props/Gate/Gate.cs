using UnityEngine;
using TMPro;

public class Gate : MonoBehaviour
{
    [SerializeField] private TMP_Text points;

    private void Update()
    {
        points.text = transform.childCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            other.transform.parent = transform;
        }
    }
}
