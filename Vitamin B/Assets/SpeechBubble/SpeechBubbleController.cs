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
        { "Bla ", "Bla? ", "Bla! ", "BlaBla ", "Blaaa "};

    [SerializeField]
    private AudioClip[] BlaSounds;
    
    private string keyword = "";
    private int wordCount = 0;
    private int wordMax = 8;
    private int NoTopicsLength = Enum.GetNames(typeof(NoTopics)).Length;

    private int currentContact = 0;
    private List<Guest> targets = new List<Guest>();

    [SerializeField] private AudioSource source;
    
    [SerializeField]
    private TextMeshPro content;
    
    public IGuest target;
   
    
    private void Awake()
    {
        source = GetComponent<AudioSource>();

        
    }

    private void Start()
    {
        BlaSounds = target.Voice;
        foreach (var newTarget in target.Contacts.Keys)
        {
            if(newTarget.Vibecheck == VIBECHECK.NotPassed)
                targets.Add(newTarget);
        }
        StartCoroutine(FillBubbleDelayed());
        if(targets.Count == 0)
            StopSpeechBubble();
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
                targets[currentContact].Vibecheck = VIBECHECK.Passed;
                currentContact++;
                if (currentContact >= targets.Count)
                {
                    target.Vibecheck = VIBECHECK.Surpassed;
                    StopSpeechBubble();
                }

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
            if (Random.Range(0, 3) == 2)
            {
                keyword = targets[currentContact].Name;
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
            Debug.Log(blaIndex);
            Debug.Log(BlaSounds.Length);
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
        target.SpeechBubbleActive = false;
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
