using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandCardManager : MonoBehaviour
{
    [Header("手牌动画参数")]
    /// <summary>  
    /// 卡牌起始位置  
    /// </summary>  
    public Vector3 rootPos;
    public GameObject HandCard;
    public GameObject HandCardPos;
    /// <summary>  
    /// 扇形半径  
    /// </summary>  
    public float size;
    /// <summary>  
    /// 卡牌出现最右/左角度
    /// </summary>  
    [SerializeField]private float rightPos;
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
    /// <summary>
    /// 鼠标在屏幕上的位
    /// </summary>
    [SerializeField] private float horizonPosY = -0.1f;
    /// <summary>
    /// 选定目标类卡牌选择目标时，卡牌在手牌区的中心位置
    /// </summary>
    [SerializeField] private Vector3 selectedMidPos;

    [Header("手牌数据")]
    [SerializeField] private int CardMaxCount;
    [SerializeField] List<GameObject> cardPool;
    private int cardCurCount = 0; 
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
    private Vector3 mousePos;
    [Header("射线检测")]
    private Ray ray;
    private RaycastHit hit;
    private LayerMask layerMask;
    [SerializeField] private int curSelectIndex;
    [SerializeField] public PlayerDeck playerDeck;
    private StarDataView targetStar;
    //private float balance = 0f;
    void Start()
    {
        UpdateCard();
        layerMask = LayerMask.GetMask("Card");
    }
    void Update()
    {
        SelectItemDetection();//射线检测
        TaskItemDetection();
        RefereshCard();
        //if(curPreviewCard!=null)Debug.Log(curPreviewCard.handCard.name);
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
        //float interval = (rightPos - leftPos) / count;
        float interval = (rightPos - leftPos) / (4+count);
        bool isOdd = count % 2 == 1;
        for (int i = 0; i < count; i++)
        {
            //float nowPos = leftPos + interval * (i+0.5f);
            float nowPos;
            if (isOdd)
            {
                nowPos = interval * (i - count / 2);
            }
            else
            {
                if (i < count / 2)
                {
                    nowPos = interval * (i - count / 2 + 0.5f);
                }
                else
                {
                    nowPos = interval * (i + 1 - count / 2 - 0.5f);
                }
            }
            rotPos.Add(nowPos);
        }
        return rotPos;
    }
    // Update is called once per frame  
    public bool isDebug = false;
    //射线检测
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
    public bool AddCard()
    {
        if (cardList == null)
        {
            cardList = new List<HandCard>();
        }
        if (cardList.Count >= CardMaxCount)
        {
            Debug.Log("手牌数量上限");
            return false;
        }
        cardCurCount++;
        UpdateCard();
        GameObject item = GetCard();
        item.transform.localPosition = cardPos.localPosition;
        HandCard handCard = item.GetComponent<HandCard>();
        handCard.isHandCard = true;
        cardList.Add(handCard);
        handCard.RefreshData(rootPos, 0, 0, -1);
        CardItem cardItem = item.GetComponent<CardItem>();
        cardItem.UpdateData(playerDeck.DrawCard());
        return true;
    }
    public void AddCard(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (!AddCard()) break;
        }
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
        //Destroy(item.gameObject);
        item.gameObject.SetActive(false);
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
        //加牌
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard();
        }
        //按下左键 并且当前有预览卡牌
        if (Input.GetMouseButtonDown(0) && curPreviewCard!=null && BattleManager.CardUseable) 
        {
            curPreviewCard.handCard.IsSelected = true;
            curSelectCard = curPreviewCard;
            curSelectIndex = curSelectCard.transform.GetSiblingIndex();
            curSelectCard.transform.SetAsLastSibling();
            SetSelectCard(curSelectCard,setStart:true);
        }
        //按住左键 并且当前有选择卡牌
        if(Input.GetMouseButton(0) && curSelectCard != null)
        {   
            SetSelectCard(curSelectCard);   
        }
        //抬起左键 并且当前有选择卡牌
        if(Input.GetMouseButtonUp(0) && curSelectCard != null)
        {
            curSelectCard.transform.SetSiblingIndex(curSelectIndex);
            curSelectCard.handCard.IsSelected = false;
            Debug.Log("Hide");
            ManagerController.Instance.arrowEffectManager.ShowArrow(false);
            Cursor.visible = true;
            if (curSelectCard.card.chooseType == ChooseType.One)
            {
                targetStar = CheckStar();
                if (targetStar != null)
                {
                    ManagerController.Instance.battleManager.UseCard(curSelectCard.card);
                }
            }
            else
            {
                ManagerController.Instance.battleManager.UseCard(curSelectCard.card);
            }
            curSelectCard = null;
        }
        //删牌
        if (Input.GetKeyDown(KeyCode.D))
        {
            RemoveCard();
            isDebug = true;
        }
    }
    public void PreviewCard()
    {
        oldmousePosition = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 1000, layerMask);
        if (hit.collider != null && hit.collider.gameObject != null)
        {
            //检测到新卡牌/没检测到卡牌 都需要把之前预览的卡牌的预览状态设为false
            if (curPreviewCard != null)
            {
                curPreviewCard.handCard.IsPreviewed = false;
            }
            curPreviewCard = hit.collider.gameObject.GetComponent<CardItem>();

            //被选中就是选中态 不是预览状态了
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
    public void SetSelectCard(CardItem card,bool setStart = false)
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (curSelectCard.card.chooseType != ChooseType.One)
        {
            curSelectCard.transform.position = cardPos;
            Vector3 localPos = curSelectCard.transform.localPosition;
            localPos.z = -100;//靠近相机
            curSelectCard.transform.localPosition = localPos;
            curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else 
        {
            Debug.Log(cardPos);
            //低于手牌区域---表示还在思考出牌
            if(cardPos.y < horizonPosY)
            {
                curSelectCard.transform.position = cardPos;
                Vector3 localPos = curSelectCard.transform.localPosition;
                localPos.z = -100;//靠近相机
                curSelectCard.transform.localPosition = localPos;
                curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            //高于手牌区域---表示在思考选定目标
            else
            {
                //cardPos.y = 0;4
                curSelectCard.transform.localPosition = selectedMidPos;
                //curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            //Debug.Log("show");
            ManagerController.Instance.arrowEffectManager.ShowArrow(true);
            if(setStart)ManagerController.Instance.arrowEffectManager.SetStartPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Cursor.visible = false;
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
        return Instantiate(HandCard, HandCardPos.transform);
    }
    //射线检测 鼠标上的目标
    public StarDataView CheckStar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Star");
        Physics.Raycast(ray, out hit, 1000, layerMask);
        if (hit.collider != null && hit.collider.gameObject != null)
        {
            return hit.collider.gameObject.GetComponentInParent<StarDataView>();
        }
        return null;
    }
}