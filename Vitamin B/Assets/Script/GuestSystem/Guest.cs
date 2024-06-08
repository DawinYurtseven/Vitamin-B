using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour, IGuest
{
    private VIBECHECK _vibecheck = VIBECHECK.NotPassed;

    private Dictionary<Guest, List<Topics>> _contacts = new Dictionary<Guest, List<Topics>>();
    
    private 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public VIBECHECK Vibecheck
    {
        get
        {
            return _vibecheck;
        }
        set
        {
            _vibecheck = value;
        }
    }

    public Dictionary<Guest, List<Topics>> Contacts
    {
        get
        {
            return _contacts;
        }
    }

}
