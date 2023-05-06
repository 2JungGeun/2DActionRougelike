using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SoulData
{
    public int id;
    public string name;
    public int hp;
    public int damage;
    public int range;
    public int availableJumpCount;
    public bool isUseDash;

    public SoulData() { }

    public SoulData(string id, string name, string hp, string damage, string range, string isUseDash, string availableJumpCount)
    {
        Int32.TryParse(id, out this.id);
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
        data.id = this.id;
        data.name = this.name;
        data.hp = this.hp;
        data.damage = this.damage;
        data.range = this.range;
        data.isUseDash = this.isUseDash;
        data.availableJumpCount = this.availableJumpCount;
        return data;
    }
}

public class MovementData
{
    //walk
    public float moveSpeed;

    //jump
    public float jumpHeight;
    public float jumpPower;
    public int jumpCount;

    //dash
    public float dashDistance;
    public float dashTime;
    public float lookAt;

    //중력 변수
    public float fallGravityScale;
    public float generalGravityScale;

    public MovementData()
    {
        this.moveSpeed = 5.0f;
        this.jumpHeight = 4.0f;
        this.fallGravityScale = 6.0f;
        this.generalGravityScale = 3.0f;
        this.jumpPower = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * generalGravityScale));
        this.jumpCount = 0;
        this.dashDistance = 20.0f;
        this.dashTime = 0.25f;
    }
}

public class CooldownTime
{
    public float dashCooldownTime;
    public bool dashCoolingdown;
    public float skill1CooldownTime;
    public float skill2CooldownTime;

    public CooldownTime()
    {
        this.dashCooldownTime = 5.0f;
        this.dashCoolingdown = true;
        this.skill1CooldownTime = 6.0f;
        this.skill2CooldownTime = 6.0f;
    }

    public void chageDash()
    {
        this.dashCoolingdown = true;
    }
}

public abstract class Soul {

    protected Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } set { rigid = value; } }
    protected Transform transform;
    public Transform mTransform { get { return transform; } set { transform = value; } }
    protected SpriteRenderer sprite;
    public SpriteRenderer Sprite { get { return sprite; } set { sprite = value; } }
    protected Animator animetor;
    public Animator Anime { get { return animetor; } set { animetor = value; } }
    
    protected SoulData data;
    public SoulData Data { get { return data; } }

    protected MovementData moveData = new MovementData();
    public MovementData MoveData { get { return moveData; } set { moveData = value; } }

    protected SoulState sstate;

    protected CooldownTime cooldownTime = new CooldownTime();
    public CooldownTime mCooldownTime { get { return cooldownTime; } set { cooldownTime = value; } }

    protected bool isOnGround;
    public bool IsOnGround { get { return isOnGround; } set { isOnGround = value; } }

    protected int attackCount;
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }
    public float combatAttackTerm = 1.5f;
    public bool attacking = false;
    public Soul() { }

    public Soul(string name)
    {
        this.data = DataManager.Instance().SoulDataDic[name].Deepcopy();
        Debug.Log("실행");
    }

    public void Initialize(Rigidbody2D rigid, Transform transform, SpriteRenderer sprite, Animator anime)
    {
        this.rigid = rigid;
        this.transform = transform;
        this.sprite = sprite;
        this.animetor = anime;
        isOnGround = false;
    }

    public abstract void Start(InputManager input);

    public abstract void Update(InputManager input);

    public abstract void FixedUpdate(InputManager input);

    public abstract void HandleInput(InputManager input);

    public abstract void SwapingSoul(InputManager input);

    protected void IsGround(Soul soul)
    {
        RaycastHit2D hit1 = Physics2D.Raycast(soul.transform.position + new Vector3(-0.5f, 0.0f, 0.0f), Vector2.down, 0.1f, 64);
        RaycastHit2D hit2 = Physics2D.Raycast(soul.transform.position + new Vector3(0.5f, 0.0f, 0.0f), Vector2.down, 0.1f, 64);
        if (hit1.collider != null || hit2.collider != null)
        {
            if (soul.rigid.velocity.y == 0)
            {
                isOnGround = true;
                soul.MoveData.jumpCount = 0;
            }
        }
        else
        {
            isOnGround = false;
        }
    }
}
