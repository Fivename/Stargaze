using UnityEngine;
using R3;
using TMPro;
using System;
public class ButtonTest : MonoBehaviour
{
    public GameObject worldGo;
    // Start is called before the first frame update
    void Start()
    {
        Observable.EveryUpdate()
             .Where(_ => Input.GetKeyDown(KeyCode.A))
             .Subscribe(_ => TestAction());
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => 
                {
                    Debug.Log(Input.mousePosition);
                }
            );
        Vector3 screenPos = new Vector3();
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.D))
            .Subscribe(_ =>
            {
                screenPos.Set(Input.mousePosition.x,Input.mousePosition.y,-Camera.main.transform.position.z);
                Debug.Log(screenPos);
                Debug.Log(worldGo.transform.position);
                Debug.Log(Camera.main.WorldToScreenPoint(worldGo.transform.position));
                //Debug.Log(Camera.main.ScreenToWorldPoint(screenPos));
            }
            );

    }
    public TMP_InputField mInput;
    public TMP_Text Text;
    #region Fun-Action Test
    Func<int,int> tFun;
    Action tAc;
    Action<string> tAc2;
    private int PlusOne(int one)
    {
        return  one++;
    }
    private void PlusOne(string a="1")
    {
        return;
    }
    private void PlusOne()
    {
        return;
    }
    #endregion
    private Vector3 mousePos;

    private void Update()
    {
        //mousePos = Input.mousePosition;
        //Debug.Log(mousePos);
    }
    private void TestAction()
    {
        tFun += PlusOne;
        tAc += PlusOne;
        tAc2 += PlusOne;
        //mInput.OnValueChangedAsObservable()
        //.Where(x => x != null)
        //.SubscribeToText(Text);
        //using 
        GCTest gc = new GCTest();
        People p = new Student();
        p.Say();
        People p1 = new Js();
        p1.Say();
        Student s = new Js();
        s.StuSay();
        s.Say();
        GC.Collect();
    }
    interface IFlyable
    {
        public void Fly();
    }
    abstract class Animal
    {
        public void run()
        {

        }
        private void Fly()
        {

        }
        public abstract void Burn();
    }
    public class GCTest : IDisposable
    {

        public int a;
        ~GCTest()
        {
            Debug.Log("gc!");
        }
        public void Dispose()
        {
            Debug.Log("Dispose!");
        }
    }
}
public static class StringExtension
{
    public static int GetLines(this int input)
    {
        return 1;
    }
}
public class People
{
    private int prI=10;
    public int puI;
    public void Say()
    {
        Debug.Log("People say:" + prI);
    }
}
class Student : People
{
    private int stuPr;
    public int stuPu;
    public Student()
    {
        puI = 10;
    }
    public void StuSay()
    {

    }
}
class Js : Student
{
    private int JsPr;
    public int JsPu;
    public void JsSay()
    {

    }
}
