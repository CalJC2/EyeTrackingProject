using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Screen = UnityEngine.Device.Screen;

public class Phase1PinInteractable : MonoBehaviour, IGazeTarget
{
    [Header("Pin Settings")] 
    public float moveSpeed = 15f;
    public float StartingYPosition = 0f; // Set in editor to where the pins will always start on the Y axis
    public float StartingXPosition = 0f; 
    public float targetRangeTolerance = 25f;
    public float shakeIntensity = 3f;
    
    [Header("UI References")]
    public RectTransform pinGraphic;
    public TextMeshProUGUI numberText;

    private int pinIndex;
    private PinData pinData;
    private bool isBeingLookedAt = false;
    private bool IsSet = false;
    private float targetYPosition;
    private float YLimit;

    public void SetUpPin(int RandomPinIndex)
    {
        pinIndex = RandomPinIndex;
        pinData = LockPickManager.Instance.GetPinData(pinIndex);
        isBeingLookedAt = false;
        IsSet = false;
        moveSpeed = Random.Range(50f, 70f);
        StartingXPosition = pinGraphic.anchoredPosition.x;
        // finding screen height
        YLimit = StartingYPosition + 400f;
        // gets the lock pick managers target height for the pin
        targetYPosition = LockPickManager.Instance.lockPins[pinIndex].targetHeight;
        if (numberText != null) numberText.gameObject.SetActive(false);
        
        if( pinGraphic != null) pinGraphic.anchoredPosition = new Vector2(StartingXPosition, StartingYPosition);
    }

    private void Update()
    {
        if (IsSet) return;

        if (isBeingLookedAt)
        {
            Vector2 currentpos = pinGraphic.anchoredPosition;
            float newY = Mathf.MoveTowards(currentpos.y, YLimit, moveSpeed * Time.deltaTime);
            
            // check the distance the pin is from the target height 
            float DistanceToTarget = Mathf.Abs(targetYPosition - newY);

            // move the pin towards the target position 

            if (DistanceToTarget <= targetRangeTolerance)
            {
                float randomXShake = Random.Range(-shakeIntensity, shakeIntensity);
                //float randomYShake = Random.Range(-shakeIntensity, shakeIntensity);
                pinGraphic.anchoredPosition = new Vector2(StartingXPosition + randomXShake, newY);
            }
            else
            {
                pinGraphic.anchoredPosition = new Vector2(StartingXPosition, newY);
            }
            
            //if in position set it
            //if (pinGraphic.anchoredPosition.y == targetYPosition)
            //{
            //    LockInPin();
            //}
        }
        else
        {
            // if looked away, move back to its starting position
            Vector2 currentpos = pinGraphic.anchoredPosition;
            float newY = Mathf.MoveTowards(currentpos.y, StartingYPosition, moveSpeed * Time.deltaTime);
            pinGraphic.anchoredPosition = new Vector2(currentpos.x, newY);
        }
    }

    private void LockInPin()
    {
        IsSet = true;
        // lock the pin in place
        pinGraphic.anchoredPosition = new Vector2(pinGraphic.anchoredPosition.x, pinData.targetHeight);

        int displayNumber = LockPickManager.Instance.lockPins[pinIndex].assignedNumber;
        
        Debug.Log(displayNumber);

        // get the number for the lock pick manager and set it to be visible
        if (numberText != null)
        {
            numberText.text = displayNumber.ToString();
            numberText.gameObject.SetActive(true);   
        }

        
        // Play audio click here
        
        LockPickManager.Instance.ReportPinSet(pinIndex);
    }

    public void LookAt()
    {
       if (!IsSet) isBeingLookedAt = true;
    }

    public void LookAway()
    {
        if (IsSet) return;
        
        isBeingLookedAt = false;

        float currnetY = pinGraphic.anchoredPosition.y;
        
        float DistanceToTarget = Mathf.Abs(targetYPosition - currnetY);

        if (DistanceToTarget <= targetRangeTolerance)
        {
            LockInPin();
        }
    }

    public int GetAssignedNumber()
    {
        return pinData.assignedNumber;
    }
}
