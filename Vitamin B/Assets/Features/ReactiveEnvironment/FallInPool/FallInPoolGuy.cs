using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallInPoolGuy : MonoBehaviour
{
    public GameObject idle;
    public GameObject fallGuy;
    
    public void Fall()
    {
        idle.SetActive(false);
        fallGuy.SetActive(true);
    }
}
