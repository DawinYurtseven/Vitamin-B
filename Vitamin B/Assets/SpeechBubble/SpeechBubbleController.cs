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
    Investors,
    BlaBlaCar
}

public class SpeechBubbleController : MonoBehaviour
{
    [SerializeField]
    private float textFillSpeedMin = 0.1f;
    [SerializeField]
    private float textFillSpeedMax = 1.0f;

    private string[] BlaList = 
        { "Bla ", "Bla? ", "Bla! ", "BlaBla ", "Blaaaaa "};

    [SerializeField] private AudioClip[] BlaSounds;
    
    private string keyword = "";
    private int wordCount = 0;
    private int wordMax = 8;
    private int NoTopicsLength = Enum.GetNames(typeof(NoTopics)).Length;

    [SerializeField] private AudioSource source;
    
    [SerializeField]
    private TextMeshPro content;

    private IGuest target;
   
    
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(FillBubbleDelayed());
        //StartCoroutine(FillBubbleError("Bitcoin"));
    }

    public void Interact()
    {
        if (keyword != "")
        {
            if (Enum.IsDefined(typeof(NoTopics), keyword))
            {
                StartCoroutine(FillBubbleError(keyword));
            }
            else
            {
                target.Vibecheck = VIBECHECK.Passed;
            }
        }
        else
        {
            //do nothing maybe play error sound
        }
    }
    
    IEnumerator FillBubbleDelayed()
    {
        if (wordCount > wordMax)
        {
            content.text = "";
            wordCount = 0;
            keyword = "";
        }

        if (Random.Range(wordCount, wordMax) == wordMax-1 && keyword == "")
        {
            content.text = content.text + "<color=\"red\"> ";
            if (Random.Range(0, 3) == 3)
            {
                keyword = target.Name;
            }
            else
            {
                keyword = Enum.GetName(typeof(NoTopics), Random.Range(0, NoTopicsLength - 1));
            }
            content.text = content.text + keyword + " ";
            content.text = content.text + "</color> ";
        }
        else
        {
            int blaIndex = getRandomBlaIndex();
            content.text = content.text + BlaList[blaIndex];
            source.PlayOneShot(BlaSounds[blaIndex]);
        }
        
        wordCount++;
        yield return new WaitForSeconds(Random.Range(textFillSpeedMin, textFillSpeedMax));
        StartCoroutine(FillBubbleDelayed());
    }

    IEnumerator FillBubbleError(string errortext)
    {
        
        content.text = content.text + errortext + " ";
        wordCount++;
        
        yield return new WaitForSeconds(Random.Range(textFillSpeedMin, textFillSpeedMax));
        if (wordCount > wordMax)
        {
            content.text = "";
            wordCount = 0;
            keyword = "";
            StartCoroutine(FillBubbleDelayed());
        }
        else
        {
            StartCoroutine(FillBubbleError(errortext));
        }
    }

    public void StopSpeechBubble()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    private int getRandomBlaIndex()
    {
        switch (Random.Range(0, 10))
        {
            case 1:
                return 1;

            case 2:
                return 2;

            case 3:
                return 3;

            case 4:
                return 4;

            default:
                return 0;
        }
    }
}
