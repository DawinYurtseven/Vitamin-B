using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public enum Topics
{
    Business
}

public enum NoTopics
{
    Bitcoin,
    NFTs,
    Blockchain,
    AI,
    Crypto,
    Microtransactions,
    Battlepass,
    Lifeservice,
    Publisher,
    Investors
}

public class SpeechBubbleController : MonoBehaviour
{
    [SerializeField]
    private float textFillSpeed = 0.5f;

    private bool keywordSpawned = false;
    private int wordCount = 0;
    private int wordMax = 8;
    private int NoTopicsLength = Enum.GetNames(typeof(NoTopics)).Length;
    
    private TMP_Text content;
    private String targetName;
   
    

    private void Awake()
    {
        //_bubbleContent.FillBubble();
    }
    
    IEnumerator FillBubbleDelayed()
    {
        if (wordCount > wordMax)
        {
            content.text = "";
            wordCount = 0;
            keywordSpawned = false;
        }

        if (Random.Range(0 + wordCount, wordMax) == wordMax && !keywordSpawned)
        {
            string keyword = "";
            if (Random.Range(0, 3) == 3)
            {
                keyword = targetName;
            }
            else
            {
                keyword = Enum.GetName(typeof(NoTopics), Random.Range(0, NoTopicsLength - 1));
            }
            keywordSpawned = true;
        }
        else
        {
            content.text.Concat("Bla ");
        }
        
        wordCount++;
        yield return new WaitForSeconds(textFillSpeed);
        StartCoroutine(FillBubbleDelayed());
    }
}
