using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableThing : MonoBehaviour {
    
    //default behavior is that if anything touches it, it will get destroyed
    internal virtual void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    public virtual void Explode()
    {
        Destroy(gameObject);
    }
}
