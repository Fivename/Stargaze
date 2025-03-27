using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public StarEntity curStar;
    public List<StarEntity> starList = new List<StarEntity>();
    public Player player;
    public BattlePlayer batPlayer = new BattlePlayer();
    [SerializeField]
    public PlayerDeck playerDeck;
    [SerializeField]
    public PlayerDataView playerDataView;
    /// <summary>
    /// 战斗数据
    /// </summary>
    private int starHp;
    private int starMaxHp;
    private int playerHp;
    private int playerMaxHp;
    private int attackPower;
    private int defensePower;
    private int defense;
    private int maxEnergy = 5;
    private int curEnergy = 5;
    /// <summary>
    /// battle default rule 
    /// start:draw five
    /// 
    /// </summary>
    public bool isNewBattle = true;
    public static bool CardUseable = false;
    public bool isPlayerPhase=true;
    public enum Phase
    {
        BattleStart = 0,
        TurnStart = 1,
        DrawStart = 2,
        MainStart = 3,
        DisDrawStart = 4,
        EndStart = 5

    }
    /// <summary>
    /// 卡牌使用时效果生效列表
    /// 按使用时的数据计算，再按顺序处理效果
    /// 防止在生效过程中数据变化导致卡面和结果不一致
    /// </summary>
    public struct CardUseEffect
    {
        public CardEffectType effectType;
        public ChooseType chooseType;
        public int num;
        public CardUseEffect(CardEffectType effectType,ChooseType chooseType,int num)
        {
            this.effectType = effectType;
            this.chooseType = chooseType;
            this.num = num;
        }
    }
    public List<CardUseEffect> cardUseEffectList = new List<CardUseEffect>();
    public Button enterBtn;
    public Button exitBtn;
    public Button endMainBtn;
    private void Awake()
    {
        enterBtn.onClick.AddListener(EnterBattle);
        exitBtn.onClick.AddListener(EnterBattle);
        endMainBtn.onClick.AddListener(EndMain);
    }
    void Start()
    {
        StarDatabase.Init();
        CardDatabase.GetInstance().LoadData();
        GameInit();
       
    }

    /// <summary>
    /// ActionList
    /// </summary>

    public static Action RefreshEvent;

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void EnterBattle()
    {
        if (isNewBattle)
        {
            GameInit();
        }
        else
        {

        }
    }
    private void GameInit()
    {
        //SetStar();
        //player = Player.GetInstance();
        player = Player.GetDefPlayer();
        batPlayer = batPlayer.InitBatPlayer(player);
        playerDeck.InitDeck();
        //starList = GetStar();
        //foreach (var star in starList)
        //{
        //    SetStar(StarDatabase.starList[0]);
        //    SetStar(star);
        //}
        SetStar(StarDatabase.starList[0]);


        if (curStar == null) return;
        playerHp = batPlayer.curHp;
        playerMaxHp = batPlayer.maxHp;
        BattleStart();
    }
    public List<StarEntity> GetStar()
    {

        return null;
    }
    /// <summary>
    /// 设置敌人属性
    /// </summary>
    /// <param name="star"></param>
    public void SetStar(Star star)
    {   
        curStar = new StarEntity(star);
        starHp = starMaxHp = curStar.hp;
    }
    #region -------------------------------------阶段处理
    private void PhaseChangeTo(Phase phase)
    {
        Debug.Log("change to phase:" + phase.ToString());
        switch (phase)
        {
            case Phase.BattleStart:
                BattleStart();
                break;
            case Phase.TurnStart:
                TurnStart();
                break;
            case Phase.DrawStart:
                DrawStart();
                break;
            case Phase.MainStart:
                MainStart();
                break;
            case Phase.DisDrawStart:
                DisDrawStart();
                break;
            case Phase.EndStart:
                EndStart();
                break;
            default:
                break;
        }

    }
    private void BattleStart()
    {
        CardUseable = false;
        ItemLoop(EffectTime.BattleStart);
        PhaseChangeTo(Phase.TurnStart);

        ManagerController.Instance.handCardManager.AddCard();
    }
    private void TurnStart()
    {
        RefreshEnerge();
        ItemLoop(EffectTime.TurnStart);
        PhaseChangeTo(Phase.DrawStart);
    }
    private void DrawStart()
    {
        ItemLoop(EffectTime.DrawStart);
        ManagerController.Instance.handCardManager.AddCard( 2 );
        PhaseChangeTo(Phase.MainStart);
    }
    private void MainStart()
    {
        ItemLoop(EffectTime.MainStart);
        CardUseable = true;
    }
    private void DisDrawStart()
    {
        CardUseable = false;
        ItemLoop(EffectTime.DisDrawStart);
        //弃牌阶段道具效果 异步执行完 跳转到回合结束
        PhaseChangeTo(Phase.EndStart);
    }
    private IEnumerator EndStart()
    {
        ItemLoop(EffectTime.EndStart);
        isPlayerPhase = false;
        yield return new WaitUntil(() => { return isPlayerPhase; });
        //等待地方执行完再行动
        PhaseChangeTo(Phase.TurnStart);
    }
    private void EndMain()
    {
        PhaseChangeTo(Phase.DisDrawStart);
    }
    private void EndBattle()
    {
        CardUseable = false;
    }
    #endregion
    private void ItemLoop(EffectTime et)
    {
        foreach (var itemEntity in batPlayer.itemEntityList)
        {
            foreach (var effect in itemEntity.effects)
            {
                if (effect.effectTime != et) continue;
                else
                {
                    if (itemEntity.effectCount == EffectCount.counts && itemEntity.curEffectCount > 0 || itemEntity.effectCount == EffectCount.AllTime)
                    {
                        foreach (var effectData in effect.effectData)
                        {
                            switch (effectData.type)
                            {
                                case EffectType.draw:
                                    ManagerController.Instance.handCardManager.AddCard();
                                    break;
                                case EffectType.defense:

                                    break;
                                case EffectType.power:
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
    private void RefreshEnerge()
    {
        curEnergy = maxEnergy;
        //RefreshEvent.Invoke();
    }
    /// <summary>
    /// 使用卡牌
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool UseCard(Card card) 
    {
        if(card.baseCost <= curEnergy)
        {
            if (card.defEffectList == null || card.defEffectList.Count == 0) return true;
            //遍历卡牌所有效果 添加到效果列表里面
            foreach (var effect in card.defEffectList)
            {
                int inf = 0;
                if (card.defEffectList.Count > 0)
                {
                    foreach (var coef in effect.coefs)
                    {
                        inf += InfCal(coef, effect);
                    }
                }
               
                switch (effect.type)
                {
                    case CardEffectType.Damage:
                        int damage = effect.num + batPlayer.attackPower + inf;
                        cardUseEffectList.Add(new CardUseEffect(CardEffectType.Damage, effect.chooseType, damage));
                        break;
                    case CardEffectType.Armor:
                        int armor = effect.num + batPlayer.defensePower + inf;
                        cardUseEffectList.Add(new CardUseEffect(CardEffectType.Armor, effect.chooseType, armor));
                        break;
                    case CardEffectType.Heal:
                        int heal = effect.num + inf;
                        cardUseEffectList.Add(new CardUseEffect(CardEffectType.Heal, effect.chooseType, heal));
                        break;
                    case CardEffectType.Cost:
                        int cost = effect.num + inf;
                        cardUseEffectList.Add(new CardUseEffect(CardEffectType.Cost, effect.chooseType, cost));
                        break;
                    case CardEffectType.Power:
                        int power = effect.num + inf;
                        cardUseEffectList.Add(new CardUseEffect(CardEffectType.Power, effect.chooseType, power));
                        break;
                }
                
            }
            //卡牌生效列表不空  则处理表内效果
            if (cardUseEffectList.Count > 0)
            {
                foreach (var cardEffect in cardUseEffectList)
                {
                    ActorEffect(cardEffect);
                    playerDataView.UpdateData(batPlayer);
                }
            }
            return true;
        } 
        return false;
    }
    public void ActorEffect(CardUseEffect cardEffect)
    {
        switch (cardEffect.effectType)
        {
            case CardEffectType.Damage:
                DamageStar(cardEffect.num, cardEffect.chooseType == ChooseType.One ? curStar : null);
                break;
            case CardEffectType.Armor:
                ArmorChange(cardEffect.num);
                break;
            case CardEffectType.Heal:
                HealChange(cardEffect.num);
                break;
            case CardEffectType.Cost:
                CostChange(cardEffect.num);
                break;
            case CardEffectType.Power:
                PowerChange(cardEffect.num);
                break;
            default:
                Debug.LogError("no type!");
                break;
        }
    }

    /// <summary>
    /// 计算被影响的数值
    /// </summary>
    /// <param name="coef"></param>
    /// <param name="effect"></param>
    /// <returns></returns>
    public int InfCal(ExtraCoef coef,CardEffect effect)
    {
        switch (coef.infType)
        {
            case InfType.Damage:
                return (int)MathF.Floor((effect.num + batPlayer.attackPower) * coef.coef);
            case InfType.Armor:
                return (int)Mathf.Floor(batPlayer.armor * coef.coef);
            case InfType.MaxHp:
                return (int)MathF.Floor(batPlayer.maxHp * coef.coef);
            case InfType.Cost:
                return (int)MathF.Floor(batPlayer.energy * coef.coef);
        }
        return 0;
    }
    #region DataChange
    public void DamageStar(int damage,StarEntity target)
    {
        if (damage > 0)
        {
            target.curHp -= damage;
        }
    }
    public void ArmorChange(int num)
    {
        if (num <= 0)
        {
            int armor = batPlayer.armor + num;
            batPlayer.armor = armor>=0? armor : 0;
        }
        else
        {
            batPlayer.armor += num;
        }
    }
    public void HealChange(int num)
    {
        batPlayer.curHp += num;
    }
    public void CostChange(int num)
    {
        batPlayer.energy += num;
    }
    public void PowerChange(int num)
    {
        batPlayer.attackPower += num;
    }
    #endregion
}
