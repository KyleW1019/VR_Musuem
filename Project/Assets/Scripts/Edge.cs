using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour, IComparable
{
    // reverse Edge
    private GameObject reverseEdge;
    
    // Source Node
    [SerializeField] private GameObject sourceGO;
    public GameObject SourceGO
    {
        set => sourceGO = value;
    }
    private Node sourceNode;
    public Node SourceNode
    {
        get => sourceNode;
        set => sourceNode = value;
    }
    // Destination Node
    [SerializeField] private GameObject destinationGO;
    public GameObject DestinationGO
    {
        set => destinationGO = value;
    }
    private Node destinationNode;
    public Node DestinationNode
    {
        get => destinationNode;
        set => destinationNode = value;
    }
    // Weight
    private float weight;
    public float Weight
    {
        get => weight;
        set => weight = value;
    }

    private void Start()
    {
        // pull script objects from source and destination
        sourceNode = sourceGO.GetComponent<Node>();
        destinationNode = destinationGO.GetComponent<Node>();
        
        // calculate weight (distance between nodes)
        weight = Vector3.Distance(sourceGO.transform.position, destinationGO.transform.position);
        
    }

    public Edge BuildReverseEdge(GameObject edgeCloneParent)
    {
        reverseEdge = Instantiate(this.gameObject, edgeCloneParent.transform, true);
        // reverseEdge.transform.position = this.gameObject.transform.position;
        // reverseEdge.transform.Rotate(0f,180f,0f);
        
        Edge reverseEdgeObj = reverseEdge.GetComponent<Edge>();
        reverseEdgeObj.sourceGO = this.destinationGO;
        reverseEdgeObj.destinationGO = this.sourceGO;
        reverseEdgeObj.SourceNode = this.destinationNode;
        reverseEdgeObj.destinationNode = this.sourceNode;
        reverseEdgeObj.weight = this.weight;

        return reverseEdgeObj;

    }
    

    /// <summary>
    /// Overrides CompareTo.  Compares this Edge to other Edge
    /// </summary>
    /// <param name="other">Object to compare to</param>
    /// <returns>0 this.Sources = other.Source and this.Destination = other.Destination,
    /// 1 if other is not of type Edge, otherwise -1</returns>
    public int CompareTo(object other)
    {
        if (other.GetType() != this.GetType()) return 1;
        Edge otherEdge = (Edge)other;
        if (otherEdge.SourceNode == this.sourceNode && otherEdge.DestinationNode == destinationNode) return 0;
        return -1;
    }
    
    /// <summary>
    /// Overrides GetHashCode
    /// </summary>
    /// <returns>Hashcode of string representation of Edge</returns>
    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }
    
    /// <summary>
    /// Generates a string object of the Edge in the form "[Source] <-> [Destination] (Weight)"
    /// </summary>
    /// <returns>"[Source] <-> [Destination] (Weight)"</returns>
    public override string ToString()
    {
        return $"[{sourceNode}] <-> [{destinationNode}] ({weight})";
    }
}
