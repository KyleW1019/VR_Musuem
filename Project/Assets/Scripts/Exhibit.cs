using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhibit : MonoBehaviour
{
    [SerializeField] private GameObject nearestNodeGO;
    [SerializeField] private string exhibitDesc;
    public string ExhibitDesc => exhibitDesc;

    private Node nearestNode;
    public Node NearestNode => nearestNode;
    private string exhibitName;
    public string ExhibitName => exhibitName;
    
    // Start is called before the first frame update
    void Start()
    {
        nearestNode = nearestNodeGO.GetComponent<Node>();
        exhibitName = this.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
