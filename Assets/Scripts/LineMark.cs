using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMark : MonoBehaviour
{



	private GameObject clone;
	private LineRenderer line;
	private int i;
	public GameObject obs;
	public GameObject run;
	Vector3 RunStart;
	Vector3 RunNext;

	// Use this for initialization
	void Start()
	{
		RunStart = run.transform.position;
		clone = (GameObject)Instantiate(obs, run.transform.position, run.transform.rotation);//��¡һ������LineRender������   
		line = clone.GetComponent<LineRenderer>();//��ø������ϵ�LineRender���  
												  //		//line.SetColors(Color.blue, Color.red);//������ɫ  
												  //		//line.SetWidth(0.2f, 0.1f);//���ÿ��  
		i = 0;
	}

	// Update is called once per frame  
	void Update()
	{

		RunNext = run.transform.position;

		if (RunStart != RunNext)
		{
			i++;
			line.SetVertexCount(i);//���ö����� 
			line.SetPosition(i - 1, run.transform.position);

		}

		RunStart = RunNext;



		//		if (Input.GetMouseButtonDown(0))  
		//		{  
		//			clone = (GameObject)Instantiate(obs, obs.transform.position, transform.rotation);//��¡һ������LineRender������   
		//			line = clone.GetComponent<LineRenderer>();//��ø������ϵ�LineRender���  
		//			line.SetColors(Color.blue, Color.red);//������ɫ  
		//			line.SetWidth(0.2f, 0.1f);//���ÿ��  
		//			i = 0;  
		//			print ("GetMouseButtonDown");
		//		}  
		//		if (Input.GetMouseButton(0))  
		//		{  
		//			i++;  
		//			line.SetVertexCount(i);//���ö�����  
		//			line.SetPosition(i - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15)));//���ö���λ��   
		//			print ("GetMouseButton");
		//
		//		}  




	}
}
