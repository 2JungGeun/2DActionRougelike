using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightIdleState : IdleState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class KnightWalkState : WalkState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class KnightJumpState : JumpState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class KnightFallState : FallState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class KnightDashState : DashState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class KnightGroundBasicAttackState : GroundBasicAttackState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}