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
    
    
    
    
    private TMP_Text content;
    private String targetName;
    private int wordCount = 0;
    private int wordMax = 8;
    private int NoTopicsLength = Enum.GetNames(typeof(NoTopics)).Length;

    private void Awake()
    {
        //_bubbleContent.FillBubble();
    }
    
    IEnumerator FillBubbleDelayed()
    {
        if (wordCount >= wordMax)
            content.text = "";
        
        if(Random.Range(0, wordMax) == wordMax)
            content.text.Append<>("Bla ");
        wordCount++;
        yield return new WaitForSeconds(textFillSpeed);
        FillBubbleDelayed();
    }
}
