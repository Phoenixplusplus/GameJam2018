using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander
{ 
    
    public Vector3 WanderSteer ( Vector3 forward, float radius, float range )
    {
        Vector3 result = Vector3.zero;

        Vector3 centre =  forward.normalized * range;
        Vector3 Rand = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Rand.Normalize();
        result = centre + (Rand * radius);
        return result.normalized;
    }


}
