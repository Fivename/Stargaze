using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class HandCardManager : MonoBehaviour
{
    [Header("���ƶ�������")]
    /// <summary>  
    /// ������ʼλ��  
    /// </summary>  
    public Vector3 rootPos;
    /// <summary>  
    /// ���ƶ���  
    /// </summary>  
    public GameObject HandCard;
    /// <summary>  
    /// ���ΰ뾶  
    /// </summary>  
    public float size;
    /// <summary>  
    /// ���Ƴ������ҽǶ�
    /// </summary>  
    [SerializeField]private float rightPos;
    /// <summary>  
    /// ���Ƴ�������Ƕ�  
    /// </summary>  
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
    [Header("��������")]
    /// <summary>
    /// �����������
    /// </summary>
    [SerializeField] private int CardMaxCount;
    /// <summary>
    /// ���Ƴ���
    /// </summary>
    [SerializeField] List<GameObject> cardPool;
    /// <summary>
    /// ��ǰ��������
    /// </summary>
    private int cardCurCount = 0;
    /// <summary>  
    /// �����б�  
    /// </summary>  
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
    [SerializeField]private int curSelectIndex;
    //private float balance = 0f;

    void Start()
    {
        UpdateCard();
    }
    void Update()
    {
        SelectItemDetection();//���߼��
        TaskItemDetection();
        RefereshCard();
        if(curPreviewCard!=null)Debug.Log(curPreviewCard.handCard.name);
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
    /// ��ӿ���  
    /// </summary>  
    public void AddCard()
    {
        if (cardList == null)
        {
            cardList = new List<HandCard>();
        }
        if (cardList.Count >= CardMaxCount)
        {
            Debug.Log("������������");
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
        Destroy(item.gameObject);
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
            //��⵽�¿���/û��⵽���� ����Ҫ��֮ǰԤ���Ŀ��Ƶ�Ԥ��״̬��Ϊfalse
            if (curPreviewCard != null)
            {
                curPreviewCard.handCard.IsPreviewed = false;
            }
            curPreviewCard = hit.collider.gameObject.GetComponent<CardItem>();
            //��ѡ�оͲ���Ԥ��״̬��
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
    public void SetSelectCard(CardItem card)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (curSelectCard.chooseType != ChooseType.One)
        {
            curSelectCard.transform.position = cardPos;
            Vector3 localPos = curSelectCard.transform.localPosition;
            localPos.z = -100;//�������
            curSelectCard.transform.localPosition = localPos;
            curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else 
        {
            if(cardPos.y < 100)
            {
                curSelectCard.transform.position = cardPos;
                Vector3 localPos = curSelectCard.transform.localPosition;
                localPos.z = -100;//�������
                curSelectCard.transform.localPosition = localPos;
                curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                cardPos.y = 0;
                curSelectCard.transform.position = cardPos;
                Vector3 localPos = curSelectCard.transform.localPosition;
                localPos.z = -100;//�������
                curSelectCard.transform.localPosition = localPos;
                curSelectCard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
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
        return Instantiate(HandCard, this.transform);
    }
    public void DrawBezel(Vector3 startPos,Vector3 targetPos)
    {

    }
}