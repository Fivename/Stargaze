using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player:AutosingleTon<Player>
{
    private int level;
    private int exp;
    private List<Character> characterList = new List<Character>();

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            if (value > level)
            {
                //LevelUpFunction?
                level = value;
            }
            //UpdateLevel(level);
        }
    }
    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            if (value > exp)
            {
                //LevelUpFunction?
                exp = value;
            }
            //UpdateLevel(level);
        }
    }
    public void AddCharacter(Character character)
    {
        if(characterList.Count < 3)
        characterList.Add(character);
    }
    public List<Character> GetCharacters()
    {
        return characterList;
    }
    public void SetCharacters(List<Character> charcters)
    {
        characterList = charcters;
    }
    public void SetPlayer()
    {
        Instance.level = level;
        Instance.exp = exp;
        Instance.characterList = characterList;
    }
}
