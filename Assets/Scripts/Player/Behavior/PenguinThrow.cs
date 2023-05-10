using System.IO;
using Photon.Pun;
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
    private PhotonView pv;
    
    public UnityEvent Throwed;
    
    // Start is called before the first frame update
    void Start()
    {
        grab = GetComponent<PenguinGrab>();
        pv = GetComponentInParent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine) Throw();
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
            pv.RPC("CallOnThrow", RpcTarget.AllBuffered);
        
            Rigidbody2D rb = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Gameplay", "Fish"), grab.Origin, quaternion.identity).GetComponent<Rigidbody2D>();
            
            Vector3 forceDirection = rb.gameObject.transform.position - transform.position;
            forceDirection.x = 0;
            
            rb.AddForceAtPosition(forceDirection.normalized * throwForce, transform.position, ForceMode2D.Impulse);
        }
    }
    
    private void Throw()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grab.grabbed)
        {
            if (!grab.holded)
            {
                grab.Drop();
                pv.RPC("CallOnThrow", RpcTarget.AllBuffered);

                Rigidbody2D rb = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Gameplay", "Fish"), grab.Origin, quaternion.identity).GetComponent<Rigidbody2D>();
            
                Vector3 forceDirection = rb.gameObject.transform.position - transform.position;
                forceDirection.y = 0;
            
                rb.AddForceAtPosition(forceDirection.normalized * throwForce, transform.position, ForceMode2D.Impulse);
            }
            else
            {
                if (count < 10)
                {
                    pv.RPC("IncrementCount", RpcTarget.AllBuffered);
                }
                else
                {
                    pv.RPC("CallOnEscape", RpcTarget.AllBuffered);
                }
            }
        }
    }
    
    [PunRPC]
    private void CallOnThrow()
    {
        Throwed.Invoke();
    }
    
    [PunRPC]
    private void CallOnEscape()
    {
        grab.OnEscape.Invoke();
    }

    [PunRPC]
    private void IncrementCount()
    {
        count++;
    }
}