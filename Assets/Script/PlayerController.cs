using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager
{
    public float moveDir;
    public bool isJumpKeyDown;
    public bool isDownJumpKeyDown;
    public bool isDashKeyDown;
    public bool isAttackKeyDown;
    public InputManager()
    {
        moveDir = 0.0f;
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
    private int currIndex = 0;
    private Soul currSoul;
    private List<Soul> ownSouls;

    // Start is called before the first frame update
    void Start()
    {
        ownSouls = new List<Soul>();
        //base 캐릭터 초기화
        InitializeSoul();
        //ModifySoul("Masition", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (currSoul == null)
            return;
        input.moveDir = Input.GetAxisRaw("Horizontal");

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
        }
        else if (Input.GetKeyDown(KeyCode.C) && currSoul.mCooldownTime.dashCoolingdown && currSoul.Data.isUseDash)
        {
            input.isDashKeyDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            input.isAttackKeyDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            bool result = SwapSoul();
            if(result)
                currSoul.Start(input);
        }
        currSoul.HandleInput(input);
        currSoul.Update(input);
    }

    private void FixedUpdate()
    {
        currSoul.FixedUpdate(input);
    }

    public void InitializeSoul()
    {
        object[] args = new object[] { "Knight" };
        Type t = Type.GetType("Knight");
        ownSouls.Add((Soul)System.Activator.CreateInstance(t, args));
        ownSouls[currIndex].Initialize(this.GetComponent<Collider2D>(), this.GetComponent<Rigidbody2D>(), this.transform, this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>());
        currSoul = ownSouls[currIndex];
        this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animator/" + currSoul.Data.name + "_Anime") as RuntimeAnimatorController;
    }

    public bool SwapSoul()
    {
        if (ownSouls.Count == 2)
        {
            switch (currIndex)
            {
                case 0:
                    currIndex = 1;
                    break;
                case 1:
                    currIndex = 0;
                    break;
                default:
                    break;
            }
            //currSoul.SwapingSoul(input);
            currSoul = ownSouls[currIndex];
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animator/" + currSoul.Data.name + "_Anime") as RuntimeAnimatorController;
            Debug.Log("소울 변경");
            return true;
        }
        else
        {
            Debug.Log("변경할 소울이 존재하지 않습니다.");
            return false;
        }
    }

    public void ModifySoul(string name, int selectedNum)
    {
        if (ownSouls.Count == 1)
        {
            object[] args = new object[] { name };
            Type t = Type.GetType(name);
            ownSouls.Add((Soul)System.Activator.CreateInstance(t, args));
            ownSouls[ownSouls.Count - 1].Initialize(this.GetComponent<Collider2D>(), this.GetComponent<Rigidbody2D>(), this.transform, this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>());
        }
        else
        {
            object[] args = new object[] { name };
            Type t = Type.GetType(name);
            ownSouls[selectedNum] = (Soul)System.Activator.CreateInstance(t, args);
            ownSouls[selectedNum].Initialize(this.GetComponent<Collider2D>(), this.GetComponent<Rigidbody2D>(), this.transform, this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>());
        }
    }

    public List<string> GetPlayerSoulNameList()
    {
        List<string> nameList = new List<string>();
        foreach (Soul soul in ownSouls)
        {
            nameList.Add(soul.Data.name);
        }
        return nameList;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(currSoul.mTransform.position + new Vector3(currSoul.MoveData.lookAt * 1.0f, 0.75f, 0), new Vector3(1.8f, 1.4f, 0.0f));
    }
}
