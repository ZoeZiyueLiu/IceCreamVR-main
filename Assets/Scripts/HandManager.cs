using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private static HandManager instance = null;

    private OVRHand[] m_hands;

    public bool leftGrab, rightGrab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            gameObject.SetActive(false);
            Debug.LogWarning("More than 1 instance of hand manager in the scene!");
        }
    }
    
    public static HandManager Instance
    {
        get { return instance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hands = new OVRHand[]
        {
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/OVRCustomHandPrefab_L").GetComponent<OVRHand>(),
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/OVRCustomHandPrefab_R").GetComponent<OVRHand>()
        };
        leftGrab = false;
        rightGrab = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_hands[0].GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            m_hands[0].GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            Debug.Log("left hand is grabbing.");
            leftGrab = true;
        }
        else
        {
            leftGrab = false;
        }
        if (m_hands[1].GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            m_hands[1].GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            Debug.Log("right hand is grabbing.");
            rightGrab = true;
        }
        else
        {
            rightGrab = false;
        }
    }
}
