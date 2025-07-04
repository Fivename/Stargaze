using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using UnityEngine.UI;
using TMPro;
public class DrawTest : MonoBehaviour
{
    public GameObject drawPre;
    public GameObject drawPlace;
    public GameObject drawPoint;
    public GameObject drawPoint2;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI miss;
    public Slider armorSlider;
    public Slider missSlider;
    private float armorChangeToPosX = 10;
    private float armorBlockChangeToPosY = 500;
    private float sliderRangeChangeToPosX = 1600;
    private float sliderRangeChangeToPosY = 160;
    public float armorValue;
    public float missRate=0;
    public void Start()
    {
        GameObject go;
        for (int armor = -80; armor <= 80; armor++)
        {
            go = Instantiate(drawPre, drawPlace.transform);
            go.transform.localPosition = new Vector2(armor * armorChangeToPosX, armorBlockChangeToPosY * GeTY(armor)  );//i = 1 => (10,f(10))=
            go.name = "Point-" + armor;
        }
        //Observable.EveryUpdate()
        //    .Where(_ => Input.GetKeyDown(KeyCode.D))
        //    .Subscribe(_ => Draw());
        armorSlider.OnValueChangedAsObservable()
            .Subscribe(v => 
            {
                armorValue = v;
                Draw(); 
            })
            .AddTo(this);
        missSlider.OnValueChangedAsObservable()
            .Subscribe(v =>
            {
                missRate = v;
                Draw();
            })
            .AddTo(this);
    }
    public void Draw()
    {
        Vector2 vec = new Vector2((armorValue - 0.5f)* sliderRangeChangeToPosX, armorBlockChangeToPosY * GeTY((armorValue - 0.5f) * sliderRangeChangeToPosY));
        drawPoint.transform.localPosition = vec;
        armor.text = (vec.x / armorChangeToPosX).ToString("f2");
        text.text = string.Format("({0},{1})", armor.text, (vec.y/armorBlockChangeToPosY).ToString("f3"));
        //missDraw
        Vector2 vec2 = new Vector2((armorValue - 0.5f) * sliderRangeChangeToPosX, armorBlockChangeToPosY * GetMissA(GeTY((armorValue - 0.5f) * sliderRangeChangeToPosY), missRate));
        drawPoint2.transform.localPosition = vec2;
        miss.text = missRate.ToString("f2");
        text2.text = string.Format("({0},{1})", (vec.x / armorChangeToPosX).ToString("f2"), (GetMissA(vec.y / armorBlockChangeToPosY,missRate)).ToString("f3"));
    }
    public static float GeTY(float x)
    {
        return (0.06f * x) / (1f + 0.06f * Mathf.Abs(x));
    }
    public static float GetMissA(float armorBlockRate,float missRate)
    {
        return (1-(1 - armorBlockRate) * (1 - missRate));
    }
}
public struct LineData
{
    public float Y;
    public float X;
    public static float GeTY(float x)
    {
        return (0.06f * x) / (1 + 0.06f * Mathf.Abs(x));
    }
}
public class HeroData
{
    public string heroName;
    public float armor;
    public HeroData()
    {
        
    }
}