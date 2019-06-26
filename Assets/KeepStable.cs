using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KeepStable : MonoBehaviour
{
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, -InputTracking.GetLocalPosition(XRNode.CenterEye).y + offset, transform.position.z);

        //transform.position = -InputTracking.GetLocalPosition(XRNode.CenterEye);

        //transform.rotation = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.CenterEye));
    }
}
