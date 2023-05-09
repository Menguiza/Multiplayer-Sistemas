using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PenguinMovement))]
public class PenguinGrab : MonoBehaviour
{
    public LayerMask fishLayer, playerLayer;
    [SerializeField] private Transform grab_R, grab_L;
    [SerializeField] private float radius = 1f, grabCD = 0.1f, holdCountdown = 2f;
    [SerializeField] private GameObject alert;

    private PenguinMovement movement;
    private Vector3 origin;
    private bool slide;
    private PhotonView pv;

    public bool grabbed { get; private set; }
    public bool holded { get; private set; }
    public bool CanHold { get; private set; }
    public Vector3 Origin { get => origin; }

    public UnityEvent OnHold, OnRelease, OnEscape;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PenguinMovement>();
        pv = GetComponentInParent<PhotonView>();
        origin = grab_R.position;
        CanHold = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            SetOrigin();
            GrabFish();

            if (Input.GetKeyDown(KeyCode.L))
            {
                CanHold = false;
                //call.Hold();
                Invoke("Escape", holdCountdown);
                OnHold.Invoke();
            }
        
            movement.Anim.SetBool("Slide", slide);
        }
    }

    public void Drop()
    {
        slide = false;
        Invoke("GrabCD", grabCD);
    }

    public void Escape()
    {
        holded = false;
        CanHold = true;
        movement.EnableMovement();
        alert.SetActive(false);
    }

    private void GrabCD()
    {
        grabbed = false;
    }

    private void Hold()
    {
        holded = true;
        CanHold = false;
        OnHold.Invoke();
        alert.SetActive(true);
        Invoke("HoldCD", holdCountdown);
    }

    private void HoldCD()
    {
        OnRelease.Invoke();
        holded = false;
        CanHold = true;
        alert.SetActive(false);
    }

    private void SetOrigin()
    {
        if (movement.Rend.flipX)
        {
            origin = grab_L.position;
        }
        else
        {
            origin = grab_R.position;
        }
    }
    
    private void GrabFish()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !grabbed && CanHold)
        {
            Collider2D collider = Physics2D.OverlapCircle(origin, radius, playerLayer);
            
            if (collider != null)
            {
                if (collider.TryGetComponent(out PenguinGrab call))
                {
                    if (call.grabbed && call.CanHold)
                    {
                        CanHold = false;
                        call.Hold();
                        Invoke("Escape", holdCountdown);
                        OnHold.Invoke();
                        return;
                    }
                }
            }
            
            collider = Physics2D.OverlapCircle(origin, radius, fishLayer);

            if (collider != null)
            {
                if (collider.TryGetComponent(out Fish call))
                {
                    grabbed = true;
                    slide = true;
                    call.Grabbed();
                }
            }
        }
    }

    #if UNITY_EDITOR
    
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, radius);
    }
    
    #endif
}
