using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorIdleState : IdleState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class WarriorWalkState : WalkState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class WarriorJumpState : JumpState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class WarriorFallState : FallState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class WarriorDashState : DashState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}

public class WarriorGroundBasicAttackState : GroundBasicAttackState
{
    public override SoulState stateChanger(Soul soul)
    {
        return soul.StateChanger(innerState);
    }
}


