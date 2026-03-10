using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Controls")] 
    public float moveSpeed = 10f;
    public float rotateSpeed = 10f;

    public float interactRadius = 3f;
    
    private InputAction moveAction;
    private InputAction interactAction;
    private bool isWalking;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void FixedUpdate()
    {
        Vector3 moveValue = new Vector3(moveAction.ReadValue<Vector2>().x, 0, moveAction.ReadValue<Vector2>().y);
        
        isWalking = moveValue != Vector3.zero;
        transform.position += moveValue * moveSpeed * Time.fixedDeltaTime;
        
    }

    private void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            DoorInteract();
        }
    }

    private void DoorInteract()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRadius);
        
        DoorController closestDoor = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hitColliders)
        {
            DoorController door = hit.GetComponentInParent<DoorController>();

            if (door != null)
            {
                float distanceToDoor = Vector3.Distance(transform.position, door.transform.position);
                if (distanceToDoor < closestDistance)
                {
                    closestDistance = distanceToDoor;
                    closestDoor = door;
                }
            }
        }

        if (closestDoor != null)
        {
            closestDoor.OnPlayerInteract();
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsInteracting()
    {
        return interactAction.WasPressedThisFrame();
    }
    
}