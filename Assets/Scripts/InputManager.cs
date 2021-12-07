using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance = null;

    public bool leftGrab, rightGrab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            gameObject.SetActive(false);
            Debug.LogWarning("More than 1 instance of InputManager in the scene!");
        }

    }
    public static InputManager Instance
    {
        get { return instance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        leftGrab = false;
        rightGrab = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.Touch))
        {
            Debug.Log("LHandTrigger hand is grabbing.");
            leftGrab = true;
        }
        else if(OVRInput.GetUp(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.Touch))
        {
            Debug.Log("LHandTrigger hand is released.");
            leftGrab = false;
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.Touch))
        {
            Debug.Log("RHandTrigger hand is grabbing.");
            rightGrab = true;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.Touch))
        {
            Debug.Log("RHandTrigger hand is released.");
            rightGrab = false;
        }
    }
}
