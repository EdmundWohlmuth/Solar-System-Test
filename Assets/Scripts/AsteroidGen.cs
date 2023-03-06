using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGen : MonoBehaviour
{
    [SerializeField] int debriesAmmount = 100;
    [SerializeField] GameObject debris;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < debriesAmmount; i++)
        {
            Vector3 startPos = new Vector3(Random.Range(-100, 100), Random.Range(-10, 10), Random.Range(-100, 100));

            GameObject Planetoid = Instantiate(debris, debris.transform.position = startPos, debris.transform.rotation);
            Planetoid.name = "Planetoid" + " " + i.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
