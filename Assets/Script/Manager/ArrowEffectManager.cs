using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffectManager : MonoBehaviour
{
    public GameObject reticleBlock;
    public GameObject reticleArrow;
    public int MaxCount = 18;
    public Vector3 startPoint; // ��ʼ��  
    private Vector3 controlPoint1; // ���Ƶ�1  
    private Vector3 controlPoint2; // ���Ƶ�2  
    private Vector3 endPoint; // ������  
    private List<GameObject> ArrowItemList;
    private List<SpriteRenderer> RendererItemList;
    private Animator Arrow_anim;
    private bool isSelect;

    public bool IsSelect
    {
        get => isSelect;
        set
        {
            if (isSelect != value)
            {
                isSelect = value;
                if (value == true)
                {
                    PlayAnim();
                }
            }
        }
    }
    private void Awake()
    {
        InitData();
        ShowArrow(false);
    }
    private void InitData()
    {
        ArrowItemList = new List<GameObject>();
        RendererItemList = new List<SpriteRenderer>();
        for (int i = 0; i < MaxCount; i++)
        {
            GameObject Arrow = (i == MaxCount - 1) ? reticleArrow : reticleBlock;
            GameObject temp = Instantiate(Arrow, this.transform);
            if (i == MaxCount - 1)
            {
                Arrow_anim = temp.GetComponent<Animator>();
            }
            ArrowItemList.Add(temp);
            //SpriteRenderer re = temp.transform.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer re = temp.transform.GetComponent<SpriteRenderer>();
            if (re != null)
            {
                RendererItemList.Add(re);
            }
        }
    }
    /// <summary>
    /// Test
    /// </summary>
    //public static int OkKey = 1;
    public void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    OkKey = 1;
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    OkKey = 0;
        //}
        //if (OkKey == 0) return;
        Move();
        DrawBezierCurve();
    }
    private void DrawBezierCurve()
    {
        for (int i = 0; i < ArrowItemList.Count; i++)
        {
            float t = i / (float)(ArrowItemList.Count - 1); // ���� t �� 0 �� 1 ֮��  
            Vector3 position = CalculateBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
            //Debug.Log("i:" + i + position + "\n");
            //ArrowItemList[i].gameObject.SetActive(i != ArrowItemList.Count - 1);
            ArrowItemList[i].gameObject.SetActive(true);
            ArrowItemList[i].transform.position = position;
            //ArrowItemList[i].transform.localPosition = position;
            ArrowItemList[i].transform.localScale = Vector3.one * (t / 2f) + Vector3.one * 0.3f;
            if (i > 0)
            {
                float SignedAngle = Vector2.SignedAngle(Vector2.up,
                ArrowItemList[i].transform.position - ArrowItemList[i - 1].transform.position);
                //ArrowItemList[i].transform.localPosition - ArrowItemList[i - 1].transform.localPosition);
                Vector3 euler = new Vector3(0, 0, SignedAngle);
                ArrowItemList[i].transform.rotation = Quaternion.Euler(euler);
            }
            //Debug.Log("aft--i:" + i + position);
        }
    }
    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;        
        // Gizmos.DrawLine(startPoint, controlPoint1);       
        // Gizmos.DrawSphere(startPoint, 0.1f);        
        // Gizmos.DrawSphere(controlPoint1, 0.1f);        
        // Gizmos.DrawLine(endPoint, controlPoint2);        
        // Gizmos.DrawSphere(endPoint, 0.1f);        
        // Gizmos.DrawSphere(controlPoint2, 0.1f);    
    }
    public void Move()
    {
        Vector3 mousePosition = Input.mousePosition; // ��ȡ���λ��  
        mousePosition.z = Camera.main.nearClipPlane; // ����z����Ϊ��������ü�ƽ���λ��  
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 2f;
        endPoint = worldPosition;

        controlPoint1 = (Vector2)startPoint + (worldPosition - startPoint) * new Vector2(-0.28f, 0.8f);
        controlPoint2 = (Vector2)startPoint + (worldPosition - startPoint) * new Vector2(0.12f, 1.4f);
        controlPoint1.z = startPoint.z;
        controlPoint2.z = startPoint.z;
        //Debug.Log("End: " + endPoint);
    }
    /// <summary>  
    /// ������ʼλ��  
    /// </summary>  
    public void SetStartPos(Vector3 pos)
    {
        startPoint = pos;
    }
    public void ShowArrow(bool show)
    {
        if (RendererItemList == null)
        {
            return;
        }
        if (show)
        {
            for (int i = 0; i < RendererItemList.Count; i++)
            {
                RendererItemList[i].color = Color.white;
            }
        }
        else
        {
            for (int i = 0; i < RendererItemList.Count; i++)
            {
                RendererItemList[i].color = Color.clear;
            }
        }
        
    }
    /// <summary>  
    /// ������ɫ  
    /// </summary>  
    public void SetColor(Color color)
    {
        if (RendererItemList == null)
        {
            return;
        }
        for (int i = 0; i < RendererItemList.Count; i++)
        {
            RendererItemList[i].color = color;
        }
    }
    /// <summary>  
    /// ������ɫ  
    /// </summary>  
    /// <param name="isSelect"></param>    
    public void SetColor(bool isSelect)
    {
        IsSelect = isSelect;
        if (isSelect)
        {
            SetColor(Color.red);
        }
        else
        {
            SetColor(Color.white);
        }
    }
    private void PlayAnim()
    {
        if (Arrow_anim == null)
        {
            return;
        }
        Arrow_anim.SetTrigger("select");
    }
    /// <summary>  
    /// ��ȡ3�ױ����������м��  
    /// </summary>  
    /// <param name="t">���� t �� 0 �� 1 ֮��</param>  
    /// <param name="startPoint">��ʼ��</param>  
    /// <param name="controlPoint1">�����Ƶ�</param>  
    /// <param name="controlPoint2">�յ���Ƶ�</param>  
    /// <param name="endPoint">�յ�</param>  
    /// <returns></returns>    
    /// P0(1-T)4 + 4P1*T(1-T)3 + 4P2*T2(1-T)2 + 4P3*T3(1-T) + P4*T4
    public static Vector3 CalculateBezierPoint(float t, Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * startPoint; // (1-t)^3 * P0  
        point += 3 * uu * t * controlPoint1; // 3 * (1-t)^2 * t * P1  
        point += 3 * u * tt * controlPoint2; // 3 * (1-t) * t^2 * P2  
        point += ttt * endPoint; // t^3 * P3  

        return point;
    }
}
