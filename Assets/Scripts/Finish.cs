using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    List<string> allAINames;
    List<int> usedIndexs;
    string[] randomNames = new string[6];

    UIManager uIManager;
    GameManager gameManager;
    Movement movement;

    private void Awake()
    {
        movement = FindObjectOfType<Movement>();
        gameManager = FindObjectOfType<GameManager>();
        uIManager = FindObjectOfType<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FinishGame()
    {
        uIManager.finishPanel.SetActive(true);

        SetRandomBotFeets();

        GetRandomNames();

        WriteRandomNames();
    }

    private void SetRandomBotFeets()
    {
        List<int> randomFeets = new List<int>();

        for (int i = 0; i < uIManager.feetTexts.Length; i++)
        {
            randomFeets.Add(UnityEngine.Random.Range(0, (int)(movement.score)));
        }

        randomFeets.Sort();
        randomFeets.Reverse();

        uIManager.feetTexts[0].text = ((int)movement.score).ToString() + " ft";

        for (int i = 1; i < uIManager.feetTexts.Length; i++)
        {
            uIManager.feetTexts[i].text = randomFeets[i].ToString() + " ft";
        }
    }

    private void WriteRandomNames()
    {
        uIManager.nameTexts[0].text = gameManager.playerName;

        for (int i = 1; i < uIManager.nameTexts.Length; i++)
        {
            uIManager.nameTexts[i].text = randomNames[i - 1];
        }
    }

    private void GetRandomNames()
    {
        allAINames = new List<string>();
        usedIndexs = new List<int>();

        foreach (var item in Enum.GetValues(typeof(AINames)))
        {
            allAINames.Add(item.ToString());
        }

        for (int i = 0; i < randomNames.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allAINames.Count);

            if (!usedIndexs.Contains(randomIndex))
            {
                randomNames[i] = allAINames[randomIndex];
                usedIndexs.Add(randomIndex);
            }
            else
            {
                i--;
            }
        }
    }
}
