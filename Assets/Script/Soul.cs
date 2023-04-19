using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SoulData
{
    public string name;
    public int hp;
    public int damage;
    public int range;
    public int availableJumpCount;
    public bool isUseDash;

    public SoulData() { }

    public SoulData(string name, string hp, string damage, string range, string isUseDash, string availableJumpCount)
    {
        this.name = name;
        Int32.TryParse(hp, out this.hp);
        Int32.TryParse(damage, out this.damage);
        Int32.TryParse(range, out this.range);
        this.isUseDash = Convert.ToBoolean(isUseDash);
        this.availableJumpCount = Convert.ToInt32(availableJumpCount);
    }

    public SoulData Deepcopy()
    {
        SoulData data = new SoulData();
        data.name = this.name;
        data.hp = this.hp;
        data.damage = this.damage;
        data.range = this.range;
        data.isUseDash = this.isUseDash;
        data.availableJumpCount = this.availableJumpCount;
        return data;
    }
}

public enum SoulState
{
    IDLE,
    WALK,
    JUMPING,
    FALLING,
    DASHING,
    ATTACKING
}

public enum AttackState
{
    ATTACKABLE,
    ATTACKIMPOSSIBLE
}

[Serializable]
public class SoulStates
{
    public SoulState soulState;
    public AttackState attackState;
    public float moveDir;
    public bool isOnGournd;
    public bool isUseDash;
    public int availableJumpCount;
    public float attackCount;
    public SoulStates(bool isUseDash, int availableJumpCount)
    {
        this.soulState = SoulState.IDLE;
        this.attackState = AttackState.ATTACKABLE;
        this.moveDir = 0.0f;
        this.isOnGournd = true;
        this.isUseDash = isUseDash;
        this.availableJumpCount = availableJumpCount;
        attackCount = 0.0f;
    }
}


[Serializable]
public class Soul {

    private SoulData data;
    public SoulData Data { get { return data; } }

    private Movement movement;
    public Movement Movement { get { return movement; } }
    private Attack attack;
    public Attack Attack { get { return attack; } }
    private Animation animation;
    public Animation Animation { get { return animation; } }

    //ป๓ลย
    private SoulStates states;
    public SoulStates States { get { return states; } set { states = value; } }

    public Soul() { }

    public Soul(string name)
    {
        this.data = DataManager.Instance().SoulDataDic[name].Deepcopy();
        string movementType = name + "Movement";
        string attackType = name + "Attack";
        string animetionType = name + "Animation";
        this.movement = (Movement)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(movementType);
        this.attack = (Attack)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(attackType);
        this.animation = (Animation)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(animetionType);
        this.states = new SoulStates(data.isUseDash, data.availableJumpCount);
    }

    public void Start(Rigidbody2D rigid, Transform transform, SpriteRenderer sprite, Animator anime)
    {
        movement.Start(rigid, transform);
        attack.Start(transform);
        animation.Start(sprite, anime);
    }

    public void Update()
    {
        animation.Update(states);
    }

    public void FixedUpdate(InputManager input)
    {
        Debug.Log(states.soulState);
        attack.Update(input, states);
        movement.Update(input, states);
    }
}
