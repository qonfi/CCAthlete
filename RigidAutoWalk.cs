//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking; // with NetworkBehaviour
//using UnityEngine.UI;
//

public class RigidAutoWalk : MonoBehaviour {
    #region
    private Rigidbody rBody;
    public float Force;
    #endregion


    private void Start()
    {
        if (Force < 0.001) { Force = 3f; }
        rBody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        //rBody.AddForce(Vector3.forward * Force);
        rBody.velocity = Vector3.forward * Force;
    }
}
