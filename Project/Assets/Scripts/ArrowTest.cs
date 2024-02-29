using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : MonoBehaviour
{

    public GameObject arrow;

    private float x2 = 0f;

    private float x1 = 0f;

    private float z2 = 5f;

    private float z1 = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        var midx = (x2 - x1) / 2f;
        var midz = (z2 - z1) / 2f;

        arrow.transform.position = new Vector3(midx, 0, midz);
        var ang = Vector2.Angle(Vector2.right, new Vector2(x2 - x1, z2 - z1));
        arrow.transform.rotation = Quaternion.Euler(0f, ang, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
