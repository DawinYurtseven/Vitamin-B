using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour, IGuest
{
    [SerializeField]
    private VIBECHECK _vibecheck = VIBECHECK.NotPassed;

    private List<Guest> _contacts = new List<Guest>();
    
    /* would be the dream
    [SerializeField]
    private Dictionary<Guest, List<Topics>> _contacts = new Dictionary<Guest, List<Topics>>();
    */
    
    
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
            Dictionary<Guest, List<Topics>> result = new Dictionary<Guest, List<Topics>>();
            foreach (Guest contact in _contacts)
            {
                result.Add(contact, new List<Topics>());
            }

            return result;
        }
    }

}
