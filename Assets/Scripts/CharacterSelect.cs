using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelect : MonoBehaviour
{
    private UI_Stats uiStats;

    [SerializeField] GameObject player;
    public GameObject label;
    GameObject[] characters;
    string[] classes;
    string charPath = "Characters";
    int i;
    GameObject sel;
    GameObject unit;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Stats") != null)
        {
            uiStats = GameObject.Find("Stats").GetComponent<UI_Stats>();
            Stats stats = new Stats(65, 85, 35, 90, 45, 45); // Assassin Stats loaded by def
            uiStats.SetStats(stats);
        }

        i = 0;
        characters = Resources.LoadAll<GameObject>(charPath);
        classes = new string[]{ "Assassin","Hero", "Mage", "Scholar"};
        label.GetComponent<TMPro.TextMeshProUGUI>().text = classes[i];
        sel = Instantiate(characters[i], transform);
        if (player != null) { unit = Instantiate(characters[i], player.transform); }
    }

    public void nextCharacter()
    {
        if (i == characters.Length - 1)
            i = 0;
        else
            i++;
        if(sel != null)
        {
            Destroy(sel);
            Destroy(unit);
        }
        sel = Instantiate(characters[i], transform);
        if (player != null) { unit = Instantiate(characters[i], player.transform); }        
        label.GetComponent<TMPro.TextMeshProUGUI>().text = classes[i];
    }

    public void prevCharacter()
    {
        if (i == 0)
            i = characters.Length - 1;
        else
            i--;
        if (sel != null)
        {
            Destroy(sel);
            Destroy(unit);
        }
        sel = Instantiate(characters[i], transform);
        if (player != null) { unit = Instantiate(characters[i], player.transform); }
        label.GetComponent<TMPro.TextMeshProUGUI>().text = classes[i];
    }

    public void setSkill()
    {
        string warrior = label.GetComponent<TMPro.TextMeshProUGUI>().text;
        if (GameObject.Find("SkillsText") != null)
        {
            GameObject skillText = GameObject.Find("SkillsText");
            if (warrior == "Hero")
            {

                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "SWORD ATTACK:\nA powerful strike to a single foe\n" +
                    "\nGUARD:\nRaises DEF for next turn";
            }
            if (warrior == "Mage")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "BOOM:\nAttacks a foe from a distance with strong magic\n" +
                    "\nKA-BOOM:\nA powerful range-attack on all warriors in an area.";
            }
            if (warrior == "Assassin")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "SLASH:\nClose-up attack on a single foe\n" +
                    "\nBLADE THROW:\nAttacks a foe from a distance";
            }
            if (warrior == "Scholar")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "FIRST AID:\nHeals a nearby ally\n" +
                    "\nWORDS OF WISDOM:\nAttacks a foe from a distance with words of wisdom";
            }
        }
    }

    public void setAbilities()
    {
        string warrior = label.GetComponent<TMPro.TextMeshProUGUI>().text;
        if (GameObject.Find("AbilitiesText") != null)
        {
            GameObject skillText = GameObject.Find("AbilitiesText");
            if (warrior == "Hero")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "AGILITY:\nSPEED increases when hit with an ATK\n" +
                    "\nFOREST EXPERT:\nDEF increases in Forest battlefields";
            }
            if (warrior == "Mage")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "INNER PEACE:\nSPA increases when HP is below 50%\n" +
                    "\nDIVINE PROTECTION:\nIncreases nearby allies' SPD";
            }
            if (warrior == "Assassin")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "DARK AURA:\nATK increases when hit with SPA\n" +
                    "\nCAMOUFLAGE:\nSPD increases in City battlefields";
            }
            if (warrior == "Scholar")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "RESEARCH:\nIncreases nearby allies' SPD and DEF\n" +
                    "\nANALYZE:\nIncreases ATK and SPA every turn";
            }
        }
    }

    public void setStats()
    {
        string warrior = label.GetComponent<TMPro.TextMeshProUGUI>().text;
        if (GameObject.Find("StatsText") != null)
        {
            GameObject statsText = GameObject.Find("StatsText");
            if (warrior == "Hero")
            {
                Stats stats = new Stats(80, 70, 65, 60, 45, 45); // Hp, Atk, Def, Speed, SpD, SpA
                uiStats.SetStats(stats);
            }
            if (warrior == "Mage")
            {
                Stats stats = new Stats(65, 30, 45, 65, 65, 80); // Hp, Atk, Def, Speed, SpD, SpA
                uiStats.SetStats(stats);
            }
            if (warrior == "Assassin")
            {
                Stats stats = new Stats(65, 85, 35, 90, 45, 45); // Hp, Atk, Def, Speed, SpD, SpA
                uiStats.SetStats(stats);
            }
            if (warrior == "Scholar")
            {
                Stats stats = new Stats(100, 50, 50, 55, 50, 55); // Hp, Atk, Def, Speed, SpD, SpA
                uiStats.SetStats(stats);
            }
        }
    }

    public void setFacts()
    {
        string warrior = label.GetComponent<TMPro.TextMeshProUGUI>().text;
        if (GameObject.Find("FactsText") != null)
        {
            GameObject skillText = GameObject.Find("FactsText");
            if (warrior == "Hero")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "Name: Boots\nHobby: Riding bikes, Sparring tournaments & Jumping Jacks\n\nFavourite quote:\nTo see what is right and not do it is a lack of courage.";
            }
            if (warrior == "Mage")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "Name: Torty\nHobby: Fishing, Playing games & Reading spellbooks\n\nFavourite quote:\nOne can have no smaller or greater mastery than mastery of oneself.";
            }
            if (warrior == "Assassin")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "Name: Zeno\nHobby: Climbing trees, Chasing cats & Cooking\n\nFavourite quote:\nGreat things are not done by impulse, but by a series of small things brought together.";
            }
            if (warrior == "Scholar")
            {
                skillText.GetComponent<TMPro.TextMeshProUGUI>().text = "Name: Gray\nHobby: Museum hopping, Reading & Writing fanfictions\n\nFavourite quote:\nAll knowledge the world has received comes from an infinite library - our minds.";
            }
        }
    }
}
