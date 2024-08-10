using System.Collections.Generic;

[System.Serializable]
public class EffectData
{
    public string type;
    public string name;
    public string description;
    public int value;
}

[System.Serializable]
public class EffectConfig
{
    public List<EffectData> effects;
}