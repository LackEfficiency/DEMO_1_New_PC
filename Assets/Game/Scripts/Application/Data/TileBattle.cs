using System;
using System.Collections.Generic;
using UnityEngine;


//ս������ ������Ϣ
public class TileBattle : Tile
{
    public bool CanSetMe; //�Ƿ���Է��õ�λ
    public bool CanSetEnemy; //�����Ƿ���Է�ֹ��λ
    public bool CanHold; //��λ�Ƿ���Խ���
    public GameObject Card; //���ӱ���Ŀ�����Ϣ 

    public TileBattle(int x, int y):base(x, y)
    {
        X = x;
        Y = y;
    }
}