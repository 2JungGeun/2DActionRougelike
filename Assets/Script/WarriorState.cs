using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorIdleState : IdleState
{
    public override void start(Soul soul, InputManager input)
    {
        Debug.Log("Warrior");
        base.start(soul, input);
    }
    public override SoulState handleInput(Soul soul, InputManager input)
    {
        if (input.isJumpKeyDown || input.isDownJumpKeyDown || !soul.IsOnGround)
        {
            return new WarriorJumpState();
        }

        else if (Mathf.Abs(input.moveDir) > 0)
        {
            return new WarriorWalkState();
        }

        else if (soul.Data.isUseDash && soul.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new WarriorDashState();
        }

        else if (input.isAttackKeyDown)
        {
            return new WarriorBasicAttackState();
        }
        return null;
    }
}


public class WarriorWalkState : WalkState
{
    public override SoulState handleInput(Soul soul, InputManager input)
    {
        if (input.isJumpKeyDown || input.isDownJumpKeyDown)
        {
            return new WarriorJumpState();
        }
        else if (Mathf.Abs(input.moveDir) == 0)
        {
            return new WarriorIdleState();
        }
        else if (soul.Data.isUseDash && soul.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new WarriorDashState();
        }
        else if (input.isAttackKeyDown)
        {
            return new WarriorBasicAttackState();
        }
        return null;
    }
}

public class WarriorJumpState : JumpState
{
    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if (soul.Rigid.velocity.y == 0 && soul.IsOnGround)
        {
            if (Mathf.Abs(input.moveDir) > 0)
                return new WarriorWalkState();
            else
                return new WarriorIdleState();
        }
        else if (soul.Data.isUseDash && soul.mCooldownTime.dashCoolingdown && input.isDashKeyDown)
        {
            return new WarriorDashState();
        }
        else if (input.isAttackKeyDown)
        {
            return new WarriorBasicAttackState();
        }
        return null;
    }
}

public class WarriorDashState : DashState
{
    override public SoulState handleInput(Soul soul, InputManager input)
    {
        if (soul.MoveData.dashTime < dashTime)
        {
            if (soul.IsOnGround)
                return new WarriorIdleState();
            else
                return new WarriorJumpState();
        }
        return null;
    }
}

public class WarriorBasicAttackState : BasicAttackState
{
    public override SoulState handleInput(Soul soul, InputManager input)
    {
        if (time >= attackDelay[soul.AttackCount])
        {
            if (soul.IsOnGround)
                return new WarriorIdleState();
            else
                return new WarriorJumpState();
        }
        return null;
    }
}


