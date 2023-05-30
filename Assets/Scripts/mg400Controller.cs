using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg400Controller : MonoBehaviour
{
    [System.Serializable]
    public struct Joint
    {
        public string inputAxis;
        public GameObject robotPart;
    }
    public Joint[] joints;

    public ArticulationBody articulation;


    public void StopAllJointRotations()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            GameObject robotPart = joints[i].robotPart;
            
            UpdateRotationState(RotationDirection.None, robotPart);
           
        }
    }

    //³õÊ¼»¯joint
    public void RotateJoint(int jointIndex, RotationDirection direction)
    {
     //   StopAllJointRotations();
        Joint joint = joints[jointIndex];
        UpdateRotationState(direction, joint.robotPart);

    }

    public void RotateAngle(int jointIndex,float angle)
    {
            //    StopAllJointRotations();
            Joint joint = joints[jointIndex];

            JointController jointController = joint.robotPart.GetComponent<JointController>();

            jointController.RotateTo(angle);


    }


    // HELPERS

    static void UpdateRotationState(RotationDirection direction, GameObject robotPart)
    {
        JointController jointController = robotPart.GetComponent<JointController>();
        jointController.rotationState = direction;
    }
}
