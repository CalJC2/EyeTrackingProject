using System;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ObstaclePin
{
    [Tooltip("The fill percentage (0.0 to 1.0) where the bar hits this pin")]
    public float fillThreshold;
    
    [Tooltip("Drag the small visual pin that sits on the bar here")]
    public RectTransform pinGraphic;

    [HideInInspector] public int requiredNumber;
    [HideInInspector] public bool isPassed;
    
}
public class Phase2Bar : MonoBehaviour
{
    [Header("Bar Settings")] 
    public Image fillBar;
    public float fillSpeed = 0.2f;
    public float pinRaisedHeight = 50f;
    public float pinMoveSpeed = 300f;
    
    [Header("Obstacles")]
    public ObstaclePin[] obstacles = new ObstaclePin[5];

    private int currentGazedNumber = -1;
    private bool isRunning = false;
    private float[] originalPinHeights = new float[5];

    public void InitialisePhase2()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (obstacles[i].pinGraphic != null)
            {
                originalPinHeights[i] = obstacles[i].pinGraphic.anchoredPosition.y;
            }

            obstacles[i].requiredNumber = LockPickManager.Instance.lockPins[i].assignedNumber;
            obstacles[i].isPassed = false;
        }
        
        ResetBar();
        isRunning = true;
    }

    public void SetGazedNumber(int number)
    {
        currentGazedNumber = number;
    }

    public void ClearGazedNumber(int number)
    {
        if (currentGazedNumber == number)
        {
            currentGazedNumber = -1;
        }
    }

    private void Update()
    {
        if (!isRunning) return;
        
        fillBar.fillAmount += fillSpeed * Time.deltaTime;

        for (int i = 0; i < obstacles.Length; i++)
        {
            ObstaclePin pin = obstacles[i];

            float targetY = (currentGazedNumber == pin.requiredNumber || pin.isPassed)
                ? originalPinHeights[i] + pinRaisedHeight
                : originalPinHeights[i];

            if (pin.pinGraphic != null)
            {
                Vector2 currentPos = pin.pinGraphic.anchoredPosition;
                float newY = Mathf.MoveTowards(currentPos.y, targetY, pinMoveSpeed * Time.deltaTime);
                pin.pinGraphic.anchoredPosition = new Vector2(currentPos.x, newY);
            }

            if (!pin.isPassed && fillBar.fillAmount >= pin.fillThreshold)
            {
                if (currentGazedNumber == pin.requiredNumber)
                {
                    pin.isPassed = true;
                }
                else
                {
                    ResetBar();
                    break;
                }
            }
        }

        if (fillBar.fillAmount >= 1f)
        {
            isRunning = false;
            LockPickManager.Instance.CompletePhase2();
        }
    }

    private void ResetBar()
    {
        fillBar.fillAmount = 0f;
        foreach (var pin in obstacles)
        {
            pin.isPassed = false;
        }
    }
}
