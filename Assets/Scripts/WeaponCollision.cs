using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCollision : MonoBehaviour
{
    public static bool die;
    public string skillName;
    bool touched;
    private GameObject player;

    Dictionary<string, int> skillDmg = new Dictionary<string, int>()
                                            {
                                                {"Slash", 60},
                                                {"Blade Throw", 45},
                                                {"Sword Attack", 50},
                                                {"Guard", 0},
                                                {"Boom", 60},
                                                {"Kaboom", 45},
                                                {"First Aid", -35},
                                                {"Words Of Wisdom", 50}
                                            };

    Dictionary<string, string> skillType = new Dictionary<string, string>()
                                            {
                                                {"Slash", "Physical"},
                                                {"Blade Throw", "Physical"},
                                                {"Sword Attack", "Physical"},
                                                {"Guard", "Physical"},
                                                {"Boom", "Special"},
                                                {"Kaboom", "Special"},
                                                {"First Aid", "Special"},
                                                {"Words Of Wisdom", "Special"}
                                            };
    void OnTriggerEnter2D(Collider2D collision)
    {
        Image[] health = collision.GetComponentsInChildren<Image>();
        
        TMPro.TextMeshProUGUI classLabel = collision.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        
        if (health != null)
        {
            float damage = 0;
            damage += skillDmg[skillName];

            if (damage > 0) // then we factor atk/def or sp. atk/sp. def
            {
                CharacterClass unit = new CharacterClass(classLabel.text);
                if (skillType[skillName] == "Physical")
                {
                    float atkBuff = (float)unit.atk/(float)100;
                    damage += damage * atkBuff;
                    damage -= (float)unit.def;
                }
                else if (skillType[skillName] == "Special")
                {
                    float atkBuff = (float)unit.spAtk / (float)100;
                    damage += damage*atkBuff;
                    damage -= (float)unit.spDef;
                }
            }
            
            health[1].fillAmount -= (damage/100);
            if (health[1].fillAmount <= 0 && !touched)
            {
                GameObject.Find("DieSFX").GetComponent<AudioSource>().Play();
                collision.GetComponentInChildren<Animator>().SetTrigger("die");
                player = collision.gameObject;
                Invoke("Die", 1);
            }
            touched = true;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        touched = false;
    }

    void Die()
    {
        player.SetActive(false);
        die = true;
    }
}
