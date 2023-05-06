using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KnightState
{
    virtual public void start(Knight knight, InputManager input) { }
    virtual public KnightState handleInput(Knight knight, InputManager input) { return null; }
    virtual public void update(Knight knight, InputManager input) { }
    virtual public void fixedUpdate(Knight knight, InputManager input) { }
    virtual public void end(Knight knight, InputManager input) { }
}

public class KnightIdleState : KnightState
{
    override public void start(Knight knight, InputManager input)
    {
        Debug.Log("IDlE");
        knight.Anime.Play("IDLE");
    }
    override public KnightState handleInput(Knight knight, InputManager input)
    {
        if (input.isJumpKeyDown || input.isDownJumpKeyDown || !knight.IsOnGround)
        {
            return new KnightJumpState();
        }

        else if (Mathf.Abs(input.moveDir) > 0)
        {
            return new KnightWalkState();
        }

        else if (knight.Data.isUseDash && knight.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new KnightDashState();
        }

        else if (input.isAttackKeyDown)
        {
            return new KnightBasicAttackState();
        }
        return null;
    }
}

public class KnightWalkState : KnightState
{
    override public void start(Knight knight, InputManager input)
    {
        Debug.Log("Walk");
        knight.Anime.Play("WALK");
    }
    override public KnightState handleInput(Knight knight, InputManager input)
    {
        if (input.isJumpKeyDown || input.isDownJumpKeyDown)
        {
            return new KnightJumpState();
        }
        else if (Mathf.Abs(input.moveDir) == 0)
        {
            return new KnightIdleState();
        }
        else if (knight.Data.isUseDash && knight.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new KnightDashState();
        }
        else if (input.isAttackKeyDown)
        {
            return new KnightBasicAttackState();
        }
        return null;
    }
    override public void update(Knight knight, InputManager input)
    {
        switch (input.moveDir)
        {
            case -1:
                knight.Sprite.flipX = true;
                break;
            case 1:
                knight.Sprite.flipX = false;
                break;
        }
    }

    public override void fixedUpdate(Knight knight, InputManager input)
    {
        knight.mTransform.position = Vector2.MoveTowards(knight.mTransform.position, knight.mTransform.position + new Vector3(input.moveDir * knight.MoveData.moveSpeed * Time.fixedDeltaTime, 0, 0), 0.8f);
    }
    override public void end(Knight knight, InputManager input) { }
}

public class KnightJumpState : KnightState
{
    override public void start(Knight knight, InputManager input)
    {
        Debug.Log("Jump");
        knight.Anime.Play("JUMP");
        knight.IsOnGround = false;
        if (input.isJumpKeyDown)
        {
            Jump(knight);
            input.isJumpKeyDown = false;
        }
        else if (input.isDownJumpKeyDown)
        {
            DownJump(knight);
            input.isDownJumpKeyDown = false;
        }
    }
    override public KnightState handleInput(Knight knight, InputManager input)
    {
        if (knight.Rigid.velocity.y == 0 && knight.IsOnGround)
        {
            if (Mathf.Abs(input.moveDir) > 0)
                return new KnightWalkState();
            else
                return new KnightIdleState();
        }
        else if (knight.Data.isUseDash && knight.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new KnightDashState();
        }
        else if (input.isAttackKeyDown)
        {
            return new KnightBasicAttackState();
        }
        return null;
    }

    override public void update(Knight knight, InputManager input)
    {

    }

