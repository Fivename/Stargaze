using System;
using System.Collections.Generic;

public class EventTriggerManager
{
    private static EventTriggerManager instance;
    public static EventTriggerManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    public static List<Action> actionList = new List<Action>();
    public static void AddEvent(string actionName,string eventName)
    {

    }
    public static void AddAction(Action action)
    {
        if (!actionList.Contains(action))
        {

        }
    }
}
