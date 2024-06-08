using System.Collections;
using UnityEngine;

public class CandiceNut : MonoBehaviour
{
    public Rigidbody[] nuts;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip scream;
    
    public void FitInUrMouth()
    {
        foreach (Rigidbody nut in nuts)
            StartCoroutine(Nutting(nut));
        
        audioSource.PlayDelayed(6f);
    }

    private IEnumerator Nutting(Rigidbody nut) // ok im sorry at this point
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(1f);
        nut.isKinematic = false;
    }
}
