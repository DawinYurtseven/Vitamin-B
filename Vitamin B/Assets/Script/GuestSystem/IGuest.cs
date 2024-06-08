using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IGuest
{
    public VIBECHECK Vibecheck { get; set; }
    
    public Dictionary<Guest, List<Topics>> GetContacts() {}
}
