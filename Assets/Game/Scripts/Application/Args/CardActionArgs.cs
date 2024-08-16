using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionArgs
{
    public MonsterCard self;
    public MonsterCard target;
    public List<MonsterCard> selfCardsOnField = new List<MonsterCard>();
    public List<MonsterCard> EnemyCardsOnField = new List<MonsterCard>();

    public CardActionArgs(MonsterCard self, MonsterCard target, List<MonsterCard> selfCardsOnField, List<MonsterCard> EnemyCardsOnField)
    {
        this.self = self;
        this.target = target;
        this.selfCardsOnField = selfCardsOnField;
        this.EnemyCardsOnField = EnemyCardsOnField;
    }
}