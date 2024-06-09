using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public class Guest : MonoBehaviour, IGuest
{
    
    [SerializeField] private VIBECHECK _vibecheck = VIBECHECK.NotPassed;
    
    [SerializeField] private bool _isBoss = false;
    
    [SerializeField]
    private List<Guest> _contacts = new List<Guest>();
    
    
    
    [Header("Models")]
    [SerializeField] private GameObject[] _models;

    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    [SerializeField] private GameObject[] _hatAndHairReferences;
    [SerializeField] private GameObject[] _beardReferences;
    [SerializeField] private GameObject[] _neckReferences;
    [SerializeField] private GameObject[] _glassesReferences;
    
    private AudioClip[] _voice;
    private Material[] _vibeMaterials;
    private string _name = "placeholder";

    private int modelIndex;
    
    [SerializeField]
    private GameObject SpeechBubblePrefab;
    private GameObject _speechBubbleRef;
    private bool _speechBubbleActive = false;
    [SerializeField] 
    private Vector3 SpeechBubbleOffset = new Vector3(-1.25f,2.25f,0f);
    
    
    void Start()
    {
        GuestCustimization customization = GuestCustimization.Instance;
        
        _vibeMaterials = customization.ReceiveMaterials();
        _name = customization.ReceiveName();
        _voice = customization.ReceiveVoice();

        modelIndex = customization.ReceiveModel();
        for (int i = 0; i < _models.Length; i++)
        {
            if (i != modelIndex)
            {
                Destroy(_models[i]);
            }
            else
            {
                Material startMat = _vibeMaterials[(int)_vibecheck];
                _skinnedMeshRenderers[modelIndex].material = startMat;

                MeshRenderer meshRenderer;
                MeshFilter meshFilter;

                if (_hatAndHairReferences[modelIndex].TryGetComponent<MeshFilter>(out meshFilter))
                {
                    meshFilter.mesh = customization.ReceiveHatAndHair();
                }
                if (_hatAndHairReferences[modelIndex].TryGetComponent<MeshRenderer>(out meshRenderer))
                {
                    meshRenderer.material = startMat;
                }
                //____________
                if (_beardReferences[modelIndex].TryGetComponent<MeshFilter>(out meshFilter))
                {
                    meshFilter.mesh = customization.ReceiveBeard();
                }
                if (_beardReferences[modelIndex].TryGetComponent<MeshRenderer>(out meshRenderer))
                {
                    meshRenderer.material = startMat;
                }
                //_____________
                if (_glassesReferences[modelIndex].TryGetComponent<MeshFilter>(out meshFilter))
                {
                    meshFilter.mesh = customization.ReceiveGlasses();
                }
                if (_glassesReferences[modelIndex].TryGetComponent<MeshRenderer>(out meshRenderer))
                {
                    meshRenderer.material = startMat;
                }
                //____________
                if (_neckReferences[modelIndex].TryGetComponent<MeshFilter>(out meshFilter))
                {
                    meshFilter.mesh = customization.ReceiveNeck();
                }
                if (_neckReferences[modelIndex].TryGetComponent<MeshRenderer>(out meshRenderer))
                {
                    meshRenderer.material = startMat;
                }
            }
        }
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
            GuestCustimization customization = GuestCustimization.Instance;
            Material hatAndHairMaterial = customization.ReceiveHatAndHairMaterial();
            Material accessoryMaterial = customization.ReceiveAccessoryMaterial();
            MeshRenderer meshRenderer;
            _skinnedMeshRenderers[modelIndex].material = _vibeMaterials[(int)_vibecheck];
            if (_hatAndHairReferences[modelIndex].TryGetComponent<MeshRenderer>(out meshRenderer))
            {
                meshRenderer.material = hatAndHairMaterial;
            }
            if (_beardReferences[modelIndex].TryGetComponent<MeshRenderer>(out meshRenderer))
            {
                meshRenderer.material = hatAndHairMaterial;
            }
            if (_glassesReferences[modelIndex].TryGetComponent<MeshRenderer>(out  meshRenderer))
            {
                meshRenderer.material = accessoryMaterial;
            }
            if (_neckReferences[modelIndex].TryGetComponent<MeshRenderer>(out meshRenderer))
            {
                meshRenderer.material = accessoryMaterial;
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

    public void Interact()
    {
        if (!_speechBubbleActive && _vibecheck == VIBECHECK.Passed)
        {
            SpeechBubbleRef = Instantiate(SpeechBubblePrefab, this.transform.position + SpeechBubbleOffset, Quaternion.identity, this.transform);
            SpeechBubbleRef.GetComponent<SpeechBubbleController>().target = this;
            _speechBubbleActive = true;
        }
        else
        {
            SpeechBubbleRef.GetComponent<SpeechBubbleController>().Interact();
        }
    }
    
    public bool SpeechBubbleActive
    {
        get
        {
            return _speechBubbleActive;
        }
        set
        {
            _speechBubbleActive = value;
        }
    }
    
    public GameObject SpeechBubbleRef
    {
        get
        {
            return _speechBubbleRef;
        }
        set
        {
            _speechBubbleRef = value;
        }
    }

    public bool isBoss
    {
        get
        {
            return _isBoss;
        }
    }
    
    
}
