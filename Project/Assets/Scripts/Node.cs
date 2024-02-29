using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node : MonoBehaviour, IComparable
{
    // Node color and render properties
    private Renderer nodeRenderer;
    private Color clear = new Color(1f, 1f, 1f, 0f);
    private Color green = new Color(1f, 1f, 1f, 1f);
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    
    void Start()
    {
        nodeRenderer = this.gameObject.GetComponent<Renderer>();
        MakeNodeClear();
        
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;

    }
    
    /// <summary>
    /// Highlights the current node, called when node is along path
    /// </summary>
    public void HighLightNode()
    {
        nodeRenderer.material.SetColor(Color1, green);
    }
    
    /// <summary>
    /// Deactivates current node, makes transpartent
    /// </summary>
    public void MakeNodeClear()
    {
        nodeRenderer.enabled = false;
    }
    
    /// <summary>
    /// Overrides CompareTo.  Compares name of Nodes being compared
    /// </summary>
    /// <param name="other">Object to be compared</param>
    /// <returns>0 if this.Name = other.Name; 1 if other is not of type Node,
    /// -1 otherwise</returns>
    public int CompareTo(object other)
    {
        if (other.GetType() != this.GetType()) return 1;
        Node otherNode = (Node)other;
        if (otherNode.gameObject.name == this.gameObject.name) return 0;
        else return -1;
    }
    /// <summary>
    /// Overrides GetHashCode.
    /// </summary>
    /// <returns>Hashcode of node.Name</returns>
    public override int GetHashCode()
    {
        return this.gameObject.name.GetHashCode();
    }

    public override string ToString()
    {
        return this.gameObject.name;
    }
}
