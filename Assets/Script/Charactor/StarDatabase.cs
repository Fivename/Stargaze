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
        //star�ж�ѭ�� attack -> defense ��
        Star star = new Star(1,"����","����","FirstStar",30,5);
        starList.Add(star);
    }
}
