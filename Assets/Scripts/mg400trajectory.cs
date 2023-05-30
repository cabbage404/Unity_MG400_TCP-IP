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

    Transform bonePoint;//����ĩ�˵�

    public int ccdDepth =1;


    public float speed = 3.0f;

    
    //�켣�滮
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
        //�ж�Ŀ����Ƿ��ڽ�ռ���
        if (endposition.transform.position.x < 250|| endposition.transform.position.x >350 || endposition.transform.position.z > 300 || endposition.transform.position.z <-300 || endposition.transform.position.y > 1000 || endposition.transform.position.y < 0)
        { 
            Debug.Log("���������ռ�" ); 
        }
        else
        {
            ccdgo(endposition.transform.position);
            //��ʼλ�ã���ֹλ�ã� ÿһ֡������  
            Debug.Log("�ؽ�4��λ��" + joints[3].transform.position);

            Debug.Log("endposition" + endposition.transform.position);
        }

    }


    //�켣�滮


    Vector3 bonetopoint;//����ָ��ĩ�˵�����

    Vector3 bonetotar;//����ָ��Ŀ�������

    Vector3 vec;
    float angle;//��ת�Ƕ�
    Vector3 axis;//ת��


    public void ccdgo(Vector3 p)  //����CCD���Թ�����������pλ��
    {
        //
        //CCD��������ڵ�
        //
        //
         robotController = robot.GetComponent<mg400Controller>();
        for (int i = 0; i < ccdDepth; ++i)
        { //��������
            bonePoint = joints[3].transform;//����ĩ�˵�
            for (int j = 0; j <3; j++)
            { //���μ���ÿ������������

                //�ڵ�ѡ��
                if (Vector3.Distance(p, bonetopoint) < 10f && j == 0)
                { //���Ŀ���λ�ù�����CCD����������һ���ڵ�
                    continue;
                }

                //Debug.Log("Ŀ��λ�õ����꣺" + p);
                //Debug.Log("�ؽ�" + j + "������:" + joints[j].transform.position);

                ////         joints[0].transform.InverseTransformVector(p);

                //Debug.Log("ת����ؽ�" + j + "������:" + joints[j].transform.localPosition);
                //Debug.Log("ת����Ŀ��λ�õ����꣺" + vec);

                    vec = joints[j].transform.InverseTransformPoint(bonePoint.position);
                    bonetopoint = new Vector3(0, vec.y, vec.z);//X��ǿ�ƹ�0����Ϊƽ������ת

                    Debug.Log("ĩ�˹ؽ�:" + bonePoint.position);
                    Debug.Log("ĩ�˹ؽ������" +j+ "������" + vec);


                    //  bonetotar = joints[j].transform.position-p;//ת�뱾������

                    vec = joints[j].transform.InverseTransformPoint(p);
                    bonetotar = new Vector3(0, vec.y, vec.z);//X��ǿ�ƹ�0����Ϊƽ������ת


                Debug.Log("Ŀ�������" + j + "������" + vec);


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


                Debug.Log("�ؽ�"+j+"��ת" + angle+"��");
                robotController.RotateAngle(j,angle);
      
            }


        }

        
    }



}


