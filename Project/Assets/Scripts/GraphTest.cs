using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTest : MonoBehaviour
{
    public GameObject graphGO;
    private Graph g;
    public GameObject testA;
    public GameObject testB;
    public GameObject testC;
    public GameObject testD;
    public Node testANode;
    public Node testBNode;
    public Node testCNode;
    public Node testDNode;

    // Start is called before the first frame update
    void Start()
    {
        g = graphGO.GetComponent<Graph>();
        testANode = testA.GetComponent<Node>();
        testBNode = testB.GetComponent<Node>();
        testCNode = testC.GetComponent<Node>();
        testDNode = testD.GetComponent<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            g.DisplayPath(testANode, testBNode);
        }
        
        if (Input.GetKeyDown("e"))
        {
            g.DisplayPath(testCNode, testDNode);
        }
    }
}
