using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class mg400trajectory : MonoBehaviour
{

    public GameObject[] joints;

    public GameObject robot;

    public GameObject baseposition;

    public GameObject endposition;


    mg400Controller robotController;

    Transform bonePoint;//骨骼末端点

    public int ccdDepth =1;


    public float speed = 3.0f;

    
    //轨迹规划
    int number = 1;
    double LineLength = 0;
    double dx, dy, dz = 0;
    public List<double> x_start = new List<double>();
    public List<double> y_start = new List<double>();
    public List<double> z_start = new List<double>();
    int j = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        //判断目标点是否在解空间中
        if (endposition.transform.position.x < 250|| endposition.transform.position.x >350 || endposition.transform.position.z > 300 || endposition.transform.position.z <-300 || endposition.transform.position.y > 1000 || endposition.transform.position.y < 0)
        { 
            Debug.Log("超出工作空间" ); 
        }
        else
        {
            ccdgo(endposition.transform.position);
            //起始位置，终止位置， 每一帧最大距离  
            Debug.Log("关节4的位置" + joints[3].transform.position);

            Debug.Log("endposition" + endposition.transform.position);
        }

    }


    //轨迹规划


    Vector3 bonetopoint;//骨骼指向末端点向量

    Vector3 bonetotar;//骨骼指向目标点向量

    Vector3 vec;
    float angle;//旋转角度
    Vector3 axis;//转轴


    public void ccdgo(Vector3 p)  //解算CCD尝试够到世界坐标p位置
    {
        //
        //CCD解算骨骼节点
        //
        //
         robotController = robot.GetComponent<mg400Controller>();
        for (int i = 0; i < ccdDepth; ++i)
        { //迭代次数
            bonePoint = joints[3].transform;//骨骼末端点
            for (int j = 0; j <3; j++)
            { //依次计算每根骨骼的旋量

                //节点选择
                if (Vector3.Distance(p, bonetopoint) < 10f && j == 0)
                { //如果目标点位置过近，CCD计算跳过第一个节点
                    continue;
                }

                //Debug.Log("目标位置的坐标：" + p);
                //Debug.Log("关节" + j + "的坐标:" + joints[j].transform.position);

                ////         joints[0].transform.InverseTransformVector(p);

                //Debug.Log("转换后关节" + j + "的坐标:" + joints[j].transform.localPosition);
                //Debug.Log("转换后目标位置的坐标：" + vec);

                    vec = joints[j].transform.InverseTransformPoint(bonePoint.position);
                    bonetopoint = new Vector3(0, vec.y, vec.z);//X轴强制归0，化为平面内旋转

                    Debug.Log("末端关节:" + bonePoint.position);
                    Debug.Log("末端关节相对于" +j+ "的坐标" + vec);


                    //  bonetotar = joints[j].transform.position-p;//转入本地坐标

                    vec = joints[j].transform.InverseTransformPoint(p);
                    bonetotar = new Vector3(0, vec.y, vec.z);//X轴强制归0，化为平面内旋转


                Debug.Log("目标相对于" + j + "的坐标" + vec);


                if (Vector3.Cross(bonetopoint, bonetotar).normalized.x < 0)
                    angle = Vector3.Angle(bonetopoint, bonetotar);
                else
                    angle = -Vector3.Angle(bonetopoint, bonetotar);

                if (j == 0 && (angle > 160 || angle < -160)) {
                    angle = 160;
                }
                if (j == 1 && angle > 85) {
                   angle = 85;
                  }
                else if(j == 1 &&angle < -25){
                    angle = -25;
                }
                if (j == 2 && angle > 105)
                {
                    angle = 105;
                }
                else if (j == 2 && angle < -25) {
                    angle = -25;
                }


                Debug.Log("关节"+j+"旋转" + angle+"度");
                robotController.RotateAngle(j,angle);
      
            }


        }

        
    }



}


