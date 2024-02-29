using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Arrows : MonoBehaviour
{
    public GameObject parent;
    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;
    public GameObject arrow4;

    private bool arrowsActivated;
    public bool ArrowsActivated
    {
        get => arrowsActivated;
        set => arrowsActivated = value;
    }

    private Renderer[] arrows = new Renderer[8];
    private Color full = new Color(1f, 1f, 1f, 1f);
    private Color twoThirds = new Color(1f, 1f, 1f, 0.66f);
    private Color oneThird = new Color(1f, 1f, 1f, 0.33f);
    private Color clear = new Color(1f, 1f, 1f, 0.0f);

    private float startTime;
    public float arrowTime;
    private int counter;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        arrowsActivated = false;
        arrows[0] = arrow1.GetComponent<Renderer>();
        arrows[1] = arrow2.GetComponent<Renderer>();
        arrows[2] = arrow3.GetComponent<Renderer>();
        arrows[3] = arrow4.GetComponent<Renderer>();
        foreach (var arrow in arrows)
        {
            if (arrow) arrow.material.SetColor(Color1, clear);
        }
        counter = 8;


    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - startTime) < arrowTime) return;
        ArrowChange();
        startTime = Time.time;
    }

    private void ArrowChange()
    {
        if (arrows[counter%8]) arrows[counter%8].material.SetColor(Color1, full);
        if (arrows[(counter-1)%8]) arrows[(counter-1)%8].material.SetColor(Color1, twoThirds);
        if (arrows[(counter-2)%8]) arrows[(counter-2)%8].material.SetColor(Color1, oneThird);
        if (arrows[(counter-3)%8]) arrows[(counter-3)%8].material.SetColor(Color1, clear);
        counter++;
    }
}
