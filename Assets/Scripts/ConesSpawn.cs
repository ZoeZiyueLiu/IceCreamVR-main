using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConesSpawn : MonoBehaviour
{
    private InputManager inputManager;
    public GameObject cone;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cone"))
        {
            GetComponent<AudioSource>().Play();
            Instantiate(cone, transform.position, Quaternion.Euler(180,0,0));
        }
    }
}
