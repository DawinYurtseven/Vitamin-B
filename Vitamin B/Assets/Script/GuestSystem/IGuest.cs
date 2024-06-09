using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IGuest
{
    public VIBECHECK Vibecheck { get; set; }
    
    public Dictionary<Guest, List<Topics>> Contacts { get; }
    
    public string Name { get; }
    
    public bool SpeechBubbleActive { get; set; }

    public void Interact();
    
    public AudioClip[] Voice { get; }
}
