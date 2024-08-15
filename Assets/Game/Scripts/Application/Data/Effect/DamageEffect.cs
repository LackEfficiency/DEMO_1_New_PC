using System.Collections;
using UnityEngine;

//伤害效果均通过该类实现, 回血效果是负伤害
public class DamageEffect : Effect
{
    //伤害值
    private int DamageValue;

    public DamageEffect(string effectName, string effectDescription, int damageValue) : base(effectName, effectDescription)
    {
        DamageValue = damageValue;
    }

    //对目标造成伤害需要传入目标卡牌对象
    public override void Cast(MonsterCard card)
    {
        if (card == null)
            return;
        card.Damage(null, DamageValue);
    }
}