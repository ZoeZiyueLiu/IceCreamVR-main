using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexOrder : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verts;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        verts = new Vector3[mesh.vertices.Length];
        verts = mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        //int i = 0;
        //foreach(Vector3 vec in mesh.vertices)
        //{
        //    verts[i] = vec;
        //    i++;
        //}
        verts[2] = new Vector3(0, 0, 0);
        mesh.vertices = verts;
    }
}
