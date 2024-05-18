using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : Singleton<StaticData>
{
    Dictionary<int, BuildingInfo> m_Buildings = new Dictionary<int, BuildingInfo>();

    protected override void Awake()
    {
        base.Awake();

        InitBuildings();
    }

    void InitBuildings()
    {
        m_Buildings.Add(0, new BuildingInfo()
        {
            ID = 0,
            PrefabName = "Init",
        });
        m_Buildings.Add(1, new BuildingInfo()
        {
            ID = 1,
            PrefabName = "Chest",
        });
        m_Buildings.Add(2, new BuildingInfo()
        {
            ID = 2,
            PrefabName = "Battle",
        });
        m_Buildings.Add(3, new BuildingInfo()
        {
            ID = 3,
            PrefabName = "Shop",
        });
        m_Buildings.Add(4, new BuildingInfo()
        {
            ID = 4,
            PrefabName = "Boss",
        });
        m_Buildings.Add(5, new BuildingInfo()
        {
            ID = 5,
            PrefabName = "Exit",
        });
        m_Buildings.Add(6, new BuildingInfo()
        {
            ID = 6,
            PrefabName = "Chance",
        });
        m_Buildings.Add(7, new BuildingInfo()
        {
            ID = 7,
            PrefabName = "Path",
        });
    }

    public BuildingInfo GetBuildingInfo(int BuildingID)
    {
        return m_Buildings[BuildingID];
    }


}
