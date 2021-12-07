using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCream : MonoBehaviour
{
    public List<string> Ingredients = new List<string>();
    public bool onHolder = false;
    public int holderIndex = -1;
    public Grabber grabber = null;
    public List<GameObject> strokes = new List<GameObject>();

    private Rigidbody _rb;
    private Collider _collider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void HandlePickup()
    {
        //_rb.detectCollisions = false;
    }

    public void HandleDrop()
    {
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
            col.enabled = false;
        Collider[] dropAreas = transform.GetChild(0).GetComponents<Collider>();
        foreach (Collider col in dropAreas)
        {
            col.enabled = true;
        }
        _collider.enabled = true;
    }

    public void AddStroke(GameObject stroke)
    {
        strokes.Add(stroke);
    }
}
