using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Soul
{

    private float coolTime = 0.0f;
    public Warrior() { }

    public Warrior(string name) : base(name)
    {
        sstate = new WarriorIdleState();
    }

    public override void Start(InputManager input)
    {
        if (Mathf.Abs(input.moveDir) > 0)
            sstate = new WarriorWalkState();
        else
            sstate = new WarriorIdleState();
        sstate.start(this, input);
    }

    override public void Update(InputManager input)
    {
        moveData.lookAt = (sprite.flipX) ? -1 : 1;
        sstate.update(this, input);
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
        sstate.fixedUpdate(this, input);
    }

    override public void HandleInput(InputManager input)
    {
        SoulState state = this.sstate.handleInput(this, input);
        if (state != null)
        {
            this.sstate.end(this, input);
            this.sstate = state;
            this.sstate.start(this, input);
        }
    }

    public override void SwapingSoul(InputManager input)
    {
        this.sstate.end(this, input);
        this.sstate = new WarriorIdleState();
    }
}
