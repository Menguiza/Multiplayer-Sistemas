using UnityEngine;
using UnityEngine.UI;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Image img;

    private Transform target;
    private RectTransform rectTransform;
    private SpriteRenderer targetRend;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetRend = target.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (target != null)
        {
            GateVisible();
            LookAtTarget();
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void GateVisible()
    {
        if (targetRend.isVisible)
        {
            img.enabled = false;
        }
        else
        {
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), targetRend.bounds))
            {
                img.enabled = true;
            }
            else
            {
                img.enabled = true;
            }
        }
    }

    private void LookAtTarget()
    {
        if (img.enabled)
        {
            Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            rectTransform.localEulerAngles = new Vector3(0,0,angle);
        }
    }
}
