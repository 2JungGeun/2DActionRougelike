using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation
{
    protected Animator animator;
    public Animator Anime { get { return animator; }}
    protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRen { get { return spriteRenderer; }}
    public void Start(SpriteRenderer sprite, Animator anime)
    {
        this.animator = anime;
        this.spriteRenderer = sprite;
    }

    virtual public void Update(SoulStates states)
    {
        FlipSprite(states);
        MoveAnimation(states);
    }

    private void MoveAnimation(SoulStates states)
    {
        switch (states.soulState)
        {
            case SoulState.IDLE:
            case SoulState.WALK:
                WalkAnime(states);
                break;
            case SoulState.JUMPING:
                JumpAnime(states);
                break;
            case SoulState.FALLING:
                JumpAnime(states);
                break;
            case SoulState.DASHING:
                DashAnime(states);
                break;
            case SoulState.ATTACKING:
                AttackAnime(states);
                break;
        }
    }

    private void FlipSprite(SoulStates states)
    {
        if (states.soulState == SoulState.DASHING)
            return;
        if (states.moveDir > 0)
            spriteRenderer.flipX = false;
        else if (states.moveDir < 0)
            spriteRenderer.flipX = true;

    }

    private void AttackAnime(SoulStates states)
    {

        animator.SetFloat("AttackCount", states.attackCount);
        animator.SetTrigger("isBaseAttack");
    }
    private void WalkAnime(SoulStates states)
    {
        animator.SetFloat("speed", Mathf.Abs(states.moveDir));
    }

    private void DashAnime(SoulStates states)
    {
        animator.SetTrigger("isDash");
    }

    private void JumpAnime(SoulStates states)
    {
        if (!states.isOnGournd)
        {
            animator.SetBool("isJumping", true);
            animator.SetTrigger("isJump");
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }
}
