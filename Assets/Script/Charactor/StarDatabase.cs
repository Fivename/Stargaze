using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StarDatabase
{
    public static List<Star> starList = new List<Star>();
    public static void Init()
    {
        starList.Clear();
        //star行动循环 attack -> defense 等
        Star star = new Star(1,"五米","测试","FirstStar",30,5);
        starList.Add(star);
    }
}
