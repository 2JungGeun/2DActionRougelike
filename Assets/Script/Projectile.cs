using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 startPos = new Vector2();
    private float distance;
    private float damage;
    private float lookAt;
    private float speed = 8.0f;
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(startPos, this.transform.position) >= distance)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * lookAt * speed * Time.fixedDeltaTime);
    }

    public void Initailize(float lookAt, float distance, float damage)
    {
        this.distance = distance;
        this.damage = damage;
        this.lookAt = lookAt;
        switch (lookAt)
        {
            case 1.0f:
                this.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case -1.0f:
                this.GetComponent<SpriteRenderer>().flipX = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<Monster>().Hit();
            Destroy(this.gameObject);
        }
        else if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Hit();
            Destroy(this.gameObject);
        }
        else if(collision.CompareTag("Ground") || collision.CompareTag("Platform"))
        {
            Destroy(this.gameObject);
        }
        
    }
}
