using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class HandCardManager : MonoBehaviour
{
    [Header("���ƶ�������")]
    /// <summary>  
    /// ������ʼλ��  
    /// </summary>  
    public Vector3 rootPos;
    public GameObject HandCard;
    public GameObject HandCardPos;
    /// <summary>  
    /// ���ΰ뾶  
    /// </summary>  
    public float size;
    /// <summary>  
    /// ���Ƴ�������/��Ƕ�
    /// </summary>  
    [SerializeField]private float rightPos;
    [SerializeField]private float leftPos;
    /// <summary>
    /// ���Ƴ�Уλ��
    /// </summary>
    [SerializeField] private Transform cardPos;
    /// <summary>
    /// Ԥ������ʱ ����������չ�Ƕ�
    /// </summary>
    [SerializeField] private float extendDegree = 2;
    /// <summary>
    /// ѡ�п���ʱ ����������չ�Ƕ�
    /// </summary>
    [SerializeField] private float selectExtendDegree = 1;
    /// <summary>
    /// �������Ļ�ϵ�λ
    /// </summary>
    [SerializeField] private float horizonPosY = -0.1f;
    /// <summary>
    /// ѡ��Ŀ���࿨��ѡ��Ŀ��ʱ��������������������λ��
    /// </summary>
    [SerializeField] private Vector3 selectedMidPos;

    [Header("��������")]
    [SerializeField] private int CardMaxCount;
    [SerializeField] List<GameObject> cardPool;
    private int cardCurCount = 0; 
    private List<HandCard> cardList;
    /// <summary>  
    /// ����λ��  
    /// </summary>  
    [SerializeField]private List<float> rotPos;
    /// <summary>
    /// ����λ��
    /// </summary>
    [SerializeField] private List<float> oddPos;
    /// <summary>
    /// ż��λ��
    /// </summary>
    [SerializeField] private List<float> evenPos;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField] private float upRate;
    private CardItem curSelectCard;
    private CardItem curPreviewCard;
    private Vector3 oldmousePosition;
    private Vector3 mousePos;
    [Header("���߼��")]
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
        SelectItemDetection();//���߼��
        TaskItemDetection();
        RefereshCard();
        //if(curPreviewCard!=null)Debug.Log(curPreviewCard.handCard.name);
    }
    /// <summary>  
    /// ���ݳ�ʼ��  
    /// </summary>  
    public void UpdateCard()
    {
        rotPos = UpdateRotPos(cardCurCount);
    }
    /// <summary>  
    /// ��ʼ��λ��  
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
    //���߼��
    public void SelectItemDetection()
    {
        if (oldmousePosition == Input.mousePosition)
        {
            return;
        }
        PreviewCard();
    }
    /// <summary>  
    /// ��ӿ���  
    /// </summary>  
    public bool AddCard()
    {
        if (cardList == null)
        {
            cardList = new List<HandCard>();
        }
        if (cardList.Count >= CardMaxCount)
        {
            Debug.Log("������������");
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
    /// ����״̬ˢ��  
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
    /// �������һ�ſ���  
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
    /// ����ָ������  
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
    /// ��Ҳ������  
    /// </summary>  
    public void TaskItemDetection()
    {
        //����
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard();
        }
        //������� ���ҵ�ǰ��Ԥ������
        if (Input.GetMouseButtonDown(0) && curPreviewCard!=null && BattleManager.CardUseable) 
        {
            curPreviewCard.handCard.IsSelected = true;
            curSelectCard = curPreviewCard;
            curSelectIndex = curSelectCard.transform.GetSiblingIndex();
            curSelectCard.transform.SetAsLastSibling();
            SetSelectCard(curSelectCard,setStart:true);
        }
        //��ס��� ���ҵ�ǰ��ѡ����
        if(Input.GetMouseButton(0) && curSelectCard != null)
        {   
            SetSelectCard(curSelectCard);   
        }
        //̧����� ���ҵ�ǰ��ѡ����
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
        //ɾ��
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
            //��⵽�¿���/û��⵽���� ����Ҫ��֮ǰԤ���Ŀ��Ƶ�Ԥ��״̬��Ϊfalse
            if (curPreviewCard != null)
            {
                curPreviewCard.handCard.IsPreviewed = false;
            }
            curPreviewCard = hit.collider.gameObject.GetComponent<CardItem>();

            //��ѡ�о���ѡ��̬ ����Ԥ��״̬��
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
            //ԭ��Ԥ�����Ŀ���������겻������ ֻ������ȡ���ͷŻ�������ƿ���
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
            localPos.z = -100;//�������
            curSelectCard.transform.localPosition = localPos;
            curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else 
        {
            Debug.Log(cardPos);
            //������������---��ʾ����˼������
            if(cardPos.y < horizonPosY)
            {
                curSelectCard.transform.position = cardPos;
                Vector3 localPos = curSelectCard.transform.localPosition;
                localPos.z = -100;//�������
                curSelectCard.transform.localPosition = localPos;
                curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            //������������---��ʾ��˼��ѡ��Ŀ��
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
    /// ��ȡ/���ɿ�����Ϸ����
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
    //���߼�� ����ϵ�Ŀ��
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