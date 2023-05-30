using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

using System.Threading;
using UnityEngine;
using UnityEngine.UI;

using CSharpTcpDemo.com.dobot.api;
using System.Threading.Tasks;


public class Connect : MonoBehaviour
{

    public GameObject robot;
    private int j = -1;

    private Button button;

    string direction0;

    public double basement_height = 228;
    public double J1_length = 175;
    public double J2_length = 241;//圆台高度和两条机械臂长度


    public GameObject[] joints;



    public GameObject baseposition;            //机械臂基坐标

    public GameObject endposition;

    //获取定时数据
    private System.Timers.Timer mTimerReader = new System.Timers.Timer(300);


    public string inputIp = "192.168.1.6";   //输入ip地址
    public string Dashboard = "29999";           //输入端口号
    public string MovePort = "30003";
    public string FeedbackPort = "30004";


    public bool mIsManualDisconnect = false;

    private Feedback mFeedback = new Feedback();
    private DobotMove mDobotMove = new DobotMove();
    private Dashboard mDashboard = new Dashboard();

    //计时器

    void Start()
    {


     }

    // Update is called once per frame
    void Update()
    {

    }
    public Connect()
    {
     //    Debug.Log("Connect1111");
    //    mFeedback.NetworkErrorEvent += new DobotClient.OnNetworkError(this.OnNetworkErrorEvent_Feedback);
    //    mDobotMove.NetworkErrorEvent += new DobotClient.OnNetworkError(this.OnNetworkErrorEvent_DobotMove);
    //    mDashboard.NetworkErrorEvent += new DobotClient.OnNetworkError(this.OnNetworkErrorEvent_Dashboard);
    }

  
    //断开连接

    private void OnNetworkErrorEvent_Feedback(DobotClient sender, SocketError iErrCode)
    {
        if (mIsManualDisconnect) return;
        Task.Run(new Action(() => {
            string strIp = inputIp;
            int iPort = Parse2Int(FeedbackPort);
            DoNetworkErrorEvent(mFeedback, strIp, iPort);
        }));
    }
    private void OnNetworkErrorEvent_DobotMove(DobotClient sender, SocketError iErrCode)
    {
        if (mIsManualDisconnect) return;
        Task.Run(new Action(() => {
            string strIp = inputIp;
            int iPort = Parse2Int(MovePort);
            DoNetworkErrorEvent(mDobotMove, strIp, iPort);
        }));
    }
    private void OnNetworkErrorEvent_Dashboard(DobotClient sender, SocketError iErrCode)
    {
        if (mIsManualDisconnect) return;
        Task.Run(new Action(() => {
            string strIp = inputIp;
            int iPort = Parse2Int(MovePort);
            DoNetworkErrorEvent(mDashboard, strIp, iPort);
        }));
    }


    private int Parse2Int(string str)
    {
        int iValue = 0;
        try
        {
            iValue = int.Parse(str);
        }
        catch
        {
        }
        return iValue;
    }


    public void thisconnect()
    {
        string strIp = inputIp;
        int iPortFeedback = Parse2Int(FeedbackPort);
        int iPortMove = Parse2Int(MovePort);
        int iPortDashboard = Parse2Int(Dashboard);

        Debug.Log("Connecting...");
        Thread thd = new Thread(() => {
            if (!mDashboard.Connect(strIp, iPortDashboard))
            {
                Debug.Log(string.Format("Connect {0}:{1} Fail!!", strIp, iPortDashboard));
                return;
            }
            if (!mDobotMove.Connect(strIp, iPortMove))
            {
                Debug.Log(string.Format("Connect {0}:{1} Fail!!", strIp, iPortMove));
                return;
            }
            if (!mFeedback.Connect(strIp, iPortFeedback))
            {
                Debug.Log(string.Format("Connect {0}:{1} Fail!!", strIp, iPortFeedback));
                return;
            }

            mIsManualDisconnect = false;
            mTimerReader.Start();

            Debug.Log("Connect Success!!!");
        });
        thd.Start();

    }

    private void DoNetworkErrorEvent(DobotClient sender, string strIp, int iPort)
    {
        Debug.Log("retry connecting...");
        Thread thd = new Thread(() => {
            sender.Disconnect();

            mTimerReader.Stop();

            if (!sender.Connect(strIp, iPort))
            {
                Debug.Log("Connect Fail!!!");
                Thread.Sleep(500);
                DoNetworkErrorEvent(sender, strIp, iPort);
                return;
            }

            mTimerReader.Start();

            Debug.Log("Connect Success!!!");


        });
        thd.Start();
    }

    public void Disconnect()
    {
        Debug.Log("Disconnecting...");
        Thread thd = new Thread(() => {
            mFeedback.Disconnect();
            mDobotMove.Disconnect();
            mDashboard.Disconnect();
            Debug.Log("Disconnect success!!!");
            

            mTimerReader.Stop();
            
        });
        thd.Start(); 

    }


    //MoveJog():J1-;J1+;J2-;J2+;J3-;J3+
    public void DoMoveJog(string str)
    {
        Debug.Log(string.Format("send to {0}:{1}: MoveJog({2})", mDobotMove.IP, mDobotMove.Port, str));
        Thread thd = new Thread(() => {
            string ret = mDobotMove.MoveJog(str);
            Debug.Log(string.Format("Receive From {0}:{1}: {2}", mDobotMove.IP, mDobotMove.Port, ret));
        });
        thd.Start();

        Invoke("DoStopMoveJog", (float)0.2);        //几秒后调用方法
    }


