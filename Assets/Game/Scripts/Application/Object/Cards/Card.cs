using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    private Vector3 nextDes; //保存下个移动位置
    bool m_IsCardMoving; //判断是否需要移动
  

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
    #endregion

    #region 方法
    public void PositionCHange(Card card)
    {

    }

    void MoveTo(Vector3 dest)
    {
        transform.position = dest;
    }

    public void Move(Vector3 dest)
    {
        //获取当前位置
        Vector3 pos = transform.position;

        //计算距离
        float dis = Vector3.Distance(pos, dest);

        if (dis < CLOSED_DISTANCE)
        {
            MoveTo(dest);
            IsCardMoving = false;
        }
        else
        {
            //计算移动方向
            Vector3 direction = (dest - pos).normalized;

            //平滑移动(米/帧 = 米/秒 * Time.deltaTime)
            transform.Translate(direction * MOVE_SPEED * Time.deltaTime);
        }
    }
    #endregion

    #region Unity回调
    private void Update()
    {
        if (IsCardMoving)
        {
            Move(NextDes);
        }
    }
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
        this.CardPosistionChange += PositionCHange;
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
