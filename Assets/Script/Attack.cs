using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    Transform transform;

    public void Start(Transform transform) { this.transform = transform; }
    
    virtual public void Update(InputManager input, SoulStates states)
    {

    }


    
}
