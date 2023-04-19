using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public bool isJumpKeyDown;
    public bool isDownJumpKeyDown;
    public bool isDashKeyDown;
    public bool isAttackKeyDown;
    public InputManager()
    {
        isJumpKeyDown = false;
        isDownJumpKeyDown = false;
        isDashKeyDown = false;
        isAttackKeyDown = false;
    }
}


public class PlayerController : MonoBehaviour
{
    //Input관련 변수
    private InputManager input = new InputManager();
    
    //soul
    private Soul currSoul;
    //private List<Soul> soulList;

    // Start is called before the first frame update
    void Start()
    {
        //base 캐릭터 초기화
        InitializeSoul();
    }

    // Update is called once per frame
    void Update()
    {
        currSoul.States.moveDir = Input.GetAxisRaw("Horizontal");
        
        if (!(input.isJumpKeyDown || input.isDownJumpKeyDown) && Input.GetButtonDown("Jump"))
        {
            if (!Input.GetKey(KeyCode.DownArrow))
            {
                input.isJumpKeyDown = true;
            }
            else
            {
                input.isDownJumpKeyDown = true;
            }
        };
        if (Input.GetKeyDown(KeyCode.C))
        {
            input.isDashKeyDown = true;
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            input.isAttackKeyDown = true;
        }
        currSoul.Update();
    }

    private void FixedUpdate()
    {
        currSoul.FixedUpdate(input);
    }

    public void InitializeSoul()
    {
        currSoul = new Soul("Knight");
        currSoul.Start(this.GetComponent<Rigidbody2D>(), this.transform, this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>());
    }
}
