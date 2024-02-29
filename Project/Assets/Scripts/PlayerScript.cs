using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Player Objects
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;

    // Player settings
    [SerializeField] private float playerHeight;
    [SerializeField] private float velocityConstant = 2.0f;
    [SerializeField] private float rotationConstant = 1f;
    [SerializeField] private bool seated;

    // Seated Arrow
    [SerializeField] private GameObject seatedArrow;
    [SerializeField] private GameObject feet;

    // Nearest Nodes
    private Node nearestNode;
    private GameObject nearestNodeGO;
    public Node NearestNode => nearestNode;
    
    private Node nearestMusicNode;
    private GameObject nearestMusicNodeGO;
    private GameObject prevMusicNodeGO;
    private music musicNodeScript;
    public Node NearestMusicNode => nearestMusicNode;
    
    private int currRoom;

    // Start is called before the first frame update
    void Start()
    {
        prevMusicNodeGO = null;
        musicNodeScript = null;
        currRoom = 0;
        seatedArrow.SetActive(true ? seated : false);
        feet.SetActive(true ? seated : false);
    }

    // Update is called once per frame
    void Update()
    {
        RayDown();

        // Determine if player has left the room music node was in
        if ( prevMusicNodeGO != null && musicNodeScript != null &&
            (currRoom != Int32.Parse(prevMusicNodeGO.name.Substring(0, 1))))
        {
            // deactivate old music
            musicNodeScript.DeActivateMusic();
            musicNodeScript = null;
            prevMusicNodeGO = null;
        }

        var playerPos = player.transform.position;

        // Determine Nearest Nodes
        RaycastHit[] nodes = Physics.SphereCastAll(
            new Vector3(playerPos.x, 
                playerPos.y + 20f, playerPos.z),
            10.0f, new Vector3(0f, -1f, 0f));
        
        float nearestNodeDist = Mathf.Infinity;
        float nearestMusicNodeDist = Mathf.Infinity;

        nearestNode = null;
        nearestMusicNode = null;
        nearestNodeGO = null;
        
        nearestMusicNodeGO = null;
        
        foreach (RaycastHit hit in nodes)
        {
            // determine nearest node for navigation
            if ((hit.distance < nearestNodeDist) && (hit.collider.gameObject != nearestNodeGO) &&
                (hit.transform.gameObject.layer == LayerMask.NameToLayer("Node") || 
                 hit.transform.gameObject.layer == LayerMask.NameToLayer("MusicNode")))
            {
                nearestNodeDist = hit.distance;
                nearestNodeGO = hit.transform.gameObject;
            }
            
            // determine nearest node for music
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("MusicNode")) continue;
            
            var nodeNum = Int32.Parse(hit.transform.gameObject.name.Substring(0, 1));
            if (nodeNum == currRoom &&
                (hit.distance < nearestMusicNodeDist) && (hit.collider.gameObject != nearestMusicNodeGO))
            {
                nearestMusicNodeDist = hit.distance;
                nearestMusicNodeGO = hit.transform.gameObject;
            }
        }

        // null check, update nearest node
        if (nearestNodeGO != null) nearestNode = nearestNodeGO.GetComponent<Node>();

        // Determine if music node has changed and update
        if (nearestMusicNodeGO != null && nearestMusicNodeGO != prevMusicNodeGO)
        {
            // Deactivate old music
            if (musicNodeScript != null)
                musicNodeScript.DeActivateMusic();
            
            prevMusicNodeGO = nearestMusicNodeGO;
            musicNodeScript = prevMusicNodeGO.GetComponent<music>();
        }
        
        // update volume of music node
        if (prevMusicNodeGO != null && musicNodeScript != null)
        {
            var musicNodePos = prevMusicNodeGO.transform.position;
            var musicDist = Vector2.Distance(new Vector2(playerPos.x, playerPos.z),
                new Vector2(musicNodePos.x, musicNodePos.z));
            musicNodeScript.ActivateMusic(musicDist);
        }
        
    }


    public void MovePlayer(Vector2 trackPad)
    {
        // add raycast in direction of trackpad, only excute movement if that raycast is clean

        // z = forward (vector2.y)
        // x = strafe (vector2.x)
        //if ((trackpad.x > -0.1 && trackpad.x < 0.1) && (trackpad.y > -0.1 && trackpad.y < 0.1))
        //    rb.addrelativeforce(0f, 0f, 0f, forcemode.velocitychange);
        //else
        if (seated)
        {
            if (!Physics.Raycast(player.transform.position, new Vector3(-trackPad.x, 0f, -trackPad.y), 1.75f))
                rb.AddRelativeForce(-trackPad.x * velocityConstant, 0f, -trackPad.y * velocityConstant, ForceMode.VelocityChange);
        }
        //rb.addforce(trackpad.x * velocityconstant, 0f, trackpad.y * velocityconstant, forcemode.velocitychange);
        else
        {
            var cameraFwd = playerCamera.transform.forward;
            var playerFwd = player.transform.forward;
            float theta = Mathf.Deg2Rad * (playerCamera.transform.localEulerAngles.y);
            //Debug.Log(theta);
            //Debug.Log($"x: {(Mathf.Sin(theta))} z: {MathF.Cos(theta)}");
            // Forward/Backward
            rb.AddRelativeForce(Mathf.Sin(theta) * trackPad.y * velocityConstant + Mathf.Cos(theta) * trackPad.x * velocityConstant, 
                0f,
                MathF.Cos(theta) * trackPad.y * velocityConstant + MathF.Sin(theta) * trackPad.x * velocityConstant, 
                ForceMode.VelocityChange);

            //// Sideways
            //rb.AddRelativeForce(Mathf.Cos(theta) * trackPad.x * velocityConstant, 0f,
            //    MathF.Sin(theta) * trackPad.x * velocityConstant, ForceMode.VelocityChange);
        }

        //rb.velocity = new Vector3(trackPad.x * velocityConstant, 0f, trackPad.y * velocityConstant);

    }

    public void RotatePlayer(Vector2 trackPad)
    {
        // - vector2.x == + rotation about y axis
        if (seated)
            if (!(trackPad.x > -0.2 && trackPad.x < 0.2))
               player.transform.Rotate(new Vector3(0f, trackPad.x * rotationConstant, 0f), Space.Self);

    }

    public void RayDown()
    {
        var playerPos = player.transform.position;
        // // RaycastHit hit;
        // Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.down), out hit, 6f);
        // player.transform.position = new Vector3(player.transform.position.x, hit.point.y + playerHeight, player.transform.position.z);

        RaycastHit[] hits = Physics.RaycastAll(playerPos, 
            player.transform.TransformDirection(Vector3.down), 10f);
        var deadSpace = true;
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                player.transform.position = new Vector3(playerPos.x, 
                    hit.point.y + playerHeight, playerPos.z);
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Room"))
            {
                currRoom = Int32.Parse(hit.transform.gameObject.name);
                deadSpace = false;
            }
        }

        if (deadSpace) currRoom = 0;

    }
}
