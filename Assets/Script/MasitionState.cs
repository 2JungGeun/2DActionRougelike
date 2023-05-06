using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MasitionState
{
    virtual public void start(Masition masition, InputManager input) { }
    virtual public MasitionState handleInput(Masition masition, InputManager input) { return null; }
    virtual public void update(Masition masition, InputManager input) { }
    virtual public void fixedUpdate(Masition masition, InputManager input) { }
    virtual public void end(Masition masition, InputManager input) { }
}

public class MasitionIdleState : MasitionState
{
    override public void start(Masition Masition, InputManager input)
    {
        Debug.Log("IDlE");
        Masition.Anime.Play("IDLE");
    }
    override public MasitionState handleInput(Masition Masition, InputManager input)
    {
        if (input.isJumpKeyDown || input.isDownJumpKeyDown || !Masition.IsOnGround)
        {
            return new MasitionJumpState();
        }

        else if (Mathf.Abs(input.moveDir) > 0)
        {
            return new MasitionWalkState();
        }

        else if (Masition.Data.isUseDash && Masition.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new MasitionDashState();
        }

        else if (input.isAttackKeyDown)
        {
            return new MasitionBasicAttackState();
        }
        return null;
    }
}

public class MasitionWalkState : MasitionState
{
    override public void start(Masition Masition, InputManager input)
    {
        Debug.Log("Walk");
        Masition.Anime.Play("WALK");
    }
    override public MasitionState handleInput(Masition Masition, InputManager input)
    {
        if (input.isJumpKeyDown || input.isDownJumpKeyDown)
        {
            return new MasitionJumpState();
        }
        else if (Mathf.Abs(input.moveDir) == 0)
        {
            return new MasitionIdleState();
        }
        else if (Masition.Data.isUseDash && Masition.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new MasitionDashState();
        }
        else if (input.isAttackKeyDown)
        {
            return new MasitionBasicAttackState();
        }
        return null;
    }
    override public void update(Masition Masition, InputManager input)
    {
        switch (input.moveDir)
        {
            case -1:
                Masition.Sprite.flipX = true;
                break;
            case 1:
                Masition.Sprite.flipX = false;
                break;
        }
    }

    public override void fixedUpdate(Masition Masition, InputManager input)
    {
        Masition.mTransform.position = Vector2.MoveTowards(Masition.mTransform.position, Masition.mTransform.position + new Vector3(input.moveDir * Masition.MoveData.moveSpeed * Time.fixedDeltaTime, 0, 0), 0.8f);
    }
    override public void end(Masition Masition, InputManager input) { }
}

public class MasitionJumpState : MasitionState
{
    override public void start(Masition Masition, InputManager input)
    {
        Debug.Log("Jump");
        Masition.Anime.Play("JUMP");
        Masition.IsOnGround = false;
        if (input.isJumpKeyDown)
        {
            Jump(Masition);
            input.isJumpKeyDown = false;
        }
        else if (input.isDownJumpKeyDown)
        {
            DownJump(Masition);
            input.isDownJumpKeyDown = false;
        }
    }
    override public MasitionState handleInput(Masition Masition, InputManager input)
    {
        if (Masition.Rigid.velocity.y == 0 && Masition.IsOnGround)
        {
            if (Mathf.Abs(input.moveDir) > 0)
                return new MasitionWalkState();
            else
                return new MasitionIdleState();
        }
        else if (Masition.Data.isUseDash && Masition.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new MasitionDashState();
        }
        else if (input.isAttackKeyDown)
        {
            return new MasitionBasicAttackState();
        }
        return null;
    }

    override public void update(Masition Masition, InputManager input)
    {

    }

