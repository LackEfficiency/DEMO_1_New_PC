using System;
using System.Collections.Generic;

[Serializable]
public class BuffData
{
    public string type;
    public string name;
    public int duration;
    public bool stackable;
    public float value;
}

[Serializable]
public class BuffConfig
{
    public List<BuffData> buffs;
}
