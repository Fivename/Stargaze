﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class Save
{
    public static void SaveData(System.Object obj,string path)
    {
        string json = JsonMapper.ToJson(obj);
        File.WriteAllText(path, json, Encoding.UTF8);
    }
    public static T LoadData<T>(string loadJson)
    {
        string playerText = File.ReadAllText(loadJson);
        return JsonMapper.ToObject<T>(playerText);
    }
}
