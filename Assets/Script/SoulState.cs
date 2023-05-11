using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handelInput에서 조건은 같은데 리턴값이 달라 발생하는 중복코드를 줄이기 위해서 정의한 모든 상태에 대한 enum값 정의
public enum State
{
    IDLE,
    WALK,
    JUMP,
    FALL,
    DASH,
    BASEATTACK,
    AIRATTACK,
    SKILL,
    NULL
}

abstract public class SoulState
{
    protected State innerState = State.NULL;
    virtual public void start(Soul soul, InputManager input) { }
    abstract public SoulState handleInput(Soul soul, InputManager input);
    abstract public SoulState stateChanger(Soul soul);   //handleInput에서 변경한 State값을 받아 상태머신의 상태를 변경함.
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
    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if (input.isJumpKeyDown)
        {
            innerState = State.JUMP;
        }
        else if (input.isDownJumpKeyDown || !soul.IsOnGround)
        {
            innerState = State.FALL;
        }
        else if (Mathf.Abs(input.moveDir) > 0)
        {
            innerState = State.WALK;
        }
        else if (soul.Data.isUseDash && soul.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            innerState = State.DASH;
        }
        else if (input.isAttackKeyDown)
        {
            innerState = State.BASEATTACK;
        }
        return stateChanger(soul);
    }
    abstract override public SoulState stateChanger(Soul soul);
}

abstract public class WalkState : SoulState
{
    override public void start(Soul soul, InputManager input)
    {
        Debug.Log("Walk");
        soul.Anime.Play("WALK");
    }

    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if (input.isJumpKeyDown)
        {
            innerState = State.JUMP;
        }
        else if (input.isDownJumpKeyDown || !soul.IsOnGround)
        {
            innerState = State.FALL;
        }
        else if (Mathf.Abs(input.moveDir) == 0)
        {
            innerState = State.IDLE;
        }
        else if (soul.Data.isUseDash && soul.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            innerState = State.DASH;
        }
        else if (input.isAttackKeyDown)
        {
            innerState = State.BASEATTACK;
        }
        return stateChanger(soul);
    }

    public abstract override SoulState stateChanger(Soul soul);

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
        soul.mTransform.position = Vector2.MoveTowards(soul.mTransform.position, soul.mTransform.position + new Vector3(input.moveDir * soul.Data.speed * Time.fixedDeltaTime, 0, 0), 0.8f);
    }
}

abstract public class JumpState : SoulState
{
    override public void start(Soul soul, InputManager input)
    {
        Debug.Log("Jump");
        soul.Anime.Play("JUMP");
        soul.IsOnGround = false;
        Jump(soul);
        input.isJumpKeyDown = false;
        soul.Rigid.gravityScale = soul.MoveData.generalGravityScale;
    }

    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if(soul.Rigid.velocity.y < 0)
        {
            innerState = State.FALL;
        }
        else if (input.isJumpKeyDown && soul.MoveData.jumpCount < soul.Data.availableJumpCount)
        {
            innerState = State.JUMP;
        }
        else if (soul.Data.isUseDash && soul.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            innerState = State.DASH;
        }
        else if (input.isAttackKeyDown)
        {
            innerState = State.BASEATTACK;
        }
        return stateChanger(soul);
    }
    public abstract override SoulState stateChanger(Soul soul);

    override public void fixedUpdate(Soul soul, InputManager input)
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
        soul.mTransform.position = Vector2.MoveTowards(soul.mTransform.position, soul.mTransform.position + new Vector3(input.moveDir * soul.Data.speed * Time.fixedDeltaTime, 0, 0), 0.8f);
    }

    override public void end(Soul soul, InputManager input) { }

    private void Jump(Soul soul)
    {
        soul.Rigid.velocity = new Vector2(soul.Rigid.velocity.x, 0.0f);
        soul.Rigid.AddForce(Vector2.up * soul.MoveData.jumpPower, ForceMode2D.Impulse);
        soul.MoveData.jumpCount++;
    }
}

abstract public class FallState : SoulState
{
    public override void start(Soul soul, InputManager input)
    {
        Debug.Log("Fall");
        soul.Anime.Play("FALL");
        soul.IsOnGround = false;
        soul.Rigid.gravityScale = soul.MoveData.fallGravityScale;
        if (input.isDownJumpKeyDown)
        {
            soul.MoveData.jumpCount++;
            input.isDownJumpKeyDown = false;
        }
    }
    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if (soul.Rigid.velocity.y == 0 && soul.IsOnGround)
        {
            if (Mathf.Abs(input.moveDir) > 0)
                innerState = State.WALK;
            else
                innerState = State.IDLE;
        }
        else if (input.isJumpKeyDown && soul.MoveData.jumpCount < soul.Data.availableJumpCount)
        {
            innerState = State.JUMP;
        }
        else if (input.isAttackKeyDown)
        {
            innerState = State.BASEATTACK;
        }
        return stateChanger(soul);
    }
    public abstract override SoulState stateChanger(Soul soul);

    public override void fixedUpdate(Soul soul, InputManager input)
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
        soul.mTransform.position = Vector2.MoveTowards(soul.mTransform.position, soul.mTransform.position + new Vector3(input.moveDir * soul.Data.speed * Time.fixedDeltaTime, 0, 0), 0.8f);
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
    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if (soul.MoveData.dashTime < dashTime)
        {
            if (soul.IsOnGround)
                innerState = State.IDLE;
            else
                innerState = State.FALL;
        }
        return stateChanger(soul);
    }
    public abstract override SoulState stateChanger(Soul soul);
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

abstract public class GroundBasicAttackState : SoulState
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

    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if (time >= attackDelay[soul.AttackCount])
        {
            if (soul.IsOnGround)
                innerState = State.IDLE;
            else
                innerState = State.FALL;
        }
        return stateChanger(soul);
    }

    public abstract override SoulState stateChanger(Soul soul);

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

abstract public class AirBasicAttackState : SoulState
{

}

abstract public class MeleeGroundBasicAttackState : GroundBasicAttackState
{

}

abstract public class RangedGroundBasicAttackState : GroundBasicAttackState
{

}

abstract public class MeleeAirBasicAttackState : AirBasicAttackState
{

}

abstract public class RangedAirBasicAttackState : AirBasicAttackState
{

}





