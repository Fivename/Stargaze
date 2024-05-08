using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 手牌预制件
/// </summary>
public class HandCard : MonoBehaviour
{
    /// <summary>  
    /// 卡牌扇形展开中心点  
    /// </summary>  
    public Vector3 root;
    /// <summary>  
    /// 展开弧度  
    /// </summary>  
    public float rot;
    /// <summary>  
    /// 展开半径  
    /// </summary>  
    public float size;
    /// <summary>  
    /// 动画速度  
    /// </summary>  
    public float animSpeed = 10;
    /// <summary>  
    /// 高度值（决定卡牌层级）  
    /// </summary>  
    public float zPos = 0;
    [Header("是手牌")]
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
        //设置卡牌位置  
        float y = root.y + Mathf.Cos(rot) * size;
        float x = root.x + Mathf.Sin(rot) * size *-1;
        //Vector3 localPos = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.)
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(x, y, root.z), Time.deltaTime * animSpeed);
        //设置卡牌角度  
        if (transform.localPosition.z > 10)
        {
            Debug.Log("1");
        }
        float rotZ = rot * radianToDegree;
        Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, rotZ));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, Time.deltaTime * animSpeed * 30);
    }
    /// <summary>  
    /// 获取两个向量之间的弧度值0-2π  
    /// </summary>    /// <param name="positionA">点A坐标</param>  
    /// <param name="positionB">点B坐标</param>  
    /// <returns></returns>    
    public static float GetAngleInDegrees(Vector3 positionA, Vector3 positionB)
    {
        // 计算从A指向B的向量  
        Vector3 direction = positionB - positionA;
        // 将向量标准化  
        Vector3 normalizedDirection = direction.normalized;
        // 计算夹角的弧度值  
        float dotProduct = Vector3.Dot(normalizedDirection, Vector3.up);
        float angleInRadians = Mathf.Acos(dotProduct);

        //判断夹角的方向：通过计算一个参考向量与两个物体之间的叉乘，可以确定夹角是顺时针还是逆时针方向。这将帮助我们将夹角的范围扩展到0到360度。  
        Vector3 cross = Vector3.Cross(normalizedDirection, Vector3.up);
        if (cross.z > 0)
        {
            angleInRadians = 2 * Mathf.PI - angleInRadians;
        }
        // 将弧度值转换为角度值  
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        return angleInDegrees;
    }
    /// <summary>
    /// 外部选中该卡 
    /// </summary>
    public void Select()
    {

    }
}
