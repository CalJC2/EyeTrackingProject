using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PinInteractable : MonoBehaviour, IGazeTarget
{
    [Header("Movement and Positioning")]
    [SerializeField] private float moveSpeed; // how fast the Pin moves
    [SerializeField] private float targetDistance; // how far the Pin needs to travel
   
    private PinCheckManager _pinCheckManager;
    private RectTransform _rectTransform;
    private Vector2 _startPos; // where the Pin currently is
    private Vector2 _targetPos; // where the Pin needs to get to ( will change this later for to randomise pin placement)
    private bool isBeingLookedAt = false; // true it the player is looking at the specific Pin
    public bool isAtTargetPosition = false; // Once the Pin is in the correct place, it won't move from there
    private Button _button;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _pinCheckManager = FindObjectOfType<PinCheckManager>();
        
        _startPos = _rectTransform.anchoredPosition;
        // calculates the distance from the starting position to the target position and sets it
        _targetPos = _startPos + new Vector2(0,targetDistance);
    }

    private void Update()
    {
        // if the Pin has clicked in place, it can no longer be moved
        if (isAtTargetPosition) return;
    
        if (isBeingLookedAt)
        {
            // if the player looks at the pin it will move towards the target position
            _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _targetPos, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(_rectTransform.anchoredPosition, _targetPos) < 0.01f)
            {
                // if the pin as reached the correct place, with a little leeway, it will snap and click into place
                SnapAndClick();
            }
        }
        else
        {
            // if it's not being looked at it will slowly return to its starting position
            _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _startPos, moveSpeed * Time.deltaTime);
        }
    
        
        isBeingLookedAt = false;
    }

    // called byt the lockpick controller
    public void LookAt()
    {
        isBeingLookedAt = true;
        //highlight the Pin when looked at
        _button.Select();
    }
    // called by the lockpick controller
    public void LookAway()
    {
        isBeingLookedAt = false;
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    // when the pin reaches its target position
    private void SnapAndClick()
    {
        isAtTargetPosition = true;
        _rectTransform.anchoredPosition = _targetPos;
        _pinCheckManager.PhaseUpdate();
        //play audio and visuals in here


    }
}
