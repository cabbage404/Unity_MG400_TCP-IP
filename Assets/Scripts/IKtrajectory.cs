using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IKtrajectory : MonoBehaviour
{

    public double basement_height = 228;
    public double J1_length = 175;
    public double J2_length = 241;//Բ̨�߶Ⱥ�������е�۳���

    double theta0, theta1, theta2;//�����ؽڽ�

    float euler_theta0, euler_theta1, euler_theta2;

    double EF_2,EG_2,AG_2,AE_2,BAE,EAG;//��������еĸ����߶γ�

    public GameObject[] joints;

    public double test, test2;


    public GameObject robot;

    public GameObject baseposition;            //��е�ۻ�����

    public GameObject endposition;

    private Connect connect;
     
    
    public float timer = 0f;

    mg400Controller robotController;


    //�켣�滮
    int j = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;

    }

    //�켣�滮


    public void IKangle()
    {

            //�ж�Ŀ����Ƿ��ڽ�ռ���
            if (endposition.transform.position.x < 250 || endposition.transform.position.x > 350 || endposition.transform.position.z > 300 || endposition.transform.position.z < -300 || endposition.transform.position.y > 1000 || endposition.transform.position.y < 0)
            {
                Debug.Log("���������ռ�");
            }
            else
            {

                theta0 = -Math.Atan((endposition.transform.position.z - baseposition.transform.position.z) / (endposition.transform.position.x - baseposition.transform.position.x));

                AG_2 = Math.Pow(endposition.transform.position.x - baseposition.transform.position.x, 2) + Math.Pow(endposition.transform.position.z - baseposition.transform.position.z, 2);

                EG_2 = Math.Pow(Math.Abs(endposition.transform.position.y - baseposition.transform.position.y) - basement_height, 2);

                AE_2 = AG_2 + EG_2;

                //�����Ҷ���

                theta2 = Math.Acos((Math.Pow(J1_length, 2) + Math.Pow(J2_length, 2) - AE_2) / (2 * J1_length * J2_length));


                BAE = Math.Acos((Math.Pow(J1_length, 2) + AE_2 - Math.Pow(J2_length, 2)) / (2 * J1_length * Math.Sqrt(AE_2)));

                EAG = Math.Atan(Math.Sqrt(EG_2) / Math.Sqrt(AG_2));

                theta1 = BAE + EAG + Math.PI / 2;

                Debug.Log("theta1" + theta1);
                Debug.Log("theta2" + theta2);

                euler_theta0 = (float)(theta0 * (180 / Math.PI));
                euler_theta1 = (float)(180 - (theta1 * (180 / Math.PI)));
                euler_theta2 = (float)(90 - (theta2 * (180 / Math.PI)));

                //Debug.Log("ͶӰ��:" + chuizhitouying);
                //Debug.Log("б�߳�:" + xiebian);
                //Debug.Log("�̱ߣ�" + duanbian);

                Debug.Log("�ؽ�0" + "��ת" + euler_theta0 + "��");
                Debug.Log("�ؽ�1" + "��ת" + euler_theta1 + "��");
                Debug.Log("�ؽ�2" + "��ת" + euler_theta2 + "��");

                robotController.RotateAngle(0, euler_theta0);
                robotController.RotateAngle(1, euler_theta1);
                robotController.RotateAngle(2, euler_theta2);


                if (Vector3.Distance(endposition.transform.position, joints[3].transform.position) < 2f)
                {

                   connect.JointMovJ_Click(euler_theta0, euler_theta1, euler_theta2, 0);
                    robotController.StopAllJointRotations();
                    return;
                }
            }
   




        
    }
    private void OnDestroy()
    {
        Debug.Log("����ʱִ��");
        connect.DisableRobot();
        connect.Disconnect();
    }
    //��ʱ��
    private void delay()
    {
        long s=0;
        for (long i = 0; i < 100000000000; i++)
        {
            s += i * 10000;
            for (long j = 0; j < 1000000; j++) {
                s = j + i;
            }
        }


    }

}


