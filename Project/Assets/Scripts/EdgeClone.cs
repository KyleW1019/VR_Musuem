using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeClone : MonoBehaviour
{
    [SerializeField] private GameObject edgeEmpty;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = edgeEmpty.transform.position;
    }
}
