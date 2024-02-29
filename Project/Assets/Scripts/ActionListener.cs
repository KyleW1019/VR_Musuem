using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Valve.VR;



public class ActionListener : MonoBehaviour
{
    // Player Objects
    [SerializeField] private GameObject playerGO;
    private PlayerScript player;
    
    // HUD Game Object and Script
    [SerializeField] private GameObject HUDGO;
    private HUD HUDScript;
    
    // Controller Inputs
    public SteamVR_Action_Boolean menu;
    public SteamVR_Action_Boolean grasp;
    public SteamVR_Action_Boolean trig;
    public SteamVR_Action_Boolean touchPadSelect;
    public SteamVR_Action_Vector2 touchPad;

    private Vector2 prevLeftTP;
    private Vector2 prevRightTP;

    void Start()
    {
        player = playerGO.GetComponent<PlayerScript>();
        HUDScript = HUDGO.GetComponent<HUD>();
        prevLeftTP = Vector2.zero;
        prevRightTP = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {
        /////////////////////////////////////////
        /////   Detect controller inputs    /////
        /////////////////////////////////////////
        
        // Menu buttons
        if (menu.GetChanged(SteamVR_Input_Sources.LeftHand) && 
            menu.GetState(SteamVR_Input_Sources.LeftHand)) LeftMenu();
        if (menu.GetChanged(SteamVR_Input_Sources.RightHand) && 
            menu.GetState(SteamVR_Input_Sources.RightHand)) RightMenu();
        
        // TouchPad Select
        if (touchPadSelect.GetChanged(SteamVR_Input_Sources.LeftHand) && 
            touchPadSelect.GetState(SteamVR_Input_Sources.LeftHand)) LeftTPPress();
        if (touchPadSelect.GetChanged(SteamVR_Input_Sources.RightHand) && 
            touchPadSelect.GetState(SteamVR_Input_Sources.RightHand)) RightTPPress();
        
        // Grasp
        if (grasp.GetChanged(SteamVR_Input_Sources.LeftHand) && 
            grasp.GetState(SteamVR_Input_Sources.LeftHand)) LeftGrasp();
        if (grasp.GetChanged(SteamVR_Input_Sources.RightHand) && 
            grasp.GetState(SteamVR_Input_Sources.RightHand)) RightGrasp();
        
        // Trigger
        if (trig.GetChanged(SteamVR_Input_Sources.LeftHand) && 
            trig.GetState(SteamVR_Input_Sources.LeftHand)) LeftTrig();
        if (trig.GetChanged(SteamVR_Input_Sources.RightHand) && 
            trig.GetState(SteamVR_Input_Sources.RightHand)) RightTrig();
        
        
        /////////////////////////////////////////
        /////         Update Player         /////
        /////////////////////////////////////////


        Vector2 leftTP = touchPad.GetAxis(SteamVR_Input_Sources.LeftHand);
        Vector2 rightTP = touchPad.GetAxis(SteamVR_Input_Sources.RightHand);


        if (!HUDScript.HUDActive)
        {
            // move and rotate the player according to trackpad input
            player.MovePlayer(leftTP);
            player.RotatePlayer(rightTP);
        }
        // Debug.Log(rightTP);

        // prevLeftTP = leftTP;
        // prevRightTP = rightTP;

    }

    public void LeftGrasp()
    {
        Debug.Log("Left Grasp");
    }

    public void RightGrasp()
    {
        Debug.Log("Right Grasp");
    }

    public void LeftTrig()
    {
        Debug.Log("Left Trig");
    }

    public void RightTrig()
    {
        Debug.Log("Right Trig");
    }

    public void LeftTPPress()
    {
        Vector2 leftTP = touchPad.GetAxis(SteamVR_Input_Sources.LeftHand);
        
        float theta = Mathf.Atan(leftTP.y / leftTP.x);
        float r = Mathf.Sqrt(Mathf.Pow(leftTP.y, 2.0f) + Mathf.Pow(leftTP.x, 2.0f));
        
        if (!HUDScript.HUDActive) return;
        if (!(r > 0.5)) return;
        
        if (leftTP.y > 0 && (theta > Mathf.PI / 4f || theta < -Mathf.PI / 4f)) HUDScript.SwipeUp(1);
        else if (leftTP.y < 0 && (theta > Mathf.PI / 4f || theta < -Mathf.PI / 4f)) HUDScript.SwipeDown(1);
        else if (leftTP.x > 0) HUDScript.SwipeRight();
        else HUDScript.SwipeLeft();
    }

    public void RightTPPress()
    {
        Vector2 leftTP = touchPad.GetAxis(SteamVR_Input_Sources.RightHand);
        
        float theta = Mathf.Atan(leftTP.y / leftTP.x);
        float r = Mathf.Sqrt(Mathf.Pow(leftTP.y, 2.0f) + Mathf.Pow(leftTP.x, 2.0f));
        
        if (!HUDScript.HUDActive) return;
        if (!(r > 0.5)) return;
        
        if (leftTP.y > 0 && (theta > Mathf.PI / 4f || theta < -Mathf.PI / 4f)) HUDScript.SwipeUp(1);
        else if (leftTP.y < 0 && (theta > Mathf.PI / 4f || theta < -Mathf.PI / 4f)) HUDScript.SwipeDown(1);
        else if (leftTP.x > 0) HUDScript.SwipeRight();
        else HUDScript.SwipeLeft();
    }

    public void LeftTP()
    {
        Debug.Log("Left TP");
    }

    public void RightTP()
    {
        Debug.Log("Right TP");
    }

    public void LeftMenu() {
        if (HUDScript.HUDActive) HUDScript.DeActivateHUD();
        else HUDScript.ActivateHUD();
    }

    public void RightMenu() {
        if(HUDScript.NavigatingToExhibit) HUDScript.EndNavigation();
    }


}
