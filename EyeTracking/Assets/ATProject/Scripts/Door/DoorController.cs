using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")] 
    public bool isLocked = true;
    public float openAngle = 90f;
    public float swingSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRoation;
    private Quaternion openRoation;

    private void Start()
    {
        closedRoation = transform.rotation;
        openRoation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    private void Update()
    {
        Quaternion targetRotation = isOpen ? openRoation : closedRoation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * swingSpeed);
    }
    
    public void OnPlayerInteract()
    {
        if (isLocked)
        {
            LockPickManager.Instance.StartMiniGame(this);
        }
        else
        {
            isOpen = !isOpen;
        }
    }
    
    public void UnlockAndOpenDoor()
    {
        isLocked = false;
        isOpen = true;
    }
    
}
