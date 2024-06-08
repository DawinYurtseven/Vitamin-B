using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour, IGuest
{
    [SerializeField]
    private VIBECHECK _vibecheck = VIBECHECK.NotPassed;

    [SerializeField] private string _name = "placeholder";
    
    [SerializeField]
    private List<Guest> _contacts = new List<Guest>();

    [SerializeField]
    private Material[] _vibeMaterials;
    
    /* would be the dream
    [SerializeField]
    private Dictionary<Guest, List<Topics>> _contacts = new Dictionary<Guest, List<Topics>>();
    */
    
    
    void Awake()
    {
        GuestCustimization customization = GuestCustimization.Instance;
        
        _vibeMaterials = customization.ReceiveMaterials();
        _name = customization.ReceiveName();

        if (TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer mesh))
        {
            mesh.sharedMesh = customization.ReceiveModel();
            mesh.material = _vibeMaterials[(int)_vibecheck];
        }
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
            if (this.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer meshRenderer))
            {
                meshRenderer.material = _vibeMaterials[(int)value];
            }
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

    public string Name
    {
        get
        {
            return _name;
        }
    }

}
