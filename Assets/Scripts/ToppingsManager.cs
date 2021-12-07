using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingsManager : MonoBehaviour
{
    public Transform spawnPoint;
    public float spawnVolumeRadius;
    //public List<GameObject> toppings;

    //spawn topping with index i
    public void SpawnToppings(GameObject prefab, int spawnNum, List<Material> matList)
    {
        spawnPoint.GetComponent<AudioSource>().Play();
        GameObject parent = new GameObject("topping "+ prefab.name);
        bool randomColor = false;
        if (matList.Count > 0)
        {
            randomColor = true;
        }

        if(spawnNum == 1)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity, parent.transform);
        }
        else
        {
            for (int i = 0; i < spawnNum; i++)
            {
                var offset = new Vector3(Random.value * spawnVolumeRadius, Random.value * spawnVolumeRadius, Random.value * spawnVolumeRadius);

                GameObject newObj = Instantiate(prefab, spawnPoint.position + offset, Quaternion.identity, parent.transform);
                if (randomColor)
                    newObj.GetComponent<MeshRenderer>().material = matList[Random.Range(0, matList.Count)];
            }
        }
    }
}
