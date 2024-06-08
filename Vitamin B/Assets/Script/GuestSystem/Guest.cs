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

    [Header("Models")]
    [SerializeField] private GameObject[] _models;

    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    [SerializeField] private GameObject[] _hatAndHairReferences;
    [SerializeField] private GameObject[] _beardReferences;
    [SerializeField] private GameObject[] _neckReferences;
    [SerializeField] private GameObject[] _glassesReferences;
    
    /* would be the dream
    [SerializeField]
    private Dictionary<Guest, List<Topics>> _contacts = new Dictionary<Guest, List<Topics>>();
    */
    
    
    void Start()
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

                Mesh _hatAndHair = customization.ReceiveHatAndHair();
                if (_hatAndHairReferences[modelIndex].TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
                {
                    meshFilter.mesh = _hatAndHair;
                }
                /*
                if (_hatAndHairReferences[modelIndex].TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
                {
                    meshRenderer.material = 
                }
                */
                if (_beardReferences[modelIndex].TryGetComponent<MeshFilter>(out MeshFilter meshFilter2))
                {
                    meshFilter2.mesh = customization.ReceiveBeard();
                }
                
                if (_glassesReferences[modelIndex].TryGetComponent<MeshFilter>(out MeshFilter meshFilter3))
                {
                    meshFilter3.mesh = customization.ReceiveGlasses();
                }

                if (_neckReferences[modelIndex].TryGetComponent<MeshFilter>(out MeshFilter meshFilter4))
                {
                    meshFilter4.mesh = customization.ReceiveNeck();
                }
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
