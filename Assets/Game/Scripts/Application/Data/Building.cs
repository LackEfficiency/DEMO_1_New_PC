using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class Building : Reusable, IReusable
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //所在Room
    Room m_Room;
    #endregion

    #region 属性
    public int ID { get; private set; }
    #endregion

    #region 方法
    public void Load(int buildingID, Room room)
    {
        BuildingInfo info = Game.Instance.StaticData.GetBuildingInfo(buildingID);
        this.ID = info.ID;
        this.m_Room = room;
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void OnSpawn()
    {
    
    }

    public override void OnUnspawn()
    {
        //还原脏数据
        ID = -1;
        m_Room = null;  
    }
    #endregion

    #region 帮助方法
    #endregion

}
