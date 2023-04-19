using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private float moveDir = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(moveDir * 5.0f * Time.fixedDeltaTime, 0, 0), 0.8f);
    }
}
