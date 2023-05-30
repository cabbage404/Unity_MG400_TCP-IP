using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };

public class JointController : MonoBehaviour
{

    public RotationDirection rotationState = RotationDirection.None;
    private float speed = 2;

    public ArticulationBody articulation;

    private float rotationGoal;

    private float currentRotationRads;


    // Start is called before the first frame update
    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
        Time.fixedDeltaTime = (float)0.0133;
    }

    // Update Fixed time update
    //speed
    void FixedUpdate()
    {
        articulation.jointFriction = 5;


        if (rotationState != RotationDirection.None)
        {
            float rotationChange = (float)rotationState * speed * Time.fixedDeltaTime;
            //   Debug.Log("         rotationState         " + (float)rotationState);
            //    Debug.Log("         Time.fixedDeltaTime        " + Time.fixedDeltaTime);
            //   Debug.Log("                jointcontroller   " + rotationChange);
            //  Debug.Log("  speed " + speed);

            //  Debug.Log("update方法" + Time.fixedDeltaTime);
            //    Debug.Log(" rotation" + rotationChange);

            rotationGoal = CurrentPrimaryAxisRotation() + rotationChange;
            RotateTo(rotationGoal);
            return;
        }
        else
        {
            articulation.jointFriction = 10000000;
        }
        //  return;
    }


    // MOVEMENT HELPERS
    //当前主轴旋转位置
    float CurrentPrimaryAxisRotation()
    {
        float currentRotationRads = articulation.jointPosition[0];
        //   Debug.Log("currentRotationRads" + articulation.jointPosition[0]);
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }
    //var类型可以根据你赋值的类型来确定变量的类型：例如： var a = 10，则a是整形
    public void RotateTo(float primaryAxisRotation)
    {
            var drive = articulation.xDrive;
            drive.target = primaryAxisRotation;
            articulation.xDrive = drive;
    }

    public float getTarget() {

        return articulation.xDrive.target;
    } 



}
