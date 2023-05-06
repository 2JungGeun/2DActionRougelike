using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Knight : Soul
{
    private KnightState state;
    private float coolTime = 0.0f;
    public Knight() { }

    public Knight(string name) : base(name)
    {
        state = new KnightIdleState();
    }

    public override void Start(InputManager input)
    {
        if (Mathf.Abs(input.moveDir) > 0)
            state = new KnightWalkState();
        else
            state = new KnightIdleState();
        state.start(this, input);
    }

    override public void Update(InputManager input)
    {
        moveData.lookAt = (sprite.flipX) ? -1 : 1;
        state.update(this, input);
        if (!attacking)
        {
            if (attackCount >= 1)
                combatAttackTerm -= Time.deltaTime;
            if (combatAttackTerm <= 0)
            {
                combatAttackTerm = 1.5f;
                attackCount = 0;
            }
        }
        if (attackCount == 3)
            attackCount = 0;
        //dash
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

    override public void HandleInput(InputManager input)
    {
        KnightState state = this.state.handleInput(this, input);
        if (state != null)
        {
            this.state.end(this, input);
            this.state = state;
            this.state.start(this, input);
        }
    }

    public override void SwapingSoul(InputManager input)
    {
        state.end(this, input);
        this.state = new KnightIdleState();
    }
}