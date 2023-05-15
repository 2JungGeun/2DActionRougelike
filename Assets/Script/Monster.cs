using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private float time;
    private GameObject projectile;
    private GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        projectile = Resources.Load<GameObject>("Prefab/fireBall");
        hitEffect = Resources.Load<GameObject>("Prefab/hitEffect");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time>2.0f)
        {
            GameObject obj = Object.Instantiate(projectile, this.transform.position + new Vector3(1.0f, 0.0f, 0.0f), Quaternion.identity);
            obj.GetComponent<Projectile>().Initailize(1.0f, 5.0f, 50);
            time = 0.0f;
        }
    }

    public void Hit()
    {
        Debug.Log("����" + this.name);
        GameObject obj = Object.Instantiate(hitEffect, this.transform.position, Quaternion.identity);
        Destroy(obj, 0.3f);
    }
}
