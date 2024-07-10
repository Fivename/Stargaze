using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public StarEntity curStar;
    public Player player;
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
    /// <summary>
    /// battle default rule 
    /// start:draw five
    /// 
    /// </summary>
    public bool isNewBattle = true;
    void Awake()
    {
        StarDatabase.Init();
        GameInit();
    }

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
        if (curStar == null) return;
        //SetStar();
        player = Player.GetInstance();
        SetStar((StarEntity)StarDatabase.starList[0]);
        playerHp = player.curHp;
        playerMaxHp = player.maxHp;

    }
    /// <summary>
    /// 设置敌人属性
    /// </summary>
    /// <param name="star"></param>
    public void SetStar(StarEntity star)
    {
        curStar = star;
        starHp = starMaxHp = curStar.hp;
    }

    private void BattleStart()
    {
        ItemLoop(EffectTime.BattleStart);
    }
    private void TurnStart()
    {
        ItemLoop(EffectTime.TurnStart);
    }
    private void DrawStart()
    {
        ItemLoop(EffectTime.DrawStart);
    }
    private void MainStart()
    {
        ItemLoop(EffectTime.MainStart);
    }
    private void DisDrawStart()
    {
        ItemLoop(EffectTime.DisDrawStart);
    }
    private void EndStart()
    {
        ItemLoop(EffectTime.EndStart);
    }
    private void ItemLoop(EffectTime et)
    {
        foreach (var itemEntity in player.itemEntityList)
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
}
