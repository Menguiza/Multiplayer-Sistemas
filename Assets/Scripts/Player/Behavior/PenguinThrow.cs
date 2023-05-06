using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PenguinGrab))]
public class PenguinThrow : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private float throwForce = 10;

    private PenguinGrab grab;
    private int count;
    
    public UnityEvent Throwed;
    
    // Start is called before the first frame update
    void Start()
    {
        grab = GetComponent<PenguinGrab>();
    }

    // Update is called once per frame
    void Update()
    {
        Throw();
    }

    public void ResetCount()
    {
        count = 0;
    }

    public void SnatchOut()
    {
        if (grab.holded)
        {
            grab.Drop();
            Throwed.Invoke();
        
            Rigidbody2D rb = Instantiate(fishPrefab, grab.Origin, quaternion.identity).GetComponent<Rigidbody2D>();
            
            Vector3 forceDirection = rb.gameObject.transform.position - transform.position;
            forceDirection.x = 0;
            
            rb.AddForceAtPosition(forceDirection.normalized * throwForce, transform.position, ForceMode2D.Impulse);
        }
    }
    
    private void Throw()
    {
        if (Input.GetKeyDown(KeyCode.Return) && grab.grabbed)
        {
            if (!grab.holded)
            {
                grab.Drop();
                Throwed.Invoke();

                Rigidbody2D rb = Instantiate(fishPrefab, grab.Origin, quaternion.identity).GetComponent<Rigidbody2D>();
            
                Vector3 forceDirection = rb.gameObject.transform.position - transform.position;
                forceDirection.y = 0;
            
                rb.AddForceAtPosition(forceDirection.normalized * throwForce, transform.position, ForceMode2D.Impulse);
            }
            else
            {
                if (count < 10)
                {
                    count++;
                }
                else
                {
                    grab.OnEscape.Invoke();
                }
            }
        }
    }
}
