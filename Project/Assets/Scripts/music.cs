using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{
    private AudioSource myAudio;

    private bool closestNode = false;
    // private float distToNode;


    // Start is called before the first frame update
    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = 0;
    }


    public void ActivateMusic(float distToNode)
    {
        if (distToNode < 3)
        {
            myAudio.volume = 1;
        }
        else if (distToNode < 3.5)
        {
            myAudio.volume = .9f;
        }
        else if (distToNode < 4)
        {
            myAudio.volume = .8f;
        }
        else if (distToNode < 4.5)
        {
            myAudio.volume = .7f;
        }
        else if (distToNode < 5)
        {
            myAudio.volume = .5f;
        }
        else if (distToNode < 5.5)
        {
            myAudio.volume = .3f;
        }
    }

    public void DeActivateMusic()
    {
        myAudio.volume = 0;
    }
}