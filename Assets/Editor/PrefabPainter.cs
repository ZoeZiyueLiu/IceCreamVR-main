using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabPainter : ScriptableObject
{
    public GameObject prefab;

    public bool isPainting = false;
    public bool singlePlacement = false;
    public int density = 1;
    public float radius = 1f;
    public bool randomYaw = true;

    public float scaleMin = 1.0f;
    public float scaleMax = 1.5f;

    public void SpawnObject(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!prefab)
        {
            Debug.LogError("ERROR: Assign an prefab to be spawned.");
            return;
        }

        if (singlePlacement)
        {
            PlaceOneAt(hitPoint, hitNormal);
        }
        else
        {
            Vector3 hitPoint_random = hitPoint;
            Vector3 hitNormal_random = hitNormal;
            // Random Distribution
            for (int i = 0; i < density; i++)
            {
                // Raycast on to terrain
                Vector2 randomPoint = Random.insideUnitCircle * radius;
                Vector3 rayStart = hitPoint + hitNormal * 5f + new Vector3(randomPoint.x, 0, randomPoint.y);

                Ray ray = new Ray(rayStart, -hitNormal);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    hitPoint_random = hit.point;
                    hitNormal_random = hit.normal;
                }
                PlaceOneAt(hitPoint_random, hitNormal_random);
            }
        }
    }

    private void PlaceOneAt(Vector3 center, Vector3 normal)
    {
        GameObject newObject = Instantiate(prefab, center, Quaternion.identity);
        // Apply the random scale
        Vector3 newScale = Vector3.one * Random.Range(scaleMin, scaleMax);
        newObject.transform.localScale = newScale;
        // Align along normal
        newObject.transform.up = normal;
        // Random rotation around y
        if (randomYaw) { 
            newObject.transform.Rotate(new Vector3(0, 1, 0), Random.Range(0, 360));
        }
        newObject.transform.Rotate(new Vector3(1, 0, 0), Random.Range(0, 360));
        newObject.transform.Rotate(new Vector3(0, 0, 1), Random.Range(0, 360));
    }
}
