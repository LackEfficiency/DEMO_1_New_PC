using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.PlayerSettings;

public class Card: Reusable, IReusable
{
    #region 常量
    public const float CLOSED_DISTANCE = 0.1f; //低于该距离默认到达
    public const float MOVE_SPEED = 2f;
    #endregion

    #region 事件
    public event Action<Card> CardPosistionChange; //卡牌位置移动时(从手牌离开) 回收对象
    #endregion

    #region 字段
    public Player player = Player.Self; //判断召唤者是已方还是敌方
    bool m_PosState; //检测卡牌位置是否变化(从手牌移动到场上，场上移动到墓地），若为true，则发生变化
    private int m_Counter = 0; //在deckmanager场景中检测卡牌数量

    private Vector3 curPos; //保存当前位置
    private Vector3 nextDes; //保存下个移动位置
    private Card target; //保存攻击目标 

    bool m_IsCardReturn = false; //判断是否正在返回
    bool m_IsCardMoving = false; //判断是否需要移动
    bool m_IsCardAttacking = false; //判断是否需要攻击  

  

    #endregion

    #region 属性
    //卡牌位置是否发生变化
    public bool PosState
    {
        get { return m_PosState; }
        set
        {
            m_PosState = value;
            if (m_PosState == true)
            {
                CardPosistionChange?.Invoke(this);
            }
        }
    }

    public int Counter
    {
        get => m_Counter;
        set
        {
            m_Counter += value;
            if (m_Counter == 0)
            {
                CardPosistionChange?.Invoke(this);
            }
        }
    }

    public bool IsCardMoving 
    { 
        get => m_IsCardMoving; 
        set => m_IsCardMoving = value; 
    }


    public Vector3 NextDes 
    {
        get => nextDes; 
        set => nextDes = value; 
    }

    public Player Player 
    { 
        get => player; 
        set => player = value; 
    }

    public bool IsCardAttacking 
    { 
        get => m_IsCardAttacking; 
        set => m_IsCardAttacking = value; 
    }

    public Card Target 
    { 
        get => target; 
        set => target = value; 
    }
    public Vector3 CurPos 
    { 
        get => curPos; 
        set => curPos = value; 
    }
    public bool IsCardReturn 
    { 
        get => m_IsCardReturn; 
        set => m_IsCardReturn = value; 
    }
    #endregion

    #region 方法
    public void PositionChange(Card card)
    {

    }

    void MoveTo(Vector3 dest)
    {
        transform.position = dest;
    }

    //移动逻辑, 平滑移动
    public void Move()
    {
        //获取当前位置
        Vector3 pos = transform.position;

        //计算距离
        float dis = Vector3.Distance(pos, NextDes);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(NextDes);
            IsCardMoving = false;
        }
        else
        {
            //计算移动方向
            Vector3 direction = (NextDes - pos).normalized;

            //平滑移动(米/帧 = 米/秒 * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }
    }

    //攻击逻辑，攻击动画
    //TODO: 攻击动画
    public void Attack() //传入攻击目标的位置和当前的位置
    {
        //这里暂时先用移动动作代替攻击动作
        //移动到目标位置后返回当前位置

        //获取当前位置
        Vector3 pos = transform.position;

        //计算距离
        float dis = Vector3.Distance(pos, NextDes);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(NextDes);
            NextDes = CurPos;
            //第一次抵达后把目标位置设为初始位置，同时把正在返回设置为true
            //第二次抵达后把正在返回设置为false，同时正在攻击设置为false
            if (IsCardReturn == true)
            {
                IsCardAttacking = false;
                IsCardReturn = false;
            }
            IsCardReturn = true;
        }
        else
        {
            //计算移动方向
            Vector3 direction = (NextDes - pos).normalized;

            //平滑移动(米/帧 = 米/秒 * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }


    }   
    #endregion

    #region Unity回调
    //动画控制
    private void Update()
    {
        if (IsCardMoving)
        {
            Move();
        }

        if (IsCardAttacking)
        {
            Attack();
        }
    }
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        this.CardPosistionChange += PositionChange;
        this.PosState = false;
    }

    public override void OnUnspawn()
    {
        //还原脏数据
        m_Counter = 0;
        m_PosState = false;
        while (CardPosistionChange != null)
        {
            CardPosistionChange -= CardPosistionChange;
        }
    }
    #endregion

    #region 帮助方法
    #endregion
}
