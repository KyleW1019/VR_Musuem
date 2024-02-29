using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Period : IComparable
{
    private string periodName;
    public string PeriodName
    {
        get => periodName;
        set => periodName = value;
    }

    private List<Artist> artists;
    public List<Artist> getArtists => artists;

    public Period()
    {
        periodName = "";
        artists = new List<Artist>();
    }

    public Period(string name) : this()
    {
        periodName = name;
    }

    public void AddArtist(Artist artist)
    {
        artists.Add(artist);
    }

    public int CompareTo(object other)
    {
        if (other.GetType() != this.GetType()) return 1;
        Period otherPeriod = (Period)other;
        if (otherPeriod.PeriodName == this.periodName) return 0;
        else return String.Compare(this.periodName, otherPeriod.PeriodName, StringComparison.Ordinal);
    }
}
