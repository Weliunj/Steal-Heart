using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObject/DataManager", order = 1)]
public class DataManager : ScriptableObject
{
    //Item
    public int Hp_bottle = 0;
    public int Jump_bottle = 0;
    public int Speed_bottle = 0;
    public int Strength_bottle = 0;

    public void Awake()
    {
        Console.WriteLine("Awake");
    }
}
