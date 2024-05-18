using System;
using System.Collections.Generic;
using UnityEngine;

public class SummonCardRequestArgs
{
    public CardInfo cardInfo; 
    public GameObject go;
    public Player player; //0为已方，1为敌方 
    //TODO:暂时只用0，后续再改
}