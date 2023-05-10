using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    [SerializeField] private TMP_Text timer, winResult, redPoints, bluePoints;
    [SerializeField] private GameObject winScreen;

    public string TimerValue
    {
        get => timer.text;
        set
        {
            timer.text = value;
        }
    }
    
    public string WinResult
    {
        set
        {
            winResult.text = value;
        }
    }
    
    public Vector2 Points
    {
        get => new Vector2(int.Parse(redPoints.text), int.Parse(bluePoints.text));
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

    public void SetActiveWin()
    {
        winScreen.SetActive(true);
    }
}
