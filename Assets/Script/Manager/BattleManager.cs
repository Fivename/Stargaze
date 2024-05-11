using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Star curStar;
    public Player player;
    /// <summary>
    /// Õ½¶·Êý¾Ý
    /// </summary>
    private int starHp;
    private int starMaxHp;
    private int playerHp;
    private int playerMaxHp;
    
    void Awake()
    {
        StarDatabase.Init();
        GameInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GameInit()
    {
        if (curStar == null) return;
        //SetStar();
        player = Player.GetInstance();
        curStar = StarDatabase.starList[0];
        starHp = starMaxHp = curStar.Hp;
        playerHp = player.curHp;
        playerMaxHp = player.maxHp;
    }
    public void SetStar(Star star)
    {
        curStar = star;
    }
}
