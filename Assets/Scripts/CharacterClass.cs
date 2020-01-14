using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass
{
    public string skill1, skill2;
    public int hp, atk, def, spAtk, spDef, spd;

    public CharacterClass(string className)
    {
        switch (className)
        {
            case "Assassin":
                skill1 = "Slash";
                skill2 = "Blade Throw";
                hp = 65;
                atk = 85;
                def = 35;
                spAtk = 45;
                spDef = 45;
                spd = 90;
                break;
            case "Hero":
                skill1 = "Sword Attack";
                skill2 = "Guard";
                hp = 80;
                atk = 70;
                def = 65;
                spAtk = 45;
                spDef = 45;
                spd = 60;
                break;
            case "Mage":
                skill1 = "Boom";
                skill2 = "Kaboom";
                hp = 65;
                atk = 30;
                def = 45;
                spAtk = 80;
                spDef = 65;
                spd = 65;
                break;
            case "Scholar":
                skill1 = "First Aid";
                skill2 = "Words Of Wisdom";
                hp = 100;
                atk = 50;
                def = 50;
                spAtk = 55;
                spDef = 50;
                spd = 55;
                break;
            default:
                skill1 = "None";
                skill2 = "None";
                hp = 0;
                atk = 0;
                def = 0;
                spAtk = 0;
                spDef = 0;
                spd = 0;
                break;
        }
    }

    public static int getStats(string className, string stat)
    {
        CharacterClass unit = new CharacterClass(className);
        switch (stat) {
            case "hp":
                return unit.hp;
            case "atk":
                return unit.atk;
            case "def":
                return unit.def;
            case "spAtk":
                return unit.spAtk;
            case "spDef":
                return unit.spDef;
            case "spd":
                return unit.spd;
            default:
                return -1; //invalid stat
        }
    }
}