    override public void fixedUpdate(Masition Masition, InputManager input)
    {
        if (input.isJumpKeyDown)
        {
            input.isJumpKeyDown = false;
            Jump(Masition);
        }
        switch (input.moveDir)
        {
            case -1:
                Masition.Sprite.flipX = true;
                break;
            case 1:
                Masition.Sprite.flipX = false;
                break;
        }
        Masition.mTransform.position = Vector2.MoveTowards(Masition.mTransform.position, Masition.mTransform.position + new Vector3(input.moveDir * Masition.MoveData.moveSpeed * Time.fixedDeltaTime, 0, 0), 0.8f);

        if (!Masition.IsOnGround)
        {
            if (Masition.Rigid.velocity.y < 0.0f)
            {
                Masition.Rigid.gravityScale = Masition.MoveData.fallGravityScale;
            }
            else
            {
                Masition.Rigid.gravityScale = Masition.MoveData.generalGravityScale;
            }
        }
    }

    override public void end(Masition Masition, InputManager input) { }

    private void DownJump(Masition Masition)
    {
        Masition.MoveData.jumpCount++;
    }

    private void Jump(Masition Masition)
    {
        if (Masition.MoveData.jumpCount < Masition.Data.availableJumpCount || Masition.IsOnGround)
        {
            Masition.Rigid.velocity = new Vector2(Masition.Rigid.velocity.x, 0.0f);
            Masition.Rigid.AddForce(Vector2.up * Masition.MoveData.jumpPower, ForceMode2D.Impulse);
            Masition.MoveData.jumpCount++;
        }
    }
}

public class MasitionDashState : MasitionState
{
    float dashTime;
    override public void start(Masition Masition, InputManager input)
    {
        Debug.Log("DASH");
        dashTime = 0;
        Masition.Anime.Play("DASH");
        Masition.mCooldownTime.dashCoolingdown = false;
    }
    override public MasitionState handleInput(Masition Masition, InputManager input)
    {
        if (Masition.MoveData.dashTime < dashTime)
        {
            if (Masition.IsOnGround)
                return new MasitionIdleState();
            else
                return new MasitionJumpState();
        }
        return null;
    }
    override public void update(Masition Masition, InputManager input) { }
    override public void fixedUpdate(Masition Masition, InputManager input)
    {
        dashTime += Time.fixedDeltaTime;
        Masition.Rigid.velocity = new Vector2(Masition.Rigid.velocity.x, 0.0f);
        Masition.mTransform.position = Vector2.MoveTowards(Masition.mTransform.position, Masition.mTransform.position + new Vector3(Masition.MoveData.lookAt * Masition.MoveData.dashDistance * Time.fixedDeltaTime, 0), 1.0f);
    }
    override public void end(Masition Masition, InputManager input)
    {
        input.isDashKeyDown = false;
    }
}

public class MasitionBasicAttackState : MasitionState
{
    private float[] attackDelay = { 0.4f, 0.4f, 0.7f };
    private float time;
    override public void start(Masition Masition, InputManager input)
    {
        Debug.Log("Attack" + Masition.AttackCount);
        Masition.Anime.Play("ATTACK" + Masition.AttackCount.ToString());
        Masition.AttackCount++;
        time = 0.0f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(Masition.mTransform.position + new Vector3(Masition.MoveData.lookAt * 1.0f, 0.75f, 0), new Vector2(1.8f, 1.4f), 0, Vector2.up, 0, 128);
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                hit.collider.gameObject.SendMessage("Hit", null, SendMessageOptions.RequireReceiver);
            }
        }
    }
    override public MasitionState handleInput(Masition Masition, InputManager input)
    {
        if (time >= attackDelay[Masition.AttackCount])
        {
            if (Masition.IsOnGround)
                return new MasitionIdleState();
            else
                return new MasitionJumpState();
        }
        return null;
    }
    override public void update(Masition Masition, InputManager input)
    {
        time += Time.deltaTime;
    }
    override public void fixedUpdate(Masition Masition, InputManager input)
    {

    }
    override public void end(Masition Masition, InputManager input)
    {
        Masition.combatAttackTerm = 1.5f;
        input.isAttackKeyDown = false;
    }
}

public class MasitionFirstSkillState : MasitionState
{

}

public class MasitionSecondSkillState : MasitionState
{

}

