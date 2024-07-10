using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarDataView : MonoBehaviour
{
    public StarEntity curStar;
    public Image icon;
    public TextMeshProUGUI hpText;
    public Slider hpSlider;
    public void Init(StarEntity star)
    {
        curStar = star;
        icon.sprite = Resources.Load<Sprite>(star.iconPath);
        hpText.text = star.curHp.ToString() + "/" + star.curMaxHp.ToString();
        hpSlider.value = star.curHp/(float)star.curMaxHp;
    }
}
