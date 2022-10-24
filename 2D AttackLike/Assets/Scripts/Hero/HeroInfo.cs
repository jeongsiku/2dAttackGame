using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClassType
{
    Knight,
    Assassin,
    Archer,
    Tactician
}

public enum AttackType
{
    Melee,
    Arrow,
    Magic
}

[System.Serializable]
public class HeroInfo
{
    public int charID;
    public CharacterClassType charClassType;
    public AttackType attackType;
    public string name;
    public int level;
    public int currentEXP;
    public int nextEXP;
    public int attackPower;
    public float attackInterval;
    public int defense = 0;
    public int currentHP;
    public int healthPower;
    public float regen = 0;
    public float luck = 0;
    public int tacticPoint;
    public int heroCost;

    public int atkUpgrageLevel = 1;
    public int speedUpgrageLevel = 1;
    public int defUpgrageLevel = 1;
    public int hpUpgrageLevel = 1;
    public int regenUpgrageLevel = 1;
    public int luckUpgrageLevel = 1;
}
