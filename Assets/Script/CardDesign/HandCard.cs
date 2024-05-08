using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����Ԥ�Ƽ�
/// </summary>
public class HandCard : MonoBehaviour
{
    /// <summary>  
    /// ��������չ�����ĵ�  
    /// </summary>  
    public Vector3 root;
    /// <summary>  
    /// չ������  
    /// </summary>  
    public float rot;
    /// <summary>  
    /// չ���뾶  
    /// </summary>  
    public float size;
    /// <summary>  
    /// �����ٶ�  
    /// </summary>  
    public float animSpeed = 10;
    /// <summary>  
    /// �߶�ֵ���������Ʋ㼶��  
    /// </summary>  
    public float zPos = 0;
    [Header("������")]
    [SerializeField]public bool isHandCard = false;
    [SerializeField]private bool isSelected = false;
    [SerializeField]private bool isPreviewed;
    public bool IsPreviewed
    {
        get
        {
            return isPreviewed;
        }
        set
        {
            isPreviewed = value;
        }
    }
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
        }
    }
    const float radianToDegree = 180f / Mathf.PI;
    public void RefreshData(Vector3 root, float rot, float size, float zPos,float upRate = 0)
    {
        this.root = root + transform.up*upRate;
        this.rot = rot/ radianToDegree;
        this.size = size;
        this.root.z = zPos;
    }
    // Update is called once per frame  
    void Update()
    {
        if (isHandCard && !isSelected)
        {
            SetPos();
        }
    }
    public void SetPos()
    {
        //���ÿ���λ��  
        float y = root.y + Mathf.Cos(rot) * size;
        float x = root.x + Mathf.Sin(rot) * size *-1;
        //Vector3 localPos = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.)
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(x, y, root.z), Time.deltaTime * animSpeed);
        //���ÿ��ƽǶ�  
        if (transform.localPosition.z > 10)
        {
            Debug.Log("1");
        }
        float rotZ = rot * radianToDegree;
        Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, rotZ));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, Time.deltaTime * animSpeed * 30);
    }
    /// <summary>  
    /// ��ȡ��������֮��Ļ���ֵ0-2��  
    /// </summary>    /// <param name="positionA">��A����</param>  
    /// <param name="positionB">��B����</param>  
    /// <returns></returns>    
    public static float GetAngleInDegrees(Vector3 positionA, Vector3 positionB)
    {
        // �����Aָ��B������  
        Vector3 direction = positionB - positionA;
        // ��������׼��  
        Vector3 normalizedDirection = direction.normalized;
        // ����нǵĻ���ֵ  
        float dotProduct = Vector3.Dot(normalizedDirection, Vector3.up);
        float angleInRadians = Mathf.Acos(dotProduct);

        //�жϼнǵķ���ͨ������һ���ο���������������֮��Ĳ�ˣ�����ȷ���н���˳ʱ�뻹����ʱ�뷽���⽫�������ǽ��нǵķ�Χ��չ��0��360�ȡ�  
        Vector3 cross = Vector3.Cross(normalizedDirection, Vector3.up);
        if (cross.z > 0)
        {
            angleInRadians = 2 * Mathf.PI - angleInRadians;
        }
        // ������ֵת��Ϊ�Ƕ�ֵ  
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        return angleInDegrees;
    }
    /// <summary>
    /// �ⲿѡ�иÿ� 
    /// </summary>
    public void Select()
    {

    }
}
