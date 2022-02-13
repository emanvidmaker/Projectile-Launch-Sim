using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 forceVector = Vector3.right * Physics.gravity.y;
        Vector3 d = Quaternion.Euler(0,0,transform.position.z) * forceVector;
        rb.AddForce(d,ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
