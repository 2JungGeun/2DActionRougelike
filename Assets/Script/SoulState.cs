using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SoulState
{
    virtual public void start(Soul soul, InputManager input) { }
    abstract public SoulState handleInput(Soul soul, InputManager input);
    virtual public void update(Soul soul, InputManager input) { }
    virtual public void fixedUpdate(Soul soul, InputManager input) { }
    virtual public void end(Soul soul, InputManager input) { }
}

abstract public class IdleState : SoulState
{
    override public void start(Soul soul, InputManager input)
    {
        Debug.Log("IDlE");
        soul.Anime.Play("IDLE");
    }
    abstract override public SoulState handleInput(Soul soul, InputManager input);
}

abstract public class WalkState : SoulState
{
    override public void start(Soul soul, InputManager input)
    {
        Debug.Log("Walk");
        soul.Anime.Play("WALK");
    }

    abstract override public SoulState handleInput(Soul soul, InputManager input);

    override public void update(Soul soul, InputManager input)
    {
        switch (input.moveDir)
        {
            case -1:
                soul.Sprite.flipX = true;
                break;
            case 1:
                soul.Sprite.flipX = false;
                break;
        }
    }
    public override void fixedUpdate(Soul soul, InputManager input)
    {
        soul.mTransform.position = Vector2.MoveTowards(soul.mTransform.position, soul.mTransform.position + new Vector3(input.moveDir * soul.MoveData.moveSpeed * Time.fixedDeltaTime, 0, 0), 0.8f);
    }
    override public void end(Soul soul, InputManager input) { }
}

abstract public class JumpState : SoulState
{
    override public void start(Soul soul, InputManager input)
    {
        Debug.Log("Jump");
        soul.Anime.Play("JUMP");
        soul.IsOnGround = false;
        if (input.isJumpKeyDown)
        {
            Jump(soul);
            input.isJumpKeyDown = false;
        }
        else if (input.isDownJumpKeyDown)
        {
            DownJump(soul);
            input.isDownJumpKeyDown = false;
        }
    }

    abstract override public SoulState handleInput(Soul soul, InputManager input);

    override public void fixedUpdate(Soul soul, InputManager input)
    {
        if (input.isJumpKeyDown)
        {
            input.isJumpKeyDown = false;
            Jump(soul);
        }
        switch (input.moveDir)
        {
            case -1:
                soul.Sprite.flipX = true;
                break;
            case 1:
                soul.Sprite.flipX = false;
                break;
        }
        soul.mTransform.position = Vector2.MoveTowards(soul.mTransform.position, soul.mTransform.position + new Vector3(input.moveDir * soul.MoveData.moveSpeed * Time.fixedDeltaTime, 0, 0), 0.8f);

        if (!soul.IsOnGround)
        {
            if (soul.Rigid.velocity.y < 0.0f)
            {
                soul.Rigid.gravityScale = soul.MoveData.fallGravityScale;
            }
            else
            {
                soul.Rigid.gravityScale = soul.MoveData.generalGravityScale;
            }
        }
    }

    override public void end(Soul soul, InputManager input) { }

    private void DownJump(Soul soul)
    {
        soul.MoveData.jumpCount++;
    }

    private void Jump(Soul soul)
    {
        if (soul.MoveData.jumpCount < soul.Data.availableJumpCount || soul.IsOnGround)
        {
            soul.Rigid.velocity = new Vector2(soul.Rigid.velocity.x, 0.0f);
            soul.Rigid.AddForce(Vector2.up * soul.MoveData.jumpPower, ForceMode2D.Impulse);
            soul.MoveData.jumpCount++;
        }
    }
}

abstract public class DashState : SoulState
{
    protected float dashTime;
    override public void start(Soul soul, InputManager input)
    {
        Debug.Log("DASH");
        dashTime = 0;
        soul.Anime.Play("DASH");
        soul.mCooldownTime.dashCoolingdown = false;
    }
    abstract override public SoulState handleInput(Soul soul, InputManager input);

    override public void update(Soul soul, InputManager input) { }
    override public void fixedUpdate(Soul soul, InputManager input)
    {
        dashTime += Time.fixedDeltaTime;
        soul.Rigid.velocity = new Vector2(soul.Rigid.velocity.x, 0.0f);
        soul.mTransform.position = Vector2.MoveTowards(soul.mTransform.position, soul.mTransform.position + new Vector3(soul.MoveData.lookAt * soul.MoveData.dashDistance * Time.fixedDeltaTime, 0), 1.0f);
    }
    override public void end(Soul soul, InputManager input)
    {
        input.isDashKeyDown = false;
    }
}

abstract public class BasicAttackState : SoulState
{
    protected float[] attackDelay = { 0.45f, 0.33f, 0.33f };
    protected float time;
    protected bool isAttack = false;
    override public void start(Soul soul, InputManager input)
    {
        Debug.Log("Attack" + soul.AttackCount);
        soul.attacking = true;
        soul.Anime.Play("ATTACK" + soul.AttackCount.ToString());
        time = 0.0f;
    }

    abstract override public SoulState handleInput(Soul soul, InputManager input);

    override public void update(Soul soul, InputManager input)
    {
        time += Time.deltaTime;
    }
    override public void fixedUpdate(Soul soul, InputManager input)
    {
        if (time >= (attackDelay[soul.AttackCount] * 0.5f) && !isAttack)
        {
            isAttack = createHitbox(soul, attackDelay[soul.AttackCount]);
        }
    }
    override public void end(Soul soul, InputManager input)
    {
        soul.attacking = false;
        soul.combatAttackTerm = 1.5f;
        soul.AttackCount++;
        input.isAttackKeyDown = false;
    }

    private bool createHitbox(Soul soul, float delay)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(soul.mTransform.position + new Vector3(soul.MoveData.lookAt * 1.0f, 0.75f, 0), new Vector2(1.8f, 1.4f), 0, Vector2.up, 0, 128);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                hit.collider.gameObject.SendMessage("Hit", null, SendMessageOptions.RequireReceiver);
            }
        }
        return true;
    }
}