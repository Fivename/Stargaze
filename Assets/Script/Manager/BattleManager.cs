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
    public int starHp;
    public int starMaxHp;
    public int playerHp;
    public int playeerMaxHp;
    
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

    }
    public void SetStar(Star star)
    {
        curStar = star;
    }
}
