using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Phase1PinInteractable : MonoBehaviour, IGazeTarget
{
    [Header("Pin Settings")] 
    public float moveSpeed = 15f;
    public float StartingYPosition = 0f; // Set in editor to where the pins will always start on the Y axis
    
    [Header("UI References")]
    public RectTransform pinGraphic;
    public TextMeshProUGUI numberText;

    private int pinIndex;
    private bool isBeingLookedAt = false;
    private bool pinIsSet = false;
    private float targetYPosition;

    public void SetUpPin(int RandomPinIndex)
    {
        pinIndex = RandomPinIndex;
        isBeingLookedAt = false;
        pinIsSet = false;
        moveSpeed = Random.Range(30f, 70f);
        if (numberText != null) numberText.gameObject.SetActive(false);
        
        
        if( pinGraphic != null) pinGraphic.anchoredPosition = new Vector2(pinGraphic.anchoredPosition.x, StartingYPosition);
    }

    private void Update()
    {
        if (pinIsSet) return;

        if (isBeingLookedAt)
        {
            // gets the lock pick managers target height for the pin
            targetYPosition = LockPickManager.Instance.lockPins[pinIndex].targetHeight;

            // move the pin towards the target position 
            Vector2 currentpos = pinGraphic.anchoredPosition;
            float newY = Mathf.MoveTowards(currentpos.y, targetYPosition, moveSpeed * Time.deltaTime);
            pinGraphic.anchoredPosition = new Vector2(currentpos.x, newY);
            
            //if in position set it
            if (pinGraphic.anchoredPosition.y == targetYPosition)
            {
                LockInPin();
            }
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
        pinIsSet = true;
        // lock the pin in place
        pinGraphic.anchoredPosition = new Vector2(pinGraphic.anchoredPosition.x, targetYPosition);

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
        isBeingLookedAt = true;
    }

    public void LookAway()
    {
        isBeingLookedAt = false;
    }
}
