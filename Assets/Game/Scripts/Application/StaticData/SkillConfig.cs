using System.Collections.Generic;

[System.Serializable]
public class SkillData
{
    public string type;
    public string name;
    public int duration;
    public string spellType;
    public float value; 
}

[System.Serializable]
public class SkillConfig
{
    public List<SkillData> skills;
}
