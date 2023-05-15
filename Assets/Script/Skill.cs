using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Skill
{
    protected Soul soul;
    protected float cooldown;
    private float time;
    protected bool isSkillAvailable;
    protected bool isEnd;
    public Skill(Soul soul)
    {
        this.soul = soul;
        this.cooldown = 3.0f;
        this.isSkillAvailable = true;
        this.time = 0.0f;
        this.isEnd = false;
    }

    abstract public void start();
    abstract public void update();
    abstract public void fixedUpdate();
    abstract public bool end();

    public void ColldownUpdate()
    {
        if (isSkillAvailable) return;
        time += Time.deltaTime;
        if (time >= cooldown)
        {
            isSkillAvailable = true;
            time = 0.0f;
        }
    }

    public bool CanUseSkill()
    {
        if (this.isSkillAvailable)
            return true;
        else
            return false;
    }
}

public class KnightSkill : Skill
{
    GameObject prefab;
    private Vector3 startPos = new Vector3();
    private float time;
    private bool isAttacked;
    public KnightSkill(Soul soul) : base(soul)
    {
        prefab = Resources.Load<GameObject>("Prefab/fireBall");
    }
    public override void start()
    {
        isSkillAvailable = false;
        isEnd = false;
        isAttacked = false;
        time = 0.0f;
        startPos = soul.mTransform.position;
        /*GameObject obj = Object.Instantiate(prefab, soul.mTransform.position + new Vector3(soul.MoveData.lookAt * -1 * 2.0f, 1.0f, 0.0f), soul.mTransform.rotation);
        obj.GetComponent<Projectile>().Initailize(soul.MoveData.lookAt * -1, 5.0f, 50.0f);*/
        soul.Anime.Play("SKILL1");
    }

    public override void update()
    {
        if (time >= 0.4f)
            isEnd = true;
    }
    public override void fixedUpdate()
    {
        time += Time.fixedDeltaTime;
        soul.Rigid.velocity = new Vector2(soul.Rigid.velocity.x, 0.0f);
        if (Vector3.Distance(startPos, soul.mTransform.position) <= 7f)
        {
            soul.mTransform.position = Vector2.MoveTowards(soul.mTransform.position, soul.mTransform.position + new Vector3(soul.MoveData.lookAt * 40.0f * Time.fixedDeltaTime, 0), 1.0f);
        }
        else
        {
            if (!isAttacked)
            {
                float offsetX = Mathf.Abs(startPos.x + soul.mTransform.position.x) * 0.5f;
                float offsetY = soul.Collider.offset.y;
                float sizeX = Mathf.Abs(startPos.x - soul.mTransform.position.x);
                RaycastHit2D[] hits = Physics2D.BoxCastAll(new Vector2(offsetX, soul.mTransform.position.y + offsetY), new Vector2(sizeX, soul.Collider.bounds.size.y), 0, Vector2.up, 0, (int)Layer.Monster);
                foreach (RaycastHit2D hit in hits)
                {
                    hit.collider.GetComponent<Monster>().Hit();
                }
                isAttacked = true;
            }         
        }

    }
    public override bool end()
    {
        if (isEnd)
            return true;
        return false;
    }

}