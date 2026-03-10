using UnityEngine;

public class Phase3Rotator : MonoBehaviour, IGazeTarget
{
    [Header("UI References")] 
    public RectTransform keyPivot; // the center point the key rotates around
    public GameObject endTargetGraphic; // the visual target around the key
    public Camera camera;

    [Header("Settings")] 
    public float targetUnlockingAngle = -90f; // a quarter-turn to the right
    public float angleTolerance = 15f; // How close they 2 points need to be to trigger the win
    public float rotationOffset = -90f; // direction the key points 
    
    private bool isGrabbed = false;
    private bool isUnlocked = false;

    public void InitialisePhase3()
    {
        isGrabbed = false;
        isUnlocked = false;
        // hide the end target on init
        if(endTargetGraphic != null) endTargetGraphic.SetActive(false);
        
        if(keyPivot != null) keyPivot.rotation = Quaternion.Euler(0f, 0f, 0f);
        
    }
    
    public void LookAt()
    {
        if (isUnlocked) return;

        if (!isGrabbed)
        {
            isGrabbed = true;
            if(endTargetGraphic != null) endTargetGraphic.SetActive(true);
        }

    }

    public void LookAway()
    {
        
    }

    private void Update()
    {
        if (!isGrabbed || isUnlocked) return;
        
        Vector2 pivotScreenPos = RectTransformUtility.WorldToScreenPoint(camera, keyPivot.position);
        Vector2 gazePos = LockpickController.CurrentGazeScreenPosition;
        
        Vector2 direction = gazePos - pivotScreenPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        
        keyPivot.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
        
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetUnlockingAngle);
        float angleDifference = Quaternion.Angle(keyPivot.rotation, targetRotation);
        if (angleDifference < angleTolerance)
        {
            isUnlocked = true;
            LockPickManager.Instance.CompleteGame();
        }
            
    }
}
