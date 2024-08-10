using System.Collections.Generic;

[System.Serializable]
public class SkillData
{
    public string type;
    public string name;
    public int duration;
    public string spellType;
    public float value; // 根据实际需要可以改为 int
}

[System.Serializable]
public class SkillConfig
{
    public List<SkillData> skills;
}
