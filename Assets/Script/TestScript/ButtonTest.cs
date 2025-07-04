using UnityEngine;
using R3;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.Events;
using R3.Triggers;
using System.Linq;
using Unity.VisualScripting;
using OfficeOpenXml;
public class ButtonTest : MonoBehaviour
{
    public GameObject worldGo;
    public Image icon;
    // Start is called before the first frame update
    private string ABPath = Application.streamingAssetsPath + "/PC";
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    public int testA = 10;
    public List<int> list = new List<int>();
    public string readPath;
    public string readPath2;
    public string readPath3;
    public string comparedPath;
    public string intersectionPath;
    public string drawPath;
    public int judgeRow;
    IEnumerator LateAwake(UnityAction<int> action)
    {
        action(list.Count);
        yield return new WaitForSeconds(2);
        action(list.Count);
    }
    private void Awake()
    {
        Debug.Log("awakeCall");
    }

    private void OnApplicationQuit()
    {
        
    }
    void Start()
    {
        ExcelPackage.License.SetNonCommercialPersonal("MyTest");
        Debug.Log("start call");
        //a要加载一个AB包-》需要？
        //1、需要加载该AB包的所有依赖包
        //2、需要加载主包来获取依赖信息
        //3、根据依赖信息获取依赖包信息
        //4、正式加载依赖包
        //5、遍历加载主包
        //
        //Debug.Log(Application.persistentDataPath);
        //var ab = AssetBundle.LoadFromFile(ABPath);//获取主包

        //var ab = AssetBundle.LoadFromFile(ABPath);
        //var mainAb = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //mainAb.GetAllDependencies(abName);
        int a = 1, b = 2, c = 3;
        list.Add(a);
        list.Add(b);
        //加载主包
        //加载依赖包
        //加载包


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
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.R))
            .Subscribe(_ =>
            {
                ///三个文件组合
                //HashSet<string> hashSet = new HashSet<string>(ExcelRW.EppReadExcel(readPath));
                //HashSet<string> hashSet2 = new HashSet<string>(ExcelRW.EppReadExcel(readPath2)); 
                //HashSet<string> hashSet3 = new HashSet<string>(ExcelRW.EppReadExcel(readPath3));
                var tuple = ExcelRW.EppReadExcel(readPath);
                List<string> readList = tuple.Item1;
                ExcelRW.readDic = tuple.Item2;
                //List<string> readList2 = ExcelRW.EppReadExcel(readPath2);
                //List<string> readList3= ExcelRW.EppReadExcel(readPath3);
                //readList.AddRange(readList2);
                //readList.AddRange(readList3);
                readList = readList.Distinct<string>().ToList();

                //var unionSet = hashSet.Union<string>(hashSet2.Union<string>(hashSet3));
                //ExcelRW.readFile = unionSet.ToList<string>();
                ExcelRW.readFile = readList;
                ExcelRW.compareFile = ExcelRW.EppReadExcel(comparedPath).Item1;
            }
            );
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.W))
            .Subscribe(_ =>
            {
                if (ExcelRW.readFile == null || ExcelRW.compareFile == null || ExcelRW.readFile.Count == 0 || ExcelRW.compareFile.Count==0)
                {
                    Debug.Log("comfirm the file or read file first!");
                    return;
                }
                //List<string> mergedList = new List<string>();
                ////mergedList = ExcelRW.GetIntersectionList(ExcelRW.readFile, ExcelRW.compareFile);//
                //mergedList = ExcelRW.readFile.Except<string>(ExcelRW.compareFile).ToList();
                //ExcelRW.WriteOrCreateExcel(intersectionPath,mergedList);
                ExcelRW.DrawExcel(readPath,drawPath, ExcelRW.compareFile, judgeRow);
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
        StartCoroutine(LateAwake(A => 
        {
            Debug.Log("now A is" + A);
            
        }
        ));
        int c = 3;
        list.Add(c);
        //mInput.OnValueChangedAsObservable()
        //.Where(x => x != null)
        //.SubscribeToText(Text);
        //using 
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
            string str;
            int a=1;
            a.GetLines();
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