    public void ReSet0() {

        JointMovJ_Click(0,0,0,0);
    }



    private void DoStopMoveJog()
    {
        mDobotMove.StopMoveJog();
    }

    public void EnableRobot()
    {
        mDashboard.EnableRobot();

    }

    public void DisableRobot() { 
    
    mDashboard.DisableRobot();

    }

    public void TimeoutEvent(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (!mFeedback.DataHasRead)
        {
            return;
        }
        mFeedback.DataHasRead = false;
      
    }

    public void JointMovJ_Click(double j1,double j2,double j3,double j4)
    {
        JointPoint pt = new JointPoint();
        pt.j1 = j1;
        pt.j2 = j2;
        pt.j3 = j3;
        pt.j4 = j4;


        Debug.Log(string.Format("send to {0}:{1}: JointMovJ({2})", mDobotMove.IP, mDobotMove.Port, pt.ToString()));
        Thread thd = new Thread(() => {
            if (mFeedback.IsEnabled())
            {
                mDashboard.EnableRobot();
            }
            string ret = mDobotMove.JointMovJ(pt);
            Debug.Log(string.Format("Receive From {0}:{1}: {2}", mDobotMove.IP, mDobotMove.Port, ret));
        });
        thd.Start();
    }


    private double Parse2Double(string str)
    {
        double value = 0.0;
        try
        {
            value = Double.Parse(str);
        }
        catch { }
        return value;
    }


    //虚拟mg400
    public void RotateJP(int i)
    {
        mg400Controller robotController = robot.GetComponent<mg400Controller>();
        robotController.RotateJoint((i-1), (RotationDirection)(1));

        Invoke("StopAllJ", (float)0.2); //几秒后调用方法
  
    }

    public void RotateJN(int i)
    {
        mg400Controller robotController = robot.GetComponent<mg400Controller>();
        robotController.RotateJoint((i - 1), (RotationDirection)(-1));

        Invoke("StopAllJ", (float)0.2); //几秒后调用方法

    }

    public void StopAllJ() {
        mg400Controller robotController = robot.GetComponent<mg400Controller>();
        robotController.StopAllJointRotations();
    }

    public void IKangle()
    {
        double theta0, theta1, theta2;//三个关节角

        float euler_theta0, euler_theta1, euler_theta2;

        double EG_2, AG_2, AE_2, BAE, EAG;//计算过程中的辅助线段长

        double Theta2, Theta3;

            theta0 = -Math.Atan((endposition.transform.position.z - baseposition.transform.position.z) / (endposition.transform.position.x - baseposition.transform.position.x));

            AG_2 = Math.Pow(endposition.transform.position.x - baseposition.transform.position.x, 2) + Math.Pow(endposition.transform.position.z - baseposition.transform.position.z, 2);

            EG_2 = Math.Pow(Math.Abs(endposition.transform.position.y - baseposition.transform.position.y) - basement_height, 2);

            AE_2 = AG_2 + EG_2;

        //由余弦定理

           Theta2 = (J1_length * J1_length +AE_2 - J2_length * J2_length) / (2 * J1_length * Math.Sqrt(AE_2));
           Theta3 = (J1_length * J1_length - AE_2 + J2_length * J2_length) / (2 * J1_length * J2_length);
            
           theta2 = Math.Acos((Math.Pow(J1_length, 2) + Math.Pow(J2_length, 2) - AE_2) / (2 * J1_length * J2_length));


           BAE = Math.Acos((Math.Pow(J1_length, 2) + AE_2 - Math.Pow(J2_length, 2)) / (2 * J1_length * Math.Sqrt(AE_2)));

            EAG = Math.Atan(Math.Sqrt(EG_2) / Math.Sqrt(AG_2));

            theta1 = BAE + EAG + Math.PI / 2;

        //    Debug.Log("theta1" + theta1);
        //    Debug.Log("theta2" + theta2);

             mg400Controller robotController = robot.GetComponent<mg400Controller>();

            euler_theta0 = (float)(theta0 * (180 / Math.PI));
            euler_theta1 = (float)(180 - (theta1 * (180 / Math.PI)));
            euler_theta2 = (float)(90 - (theta2 * (180 / Math.PI)));
        //判断目标点是否在解空间中
        if (euler_theta0>160 || euler_theta0 <-160|| euler_theta1<-25 || euler_theta1>85|| euler_theta2<-25 || euler_theta2 >105)
        {
            Debug.Log("超出工作空间");
        }
        else
        {

            JointMovJ_Click(-(int)euler_theta0, (int)euler_theta1, (int)euler_theta2, 0);

            Debug.Log("关节0" + "旋转" + euler_theta0 + "度");
            Debug.Log("关节1" + "旋转" + euler_theta1 + "度");
            Debug.Log("关节2" + "旋转" + euler_theta2 + "度");

            StartCoroutine(WaitForSeconds(2, () =>
            {

                robotController.RotateAngle(0, euler_theta0);
                robotController.RotateAngle(1, euler_theta1);
                robotController.RotateAngle(2, euler_theta2);
                //这里写上duration秒后要执行的内容
            }));

           
               
            robotController.StopAllJointRotations();
            return;
            
        }

    }

    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }

}