using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Artist : MonoBehaviour
{
    private string artistName;
    public string ArtistName => artistName;

    private List<string> brief;
    public List<string> Brief => brief;

    private List<Exhibit> exhibitObjects;
    public List<Exhibit> Exhibits => exhibitObjects;

    private string bio;
    public string Bio => bio;

    private void Start()
    {
        brief = new List<string>();
        bio = "";
        exhibitObjects = new List<Exhibit>();
        artistName = this.name;
        
        foreach (Transform child in this.transform)
        {
            exhibitObjects.Add(child.GetComponent<Exhibit>());
        }
    }

    public void AddToBrief(string addition)
    {
        brief.Add(addition);
    }

    public void AddToBio(string addition)
    {
        bio += addition;
    }

    public void AddToExhibits(Exhibit ex)
    {
        exhibitObjects.Add(ex);
    }
}
