using System.Collections;
using UnityEngine;

//所有效果的基类，包含魔法卡效果和召唤物技能效果
public abstract class Effect
{ 
    private string effectName;
    private string effectDescription;

    protected Effect(string effectName, string effectDescription)
    {
        this.effectName = effectName;
        this.effectDescription = effectDescription;
    }

    public string EffectName { get => effectName; set => effectName = value; }
  
    public string EffectDescription { get => effectDescription; set => effectDescription = value; }

    public abstract void Cast(MonsterCard card);

}