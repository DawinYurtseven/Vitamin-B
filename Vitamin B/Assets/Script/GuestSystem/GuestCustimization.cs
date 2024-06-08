using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private Material _notPassedMat;
    [SerializeField] private Material[] _passedMats;
    [SerializeField] private Material[] _surpassedMats;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
