using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueElement : IComparable
{
    private Node node;
    public Node DestNode => node;
    private List<Edge> path;

    public List<Edge> Path
    {
        get => path;
        set => path = value;
    }
    private float distance;

    public float Distance
    {
        get => distance;
        set => distance = value;
    }

    public QueueElement(Node node, List<Edge> path, float distance)
    {
        this.node = node;
        this.path = path;
        this.distance = distance;
    }

    public int CompareTo(object other)
    {
        if (other.GetType() != this.GetType())
            throw new System.Exception("Incorrect Type to compare.");
        QueueElement qe = (QueueElement)other;
        if (this.distance < qe.distance) return -1;
        if (this.distance > qe.distance) return 1;
        return 0;
    }
}
