using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] 
    public PlayerController player;
    public Animator animator;
    
    private const string IS_WALKING = "isWalking";
    private const string IS_INTERACTING = "isInteracting";
    



    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_INTERACTING, player.IsInteracting());

    }
}
