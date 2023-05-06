using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masition : Soul
{
    private MasitionState state;
    private float coolTime = 0.0f;
    public Masition() { }

    public Masition(string name) : base(name)
    {
        state = new MasitionIdleState();
    }

    public override void Start(InputManager input)
    {
        if (Mathf.Abs(input.moveDir) > 0)
            state = new MasitionWalkState();
        else
            state = new MasitionIdleState();
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

    override public void HandleInput(InputManager input)
    {
        MasitionState state = this.state.handleInput(this, input);
        if (state != null)
        {
            this.state.end(this, input);
            this.state = state;
            this.state.start(this, input);
        }
    }

    public override void SwapingSoul(InputManager input)
    {
        this.state.end(this, input);
        this.state = new MasitionIdleState();
    }
}
