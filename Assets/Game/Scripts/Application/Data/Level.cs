using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level
{
    //����
    public string Name;

    //�ؿ�ͼ��
    public string LevelIcon;

    //����
    public string Background;

    //·������
    public string Road;

    //�ɷ��õĵ�λλ��
    public List<Point> Set = new List<Point>();

    //��λ�ɽ����λ��
    public List<Point> Holder = new List<Point>();

    //�̶���λ�غ���Ϣ ����Ϊ�����ȡ����̬���ɣ�
    public List<Round> Rounds = new List<Round>();

}