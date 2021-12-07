using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Grabber : MonoBehaviour
{
    public enum Handedness
    {
        Left = 0,
        Right = 1
    }
    public Handedness handedness;
    public Material canGrabMat;
    private Renderer m_renderer;

    //private HandManager hm;
    private InputManager im;

    [SerializeField] private GameObject grabbedObject;

    public bool isGrabbing;
    private bool isIntersecting;

    private Material oldMat;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();

        //hm = HandManager.Instance;
        im = InputManager.Instance;

        grabbedObject = null;
        isGrabbing = false;

        //GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        bool grab;
        if (handedness == Handedness.Left) grab = im.leftGrab;
        else grab = im.rightGrab;
        //if (grabbedObject.gameObject == null) return;

        // release grab
        if (!grab && isGrabbing)
        {
            if (grabbedObject.CompareTag("Cone") || grabbedObject.CompareTag("Spoon"))
            {
                grabbedObject.GetComponent<MeshCollider>().convex = true;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            if(grabbedObject.CompareTag("Cone"))
            {
                grabbedObject.GetComponent<IceCream>().HandleDrop();
            }
            isGrabbing = false;
            //reset rigid body
            grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }

    // on trigger enter: change to can grab color
    public void OnTriggerEnter(Collider other)
    {
        if (!isIntersecting)
        {
            if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Cone") || other.gameObject.CompareTag("Spoon"))
            {
                oldMat = other.gameObject.GetComponent<Renderer>().material;
                //other.gameObject.GetComponent<Renderer>().material = canGrabMat;
                isIntersecting = true;
            }
        }
        
    }

    // check if hand is grabbing, change parent
    public void OnTriggerStay(Collider other)
    {
        bool grab;
        if (handedness == Handedness.Left) grab = im.leftGrab;
        else grab = im.rightGrab;
        if (grab && !isGrabbing)
        {

            if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Cone") || other.gameObject.CompareTag("Spoon"))
            {
                other.gameObject.GetComponent<Renderer>().material = oldMat;
                isGrabbing = true;
                grabbedObject = other.gameObject;
                grabbedObject.transform.SetParent(this.transform);
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeAll;

                if (grabbedObject.CompareTag("Cone") || grabbedObject.gameObject.CompareTag("Spoon"))
                {
                    rb.isKinematic = true;
                    grabbedObject.GetComponent<MeshCollider>().convex = false;
                }
                if(grabbedObject.CompareTag("Cone"))
                {
                    //reference this grabber to icecream object
                    grabbedObject.GetComponent<IceCream>().grabber = this;
                }
            }       
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isIntersecting)
        {
            if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Cone") || other.gameObject.CompareTag("Spoon"))
            {
                
                isIntersecting = false;

            }
        }
    }


}