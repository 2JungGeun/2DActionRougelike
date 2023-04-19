using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    //component
    Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }
    Transform transform;
    public Transform mTransform { get { return transform; } }

    //이동관련 변수
    //move
    private readonly float moveSpeed;
    private float lookAt = 1.0f;
    //jump
    private float jumpHeight;
    private float jumpPower;
    private int jumpCount;
    private bool isPassingPlatform;

    //dash
    private float dashDistance;
    private float dashTime = 0.0f;
    //중력 변수
    private float fallGravityScale;
    private float generalGravityScale;
    private float time = 0.0f;
    public Movement()
    {
        moveSpeed = 7.0f;
        dashDistance = 20.0f;
        jumpHeight = 4.0f;
        fallGravityScale = 6.0f;
        generalGravityScale = 3.0f;
        jumpCount = 0;
        jumpPower = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * generalGravityScale));
        isPassingPlatform = false;
    }


    public void Start(Rigidbody2D rigid, Transform transform)
    {
        this.rigid = rigid;
        this.transform = transform;
    }

    public virtual void Update(InputManager input, SoulStates states)
    {
        Dash(input, states);
        Walk(states);
        IsGround(states);
        Jump(input, states);
        time += Time.deltaTime;
        if (input.isAttackKeyDown)
        {
            BaseAttack(states);
            input.isAttackKeyDown = false;
            time = 0.0f;
        }
        else
        {
            if (states.soulState == SoulState.ATTACKING && time > 0.2f)
                states.soulState = SoulState.IDLE;
        }
    }
    public void BaseAttack(SoulStates states)
    {
        states.soulState = SoulState.ATTACKING;
        states.attackCount += 1;
        if (states.attackCount == 3)
            states.attackCount = 0.0f;
    }
    private void Dash(InputManager input, SoulStates states)
    {
        if (!states.isUseDash)
            return;
        if (input.isDashKeyDown)
        {
            states.soulState = SoulState.DASHING;
        }
        else if (states.soulState == SoulState.DASHING)
        {
            dashTime += Time.fixedDeltaTime;
            rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(lookAt * dashDistance * Time.fixedDeltaTime, 0), 1.0f);
        }
        else
        {
            return;
        }

        if (dashTime > 0.2f)
        {
            dashTime = 0.0f;
            states.soulState = SoulState.IDLE;
        }
        input.isDashKeyDown = false;
    }


    private void Jump(InputManager input, SoulStates states)
    {
        if (states.soulState == SoulState.DASHING)
            return;
        if (input.isDownJumpKeyDown && states.isOnGournd)
        {
            jumpCount++;
        }

        if (input.isJumpKeyDown && (jumpCount < states.availableJumpCount || (states.isOnGournd && !isPassingPlatform)))
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }
        if (!states.isOnGournd)
        {
            if (rigid.velocity.y < 0.0f)
            {
                rigid.gravityScale = fallGravityScale;
                states.soulState = SoulState.FALLING;
            }
            else
            {
                rigid.gravityScale = generalGravityScale;
                states.soulState = SoulState.JUMPING;
            }
        }
        input.isDownJumpKeyDown = false;
        input.isJumpKeyDown = false;
    }


    private void IsGround(SoulStates states)
    {
        RaycastHit2D platformHit = Physics2D.BoxCast(transform.position + new Vector3(0, 0.75f, 0), new Vector2(1.0f, 1.4f), 0, Vector2.up, 0, 64);
        if (platformHit.collider != null)
        {
            isPassingPlatform = true;
        }
        else
        {
            isPassingPlatform = false;
        }

        if (isPassingPlatform)
            return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f, 64);
        if (hit.collider != null)
        {
            if (rigid.velocity.y <= 0)
            {
                states.isOnGournd = true;
                jumpCount = 0;
            }
        }
        else
        {
            states.isOnGournd = false;
        }

    }

    private void Walk(SoulStates states)
    {
        if (states.soulState == SoulState.DASHING)
            return;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(states.moveDir * moveSpeed * Time.fixedDeltaTime, 0, 0), 0.8f);
        if (Mathf.Abs(states.moveDir) > 0.0f)
            lookAt = states.moveDir;
        if (Mathf.Abs(states.moveDir) > 0.0f && states.isOnGournd)
            states.soulState = SoulState.WALK;
        else if (Mathf.Abs(states.moveDir) == 0.0f && states.isOnGournd)
            states.soulState = SoulState.IDLE;

    }

    public void CalculateJumpHeight()
    {
        jumpPower = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * generalGravityScale));
    }
}
