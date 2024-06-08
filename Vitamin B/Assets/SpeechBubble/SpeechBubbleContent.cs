using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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

public class SpeechBubbleContent : MonoBehaviour
{
    private TMP_Text content;
    private String targetName;
    private int wordMax = 8;
    private int NoTopicsLength = Enum.GetNames(typeof(NoTopics)).Length;
    
    /*public void FillBubble()
    {
        int words = 0;
        for (; words < Random.Range(0, wordMax); words++)
        {
            content.text.Append<>("Bla ");
        }

        if (Random.Range(0,3) == NoTopicsLength)
        {
            content.text.Append<>(targetName);
        }
        else
        {
            content.text.Append<>(Enum.GetName(typeof(NoTopics), Random.Range(0, NoTopicsLength-1)));
        }
        
        for (; words < wordMax; words++)
        {
            content.text.Append<>("Bla ");
        }
    }*/
    
    IEnumerator FillBubbleDelayed()
    {
        content.text.Append<>();
    }
}

