using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GuestCustimization : MonoBehaviour
{
    public static GuestCustimization Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    [Header("VibeMaterials")]
    [SerializeField] private Material _notPassedMat;
    [SerializeField] private Material[] _passedMats;
    [SerializeField] private Material[] _surpassedMats;

    [Header("GuestNames")] 
    [SerializeField] private string[] _jobs;

    [SerializeField] private string[] _names;

    [Header("Models")] 
    [SerializeField] private int _numOfModels;

    [SerializeField] private Mesh[] _hatsAndHair;
    [SerializeField] private Mesh[] _glasses;
    [SerializeField] private Mesh[] _neck;
    [SerializeField] private Mesh[] _beards;
    [Range(0.0f, 1.0f), SerializeField] private float _beardPartProbability;

    [Header("Voices")] 
    [SerializeField] private AudioClip[] _voiceSet1;
    [SerializeField] private AudioClip[] _voiceSet2;
    [SerializeField] private AudioClip[] _voiceSet3;
    [SerializeField] private AudioClip[] _voiceSet4;
    [SerializeField] private AudioClip[] _voiceSet5;

    private AudioClip[][] _audioSets;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _audioSets[0] = _voiceSet1;
        _audioSets[1] = _voiceSet2;
        _audioSets[2] = _voiceSet3;
        _audioSets[3] = _voiceSet4;
        _audioSets[4] = _voiceSet5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material[] ReceiveMaterials()
    {
        int vibes = (int)VIBECHECK.NUM_VIBES;
        
        Material[] mats = new Material[vibes];

        mats[0] = _notPassedMat;
        int index = Random.Range(0, vibes - 1);

        mats[1] = _passedMats[index];
        mats[2] = _surpassedMats[index];

        return mats;
    }

    public string ReceiveName()
    {
        return _jobs[Random.Range(0, _jobs.Length)] + " " + _names[Random.Range(0, _names.Length)];
    }

    public int ReceiveModel()
    {
        return Random.Range(0,_numOfModels);
    }
    
    public Mesh ReceiveHatAndHair()
    {
        if (_hatsAndHair.Length == 0)
        {
            Debug.LogError("no hat/hair assign in GuestCustomization");
            return new Mesh();
        }
        return _hatsAndHair[Random.Range(0,_hatsAndHair.Length)];
    }
    
    public Mesh ReceiveGlasses()
    {
        if (_glasses.Length == 0)
        {
            Debug.LogError("no models assign in GuestCustomization");
            return new Mesh();
        }
        return _glasses[Random.Range(0,_glasses.Length)];
    }
    
    public Mesh ReceiveBeard()
    {
        if (_beards.Length == 0)
        {
            Debug.LogError("no models assign in GuestCustomization");
            return new Mesh();
        }

        List<Mesh> beardParts = new List<Mesh>();

        /*
        foreach (Mesh beard in _beards)
        {
            if (Random.Range(0.0f, 1.0f) > _beardPartProbability)
            {
                beardParts.Add(beard);
            }
        }
        */
        return _beards[Random.Range(0,_beards.Length)];
    }

    public AudioClip[] ReceiveVoice()
    {
        return _audioSets[Random.Range(0, _audioSets.Length)];
    }
}
