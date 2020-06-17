using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    public GameObject actionPanel;
    public GameObject quitPanel;
    public TextMeshProUGUI victorText;
    public GameObject players;
    public GameObject classes;
    public Image distanceBar;
    public TMPro.TextMeshProUGUI turnLabel;
    public GameObject atk1;
    public GameObject atk2;

    AudioSource sfx;
    AudioSource bgm;
    public AudioClip slash;
    public AudioClip bladeThrow;
    public AudioClip guard;
    public AudioClip ka;
    public AudioClip kaboom;
    public AudioClip boom;
    public AudioClip swordAtk;
    public AudioClip firstAid;
    public AudioClip words;
    public GameObject[] stages;
    public AudioClip[] stageMusic;

    PlayerController[] playerTurns;
    Image[] playerHealth;
    Transform[] playerPositions;
    PolygonCollider2D[][] colliders;
    bool battle;
    bool checkDis;
    float hp1; // player1 total HP
    float hp2; // player2 total HP
    int turnIndex;
    float initialPos;
    float waitTime;
    int stageIndex;
    List<CharacterClass> playerCharacters;
    CinemachineVirtualCamera camera;
    CinemachineConfiner confiner;

    private void Start()
    {
        bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        playerCharacters = new List<CharacterClass>();
        playerPositions = new Transform[players.transform.childCount];
        colliders = new PolygonCollider2D[playerPositions.Length][];
    }

    public void selectStage(int stage)
    {
        stageIndex = stage;
    }
    public void battleStart()
    {
        stages[stageIndex].SetActive(true);
        bgm.clip = stageMusic[stageIndex];
        bgm.Play();
        camera = Transform.FindObjectOfType<CinemachineVirtualCamera>();
        camera.m_Lens.OrthographicSize = 3;
        camera.m_Follow = playerPositions[0];
        battle = true;
        checkDis = false;
        turnIndex = 0;
        playerTurns = players.GetComponentsInChildren<PlayerController>();
        playerHealth = players.GetComponentsInChildren<Image>();
        setCharacters();
        for (int i = 0; i < playerCharacters.Count / 2; i++)
        {
            hp1 += playerHealth[i*3+1].fillAmount;
            hp2 += playerHealth[(i + playerCharacters.Count / 2)*3+1].fillAmount;
            playerHealth[i*3+1].color = new Color32(0, 85, 255, 255);
            playerHealth[(i + playerCharacters.Count / 2)*3+1].color = new Color32(255, 0, 201, 255);
            if (i != 0) // if it is not the first player, make all arrows invisible
                playerHealth[i * 3 + 2].gameObject.SetActive(false);
            playerHealth[(i + playerCharacters.Count / 2) * 3 + 2].gameObject.SetActive(false);
        }
        foreach (PlayerController pc in playerTurns)
        {
            pc.setTurn(false);
            pc.setAttack(false);
        }
        playerChoose();
    }

    void setCharacters()
    {
        TMPro.TextMeshProUGUI[] characters = classes.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI[] classLabels = players.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        for (int i = 0; i < characters.Length; i++)
        {
            playerCharacters.Add(new CharacterClass(characters[i].text));
            classLabels[i].text = characters[i].text;
        }
        int k = 0;
        foreach (Transform child in players.transform)
        {
            playerPositions[k] = child;
            colliders[k] = playerPositions[k].GetComponentInChildren<Animator>().gameObject.GetComponentsInChildren<PolygonCollider2D>();
            for (int i = 0; i < colliders[k].Length; i++)
                colliders[k][i].enabled = false;
            k++;
        }

    }
    void playerChoose()
    {
        if (turnIndex == playerPositions.Length)
        {
            turnIndex = 0;   
        }
        if (turnIndex < playerPositions.Length / 2)
            turnLabel.text = "Team 1's Turn";
        else
            turnLabel.text = "Team 2's Turn";
        camera.m_Lens.OrthographicSize = 3;
        camera.m_Follow = playerPositions[turnIndex];

        //Skip turn if hp is 0
        if (playerHealth[turnIndex * 3 + 1].fillAmount <= 0) endTurn();

        if (playerCharacters[turnIndex].skill1 == "Sword Attack" && playerCharacters[turnIndex].def > 65) // if soldier is guarded, reset its def
        {
            playerCharacters[turnIndex].def = 65;
        }
        playerHealth[turnIndex * 3 + 2].gameObject.SetActive(true);
        initialPos = playerPositions[turnIndex].position.x;
        checkDis = true;
        playerTurns[turnIndex].setTurn(true);
        atk1.GetComponentInChildren<TextMeshProUGUI>().text = playerCharacters[turnIndex].skill1;
        atk2.GetComponentInChildren<TextMeshProUGUI>().text = playerCharacters[turnIndex].skill2;
        actionPanel.SetActive(true);
    }

    private void Update()
    {
        if (checkDis)
        {
            distanceBar.fillAmount = Math.Abs(playerPositions[turnIndex].position.x - initialPos) / (playerCharacters[turnIndex].spd / 15);
            if (Math.Abs(playerPositions[turnIndex].position.x - initialPos) > (playerCharacters[turnIndex].spd / 15))
            {
                if (playerPositions[turnIndex].position.x > initialPos)
                    playerTurns[turnIndex].setMinX(true);
                else
                    playerTurns[turnIndex].setMaxX(true);
            } else
            {
                playerTurns[turnIndex].setMinX(false);
                playerTurns[turnIndex].setMaxX(false);
            }

        }
    }

    public void playerAttack1()
    {
        playerPositions[turnIndex].GetComponent<BoxCollider2D>().enabled = false;
        for (int i = 0; i < colliders[turnIndex].Length; i++)
            colliders[turnIndex][i].enabled = true;
        checkDis = false;
        actionPanel.SetActive(false);
        playerTurns[turnIndex].setAttack(true);

        playerPositions[turnIndex].GetComponentInChildren<Animator>().SetTrigger("atk1");

        if (playerCharacters[turnIndex].skill1 == "Slash")
        {
            waitTime = 2;
            sfx.clip = slash;
            colliders[turnIndex][0].enabled = false;
            sfx.PlayDelayed(0.05f);
        }
        if (playerCharacters[turnIndex].skill1 == "Boom")
        {
            playerPositions[turnIndex].GetComponentInChildren<WeaponCollision>().skillName = "Boom";
            waitTime = 2.5f;
            camera.m_Lens.OrthographicSize = 5;
            sfx.clip = boom;
            sfx.PlayDelayed(0.1f);
        }
        if (playerCharacters[turnIndex].skill1 == "Sword Attack")
        {
            waitTime = 2.2f;
            sfx.clip = swordAtk;
            sfx.PlayDelayed(0.1f);
        }
        if (playerCharacters[turnIndex].skill1 == "First Aid")
        {
            waitTime = 2;
            sfx.PlayOneShot(firstAid);
        }
        StartCoroutine(startAttack());
    }

    public void playerAttack2()
    {
        playerPositions[turnIndex].GetComponent<BoxCollider2D>().enabled = false;
        for (int i = 0; i < colliders[turnIndex].Length; i++)
            colliders[turnIndex][i].enabled = true;
        checkDis = false;
        actionPanel.SetActive(false);
        playerTurns[turnIndex].setAttack(true);

        playerPositions[turnIndex].GetComponentInChildren<Animator>().SetTrigger("atk2");

        if (playerCharacters[turnIndex].skill2 == "Blade Throw")
        {
            waitTime = 2;
            sfx.clip = bladeThrow;
            sfx.PlayDelayed(0.1f);
        }
        if (playerCharacters[turnIndex].skill2 == "Guard")
        {
            playerCharacters[turnIndex].def = 70;
            waitTime = 2;
            sfx.PlayOneShot(guard);
        }
        if (playerCharacters[turnIndex].skill2 == "Kaboom") {
            playerPositions[turnIndex].GetComponentInChildren<WeaponCollision>().skillName = "Kaboom";
            waitTime = 3.1f;
            camera.m_Lens.OrthographicSize = 5;
            sfx.PlayOneShot(ka);
            sfx.clip = kaboom;
            sfx.PlayDelayed(2f);
        }
        if (playerCharacters[turnIndex].skill2 == "Words Of Wisdom")
        {
            waitTime = 3.1f;
            camera.m_Lens.OrthographicSize = 5;
            sfx.clip = words;
            sfx.PlayDelayed(0.1f);
        }
        StartCoroutine(startAttack());
    }

    public void endTurn()
    {
        playerTurns[turnIndex].setTurn(false);
        playerTurns[turnIndex].setAttack(false);
        updateTurn();
    }

    private IEnumerator startAttack()
    {
        yield return new WaitForSeconds(waitTime+1.2f);
        hp1 = 0;
        hp2 = 0;
        //recalculate total HP in each team
        for (int i = 0; i < playerCharacters.Count / 2; i++)
        {
            hp1 += playerHealth[i * 3 + 1].fillAmount;
            hp2 += playerHealth[(i + playerCharacters.Count / 2) * 3 + 1].fillAmount;
        }
        Debug.Log("hp1:  "+ hp1 + "  hp2:  " + hp2);
        battle = (hp1 > 0 && hp2 > 0);
        if (battle)
        {
            playerPositions[turnIndex].GetComponent<BoxCollider2D>().enabled = true;
            for (int i = 0; i < colliders[turnIndex].Length; i++)
                colliders[turnIndex][i].enabled = false;
            endTurn();
        }
        else
            endGame();
        StopAllCoroutines();
    }

    private void updateTurn()
    {
        playerHealth[turnIndex * 3 + 2].gameObject.SetActive(false);
        if (turnIndex < playerPositions.Length / 2)
            turnIndex += 3;
        else
        {
            if (turnIndex == playerPositions.Length - 1)
            {
                turnIndex++;
            }
            else
            {
                turnIndex -= 2;
            }
        }
        playerChoose();
    }

    void takeDamage(int amount)
    {
        float p = amount / playerCharacters[turnIndex].hp;
        playerHealth[turnIndex].fillAmount -= p;
    }

    void endGame()
    {
        quitPanel.SetActive(true);
        GameObject.Find("Victory").GetComponent<Animator>().SetTrigger("Credits");
        if (hp1 > 0) victorText.text = "Team 1 is the victor!";
        else victorText.text = "Team 2 is the victor!";
        
        Debug.Log("End game");
    }

    public void forfeit(GameObject forfeitMessage) {
        if (turnIndex < playerPositions.Length / 2)
            forfeitMessage.GetComponentInChildren<TextMeshProUGUI>().text = "This will result in Team 2's victory.";
        else
            forfeitMessage.GetComponentInChildren<TextMeshProUGUI>().text = "This will result in Team 1's victory.";
    }
}
