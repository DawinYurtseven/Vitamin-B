using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Guest : MonoBehaviour, IGuest
{
    [Header("Personality")]
    [SerializeField]
    private VIBECHECK _vibecheck = VIBECHECK.NotPassed;

    [SerializeField] private string _name = "placeholder";
    
    [SerializeField]
    private List<Guest> _contacts = new List<Guest>();

    //[SerializeField]
    private AudioClip[] _voice;
    //[SerializeField]
    private Material[] _vibeMaterials;

    //[SerializeField] 
    private Mesh _hatAndHair;
    //[SerializeField] 
    private Mesh _glasses;
    //[SerializeField] 
    private Mesh _beard;

    [Header("Models")]
    [SerializeField] private GameObject[] _models;

    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    [SerializeField] private GameObject[] _hatAndHairReferences;
    [SerializeField] private GameObject[] _beardReferences;
    [SerializeField] private GameObject[] _neckReferences;
    
    /* would be the dream
    [SerializeField]
    private Dictionary<Guest, List<Topics>> _contacts = new Dictionary<Guest, List<Topics>>();
    */
    
    
    void Awake()
    {
        GuestCustimization customization = GuestCustimization.Instance;

        int modelIndex = customization.ReceiveModel();
        for (int i = 0; i < _models.Length; i++)
        {
            if (i != modelIndex)
            {
                Destroy(_models[i]);
            }
            else
            {
                _vibeMaterials = customization.ReceiveMaterials();
                _name = customization.ReceiveName();
                
                _skinnedMeshRenderers[modelIndex].material = _vibeMaterials[(int)_vibecheck];

                _hatAndHair = customization.ReceiveHatAndHair();
                _beard = customization.ReceiveBeard();
                _glasses = customization.ReceiveGlasses();
            }
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

    public AudioClip[] Voice
    {
        get
        {
            return _voice;
        }
    }

}
