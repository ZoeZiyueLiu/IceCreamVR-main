using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamStroke : MonoBehaviour
{
    public string flavor;
    public GameObject cone;
    public bool isRelease = false;
    public bool isSettled = false;

    public void Start()
    {
        if (flavor == "")
            Debug.LogError("Error: flavor is not set!");
        //StartCoroutine(UpdateSettled());
    }

    public void StartFreezeCoroutine()
    {
        //StartCoroutine(FreezeAfterSeconds());
    }

    //public IEnumerator FreezeAfterSeconds()
    //{
    //    while (!isRelease)
    //    {
    //        yield return new WaitForSecondsRealtime(5f);
    //    }
    //    yield return new WaitForSecondsRealtime(3f);
    //    while (!isSettled)
    //    {
    //        yield return new WaitForSecondsRealtime(5f);
    //    }
    //    FreezeAllRigidbody();
    //    StopCoroutine(FreezeAfterSeconds());
    //}

    //private IEnumerator UpdateSettled()
    //{
    //    while(true)
    //    {
    //        bool flag = true;
    //        //if all velocity is within a threshold then is settled
    //        foreach (Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
    //        {
    //            if(rb.velocity.magnitude > velocityThreshold)
    //            {
    //                flag = false;
    //                isSettled = false;
    //            } 
    //        }
    //        if(flag)
    //        {
    //            //FreezeAllRigidbody();
    //            isSettled = true;
    //            StopCoroutine(UpdateSettled());
    //        }
    //        yield return new WaitForSeconds(2f);
    //    }
    //}

    //public void FreezeAllRigidbody()
    //{
    //    foreach(Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
    //    {
    //        rb.constraints = RigidbodyConstraints.FreezeAll;
    //    }
    //}

    //public void UnFreezeAllRigidbody()
    //{
    //    foreach (Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
    //    {
    //        rb.constraints = RigidbodyConstraints.None;
    //    }
    //}

    //private void MergeVertex()
    //{
    //    for (int i = 1; i < transform.childCount; i++)
    //    {
    //        Transform t1 = transform.GetChild(i - 1);
    //        Transform t2 = transform.GetChild(i);
    //        Mesh mesh1 = t1.GetComponent<MeshFilter>().mesh;
    //        Mesh mesh2 = t2.GetComponent<MeshFilter>().mesh;
    //        Vector3[] vers = mesh2.vertices;

    //        for (int v = 0; v < 23; v++)
    //        {
    //            if (v % 2 == 0)
    //            {
    //                Vector3 vertexPos_world = t1.transform.TransformPoint(mesh1.vertices[v + 1]);
    //                //Debug.Log("here" + vertexPos_world);
    //                Debug.Log("before " + mesh2.vertices[v]);
    //                Vector3 vertexPos_object = t2.transform.InverseTransformPoint(vertexPos_world);
    //                Debug.Log("after " + vertexPos_object);
    //                //mesh2.vertices[v] = new Vector3(0,0,0);
    //                vers[v] = vertexPos_object;
    //            }

    //        }
    //        mesh2.vertices = vers;
    //        mesh2.RecalculateBounds();
    //        mesh2.RecalculateNormals();
    //    }
    //}
}
