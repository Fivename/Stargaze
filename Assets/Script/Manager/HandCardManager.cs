using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class HandCardManager : MonoBehaviour
{
    [Header("手牌动画参数")]
    /// <summary>  
    /// 卡牌起始位置  
    /// </summary>  
    public Vector3 rootPos;
    /// <summary>  
    /// 卡牌对象  
    /// </summary>  
    public GameObject HandCard;
    /// <summary>  
    /// 扇形半径  
    /// </summary>  
    public float size;
    /// <summary>  
    /// 卡牌出现最右角度
    /// </summary>  
    [SerializeField]private float rightPos;
    /// <summary>  
    /// 卡牌出现最左角度  
    /// </summary>  
    [SerializeField]private float leftPos;
    /// <summary>
    /// 卡牌出校位置
    /// </summary>
    [SerializeField] private Transform cardPos;
    /// <summary>
    /// 预览卡牌时 其他卡牌扩展角度
    /// </summary>
    [SerializeField] private float extendDegree = 2;
    /// <summary>
    /// 选中卡牌时 其他卡牌扩展角度
    /// </summary>
    [SerializeField] private float selectExtendDegree = 1;
    [Header("手牌数据")]
    /// <summary>
    /// 最大手牌数量
    /// </summary>
    [SerializeField] private int CardMaxCount;
    /// <summary>
    /// 手牌池子
    /// </summary>
    [SerializeField] List<GameObject> cardPool;
    /// <summary>
    /// 当前手牌数量
    /// </summary>
    private int cardCurCount = 0;
    /// <summary>  
    /// 手牌列表  
    /// </summary>  
    private List<HandCard> cardList;
    /// <summary>  
    /// 手牌位置  
    /// </summary>  
    [SerializeField]private List<float> rotPos;
    /// <summary>
    /// 奇数位置
    /// </summary>
    [SerializeField] private List<float> oddPos;
    /// <summary>
    /// 偶数位置
    /// </summary>
    [SerializeField] private List<float> evenPos;
    /// <summary>
    /// 比例
    /// </summary>
    [SerializeField] private float upRate;
    private CardItem curSelectCard;
    private CardItem curPreviewCard;
    private Vector3 oldmousePosition;
    [SerializeField]private int curSelectIndex;
    //private float balance = 0f;

    void Start()
    {
        UpdateCard();
    }
    void Update()
    {
        SelectItemDetection();//射线检测
        TaskItemDetection();
        RefereshCard();
        if(curPreviewCard!=null)Debug.Log(curPreviewCard.handCard.name);
    }
    /// <summary>  
    /// 数据初始化  
    /// </summary>  
    public void UpdateCard()
    {
        rotPos = UpdateRotPos(cardCurCount);
    }
    /// <summary>  
    /// 初始化位置  
    /// </summary>  
    /// <param name="count"></param>    
    /// <param name="interval"></param>    
    /// <returns></returns>    
    public List<float> UpdateRotPos(int count)
    {
        List<float> rotPos = new List<float>();
        float interval = (rightPos - leftPos) / count;
        for (int i = 0; i < count; i++)
        {
            float nowPos = leftPos + interval * (i+0.5f);
            rotPos.Add(nowPos);
        }
        return rotPos;
    }
    // Update is called once per frame  
    public bool isDebug = false;
    public void SelectItemDetection()
    {
        if (oldmousePosition == Input.mousePosition)
        {
            return;
        }
        PreviewCard();
    }
    /// <summary>  
    /// 添加卡牌  
    /// </summary>  
    public void AddCard()
    {
        if (cardList == null)
        {
            cardList = new List<HandCard>();
        }
        if (cardList.Count >= CardMaxCount)
        {
            Debug.Log("手牌数量上限");
            return;
        }
        cardCurCount++;
        UpdateCard();
        GameObject item = GetCard();
        item.transform.localPosition = cardPos.localPosition;
        HandCard text = item.GetComponent<HandCard>();
        text.isHandCard = true;
        cardList.Add(text);
        text.RefreshData(rootPos, 0, 0, -1);
    }
    /// <summary>  
    /// 手牌状态刷新  
    /// </summary>  
    public void RefereshCard()
    {
        if (cardList == null)
        {
            return;
        }
        float extendDeg;
        if (curPreviewCard != null)
        {
            extendDeg = extendDegree;
        }else if (curSelectCard != null)
        {
            extendDeg = selectExtendDegree;
        }
        else
        {
            extendDeg = 0f;
        }
        int arriveMid=1;
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].IsPreviewed || cardList[i].IsSelected)
            {
                cardList[i].RefreshData(rootPos, rotPos[i], size, cardList.Count-i, upRate);
                arriveMid = -1;
            }
            else
            {
                cardList[i].RefreshData(rootPos, rotPos[i] + arriveMid * extendDeg, size, cardList.Count - i);
            }
        }
    }
    /// <summary>  
    /// 销毁最后一张卡牌  
    /// </summary>  
    public void RemoveCard()
    {
        if (cardList == null || cardList.Count == 0)
        {
            return;
        }
        HandCard item = cardList[cardList.Count - 1];
        if(curPreviewCard != null)
        {
            item = curPreviewCard.handCard;
        }
        cardList.Remove(item);
        Destroy(item.gameObject);
        cardCurCount--;
        UpdateCard();
    }
    /// <summary>  
    /// 销毁指定卡牌  
    /// </summary>  
    /// <param name="item"></param>    
    public void RemoveCard(HandCard item)
    {
        if (cardList == null)
        {
            return;
        }
        cardList.Remove(item);
        Destroy(item.gameObject);
    }

    /// <summary>  
    /// 玩家操作检测  
    /// </summary>  
    public void TaskItemDetection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard();
        }
        if (Input.GetMouseButtonDown(0) && curPreviewCard!=null)
        {
            curPreviewCard.handCard.IsSelected = true;
            curSelectCard = curPreviewCard;
            curSelectIndex = curSelectCard.transform.GetSiblingIndex();
            curSelectCard.transform.SetAsLastSibling();
            SetSelectCard(curSelectCard);
        }
        if(Input.GetMouseButton(0) && curSelectCard != null)
        {   
            SetSelectCard(curSelectCard);
            
        }
        if(Input.GetMouseButtonUp(0) && curSelectCard != null)
        {
            curSelectCard.transform.SetSiblingIndex(curSelectIndex);
            curSelectCard.handCard.IsSelected = false;
            curSelectCard = null;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RemoveCard();
            isDebug = true;
        }
    }
    public void PreviewCard()
    {
        oldmousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Card");
        Physics.Raycast(ray, out hit, 1000, layerMask);
        if (hit.collider != null && hit.collider.gameObject != null)
        {
            //检测到新卡牌/没检测到卡牌 都需要把之前预览的卡牌的预览状态设为false
            if (curPreviewCard != null)
            {
                curPreviewCard.handCard.IsPreviewed = false;
            }
            curPreviewCard = hit.collider.gameObject.GetComponent<CardItem>();
            //被选中就不是预览状态了
            if (!curPreviewCard.handCard.IsSelected)
            {
                curPreviewCard.handCard.IsPreviewed = true;
            }
            else
            {
                curPreviewCard = null;
            }
            return;
        }
        if (curPreviewCard != null)
        {
            //原先预览到的卡牌现在鼠标不在上面 只可能是取消释放或者鼠标移开了
            curPreviewCard.handCard.IsPreviewed = false;
            curPreviewCard.handCard.IsSelected = false;
            curPreviewCard = null;
        }
        //return;
    }
    public void SetSelectCard(CardItem card)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (curSelectCard.chooseType != ChooseType.One)
        {
            curSelectCard.transform.position = cardPos;
            Vector3 localPos = curSelectCard.transform.localPosition;
            localPos.z = -100;//靠近相机
            curSelectCard.transform.localPosition = localPos;
            curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else 
        {
            if(cardPos.y < 100)
            {
                curSelectCard.transform.position = cardPos;
                Vector3 localPos = curSelectCard.transform.localPosition;
                localPos.z = -100;//靠近相机
                curSelectCard.transform.localPosition = localPos;
                curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                cardPos.y = 0;
                curSelectCard.transform.position = cardPos;
                Vector3 localPos = curSelectCard.transform.localPosition;
                localPos.z = -100;//靠近相机
                curSelectCard.transform.localPosition = localPos;
                curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
    }
    /// <summary>
    /// 获取/生成卡牌游戏物体
    /// </summary>
    /// <returns></returns>
    public GameObject GetCard()
    {
        foreach (GameObject card in cardPool)
        {
            if (card == null) continue;
            if (!card.activeSelf)
            {
                card.SetActive(true);
                return card;
            }
        }
        return Instantiate(HandCard, this.transform);
    }
    public void DrawBezel(Vector3 startPos,Vector3 targetPos)
    {

    }
}