using System.Collections;
using UnityEngine;

//所有效果的基类，包含魔法卡效果和召唤物技能效果
public abstract class Effect
{ 
    private string effectName;
    private int effectValue;
    private string effectDescription;

    public string EffectName { get => effectName; set => effectName = value; }
  
    public string EffectDescription { get => effectDescription; set => effectDescription = value; }
    public int EffectValue { get => effectValue; set => effectValue = value; }

    public abstract void Cast(MonsterCard card);

}