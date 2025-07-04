using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDataView : MonoBehaviour
{
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI energy;
    [SerializeField] private TextMeshProUGUI icon;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private Slider slider;
    
    void Init(BatCharacter data)
    {

        UpdateData(data);
    }
    public void UpdateData(BatCharacter data)
    {
        name.text = data.name.ToString();
        energy.text = data.energy.ToString() + "/" + data.maxEnergy.ToString();
        hp.text = data.curHp.ToString()+ "/" + data.maxHp.ToString();
        slider.value = (float)data.curHp / data.maxHp;
    }
}
