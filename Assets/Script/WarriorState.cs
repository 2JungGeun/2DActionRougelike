using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorIdleState : IdleState { }

public class WarriorWalkState : WalkState { }

public class WarriorJumpState : JumpState { }

public class WarriorFallState : FallState { }

public class WarriorDashState : DashState { }

public class WarriorGroundBasicAttackState : RangedGroundBasicAttackState 
{
    public override void start(Soul soul, InputManager input)
    {
        base.start(soul, input);
        projectile = Resources.Load<GameObject>("Prefab/fireBall");
        Debug.Log(projectile.name);
    }
}

public class WarriorAirBasicAttackState : RangedAirBasicAttackState
{
    public override void start(Soul soul, InputManager input)
    {
        base.start(soul, input);
        projectile = Resources.Load<GameObject>("Prefab/fireBall");
        Debug.Log(projectile.name);
    }
}


