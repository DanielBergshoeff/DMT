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
        //transform.position = new Vector3(transform.position.x, -InputTracking.GetLocalPosition(XRNode.CenterEye).y + offset, -InputTracking.GetLocalPosition(XRNode.CenterEye).z);

        transform.localPosition = -InputTracking.GetLocalPosition(XRNode.CenterEye) + new Vector3(0f, offset, 0f);

        //transform.rotation = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.CenterEye));
    }
}
