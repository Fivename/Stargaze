using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerController : MonoBehaviour
{
    private static ManagerController instance;
    public static ManagerController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.Find("Manager").GetComponent<ManagerController>();
            }
            if(instance == null)
            {
                Debug.LogError("cant find Manager");
            }
            return instance;
        }
    }
    [SerializeField] public HandCardManager handCardManager;
    [SerializeField] public BattleManager battleManager;
    [SerializeField] public ArrowEffectManager arrowEffectManager;

}
