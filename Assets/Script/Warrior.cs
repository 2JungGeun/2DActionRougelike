using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Soul
{

    private float coolTime = 0.0f;
    public Warrior() { }

    public Warrior(string name) : base(name)
    {
        state = new WarriorIdleState();
    }

    public override void Start(InputManager input)
    {
        if (Mathf.Abs(input.moveDir) > 0)
            state = new WarriorWalkState();
        else
            state = new WarriorIdleState();
        state.start(this, input);
    }

    override public void Update(InputManager input)
    {
        moveData.lookAt = (sprite.flipX) ? -1 : 1;
        state.update(this, input);
        if (attackCount >= 1)
            combatAttackTerm -= Time.deltaTime;
        if (attackCount == 3)
            attackCount = 0;
        if (combatAttackTerm <= 0)
        {
            combatAttackTerm = 1.5f;
            attackCount = 0;
        }
        if (!cooldownTime.dashCoolingdown)
        {
            coolTime += Time.deltaTime;
            if (cooldownTime.dashCooldownTime <= coolTime)
            {
                coolTime = 0.0f;
                cooldownTime.dashCoolingdown = true;
            }
        }
    }

    override public void FixedUpdate(InputManager input)
    {
        IsGround(this);
        state.fixedUpdate(this, input);
    }

    public override void SwapingSoul(InputManager input)
    {
        this.state.end(this, input);
        this.state = new WarriorIdleState();
    }

    public override SoulState StateChanger(State innerState)
    {
        switch (innerState)
        {
            case State.IDLE:
                return new WarriorIdleState();
            case State.WALK:
                return new WarriorWalkState();
            case State.JUMP:
                return new WarriorJumpState();
            case State.FALL:
                return new WarriorFallState();
            case State.DASH:
                return new WarriorDashState();
            case State.BASEATTACK:
                return new WarriorGroundBasicAttackState();
            case State.SKILL:
                return null;
            default:
                return null;
        }
    }
}
