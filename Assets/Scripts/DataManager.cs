using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObject/DataManager", order = 1)]
public class DataManager : ScriptableObject
{
    //Item
    [Header("           -------------- Items ---------------")]
    [Range(0, 999)]public int Coin_Quan = 0;
    public int Hp_bottle = 0;
    public int Jump_bottle = 0;
    public int Speed_bottle = 0;
    public int Strength_bottle = 0;

    [Header("           -------------- Load Scene ---------------")]
    public int prev = 0;
    public int next = 1;
    public void Awake()
    {
        Console.WriteLine("Awake");
    }
}
