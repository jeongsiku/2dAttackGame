using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharState
{
    Idle,
    Attack,
    Dead
}

public class CharacterState : MonoBehaviour
{
    Animator animator;
    public CharState charState = CharState.Idle;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void PlayAnimator(bool state)
    {
        if(state)
            animator.speed = GameData.gameSpeed;
        else
            animator.speed = 0;
    }

    public void SetAnimatorSpeed()
    {
        animator.speed = GameData.gameSpeed;
    }
}