    override public void fixedUpdate(Knight knight, InputManager input)
    {
        if (input.isJumpKeyDown)
        {
            input.isJumpKeyDown = false;
            Jump(knight);
        }
        switch (input.moveDir)
        {
            case -1:
                knight.Sprite.flipX = true;
                break;
            case 1:
                knight.Sprite.flipX = false;
                break;
        }
        knight.mTransform.position = Vector2.MoveTowards(knight.mTransform.position, knight.mTransform.position + new Vector3(input.moveDir * knight.MoveData.moveSpeed * Time.fixedDeltaTime, 0, 0), 0.8f);

        if (!knight.IsOnGround)
        {
            if (knight.Rigid.velocity.y < 0.0f)
            {
                knight.Rigid.gravityScale = knight.MoveData.fallGravityScale;
            }
            else
            {
                knight.Rigid.gravityScale = knight.MoveData.generalGravityScale;
            }
        }
    }

    override public void end(Knight knight, InputManager input) { }

    private void DownJump(Knight knight)
    {
        knight.MoveData.jumpCount++;
    }

    private void Jump(Knight knight)
    {
        if (knight.MoveData.jumpCount < knight.Data.availableJumpCount || knight.IsOnGround)
        {
            knight.Rigid.velocity = new Vector2(knight.Rigid.velocity.x, 0.0f);
            knight.Rigid.AddForce(Vector2.up * knight.MoveData.jumpPower, ForceMode2D.Impulse);
            knight.MoveData.jumpCount++;
        }
    }
}

public class KnightDashState : KnightState
{
    float dashTime;
    override public void start(Knight knight, InputManager input)
    {
        Debug.Log("DASH");
        dashTime = 0;
        knight.Anime.Play("DASH");
        knight.mCooldownTime.dashCoolingdown = false;
    }
    override public KnightState handleInput(Knight knight, InputManager input) 
    {
        if (knight.MoveData.dashTime < dashTime)
        {
            if (knight.IsOnGround)
                return new KnightIdleState();
            else
                return new KnightJumpState();
        }
        return null; 
    }
    override public void update(Knight knight, InputManager input){ }
    override public void fixedUpdate(Knight knight, InputManager input)
    {
        dashTime += Time.fixedDeltaTime;
        knight.Rigid.velocity = new Vector2(knight.Rigid.velocity.x, 0.0f);
        knight.mTransform.position = Vector2.MoveTowards(knight.mTransform.position, knight.mTransform.position + new Vector3(knight.MoveData.lookAt * knight.MoveData.dashDistance * Time.fixedDeltaTime, 0), 1.0f);
    }
    override public void end(Knight knight, InputManager input)
    {
        input.isDashKeyDown = false;
    }
}

public class KnightBasicAttackState : KnightState
{
    private float[] attackDelay = { 0.45f, 0.33f, 0.33f};
    private float time;
    private bool isAttack = false;
    override public void start(Knight knight, InputManager input)
    {
        Debug.Log("Attack" + knight.AttackCount);
        knight.attacking = true;
        knight.Anime.Play("ATTACK" + knight.AttackCount.ToString());
        time = 0.0f;
    }
    override public KnightState handleInput(Knight knight, InputManager input)
    {
        if (time >= attackDelay[knight.AttackCount])
        {
            if (knight.IsOnGround)
                return new KnightIdleState();
            else
                return new KnightJumpState();
        }
        return null; 
    }
    override public void update(Knight knight, InputManager input)
    {
        time += Time.deltaTime;
    }
    override public void fixedUpdate(Knight knight, InputManager input) 
    {
        if (time >= (attackDelay[knight.AttackCount] * 0.5f) && !isAttack)
        {
            isAttack = createHitbox(knight, attackDelay[knight.AttackCount]);
        }
    }
    override public void end(Knight knight, InputManager input)
    {
        knight.attacking = false;
        knight.combatAttackTerm = 1.5f;
        knight.AttackCount++;
        input.isAttackKeyDown = false;
    }

    private bool createHitbox(Knight knight, float delay)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(knight.mTransform.position + new Vector3(knight.MoveData.lookAt * 1.0f, 0.75f, 0), new Vector2(1.8f, 1.4f), 0, Vector2.up, 0, 128);
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

public class FirstSkillState : KnightState
{

}

public class SecondSkillState : KnightState
{

}
