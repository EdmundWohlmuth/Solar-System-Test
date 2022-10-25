using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    const float G = 667.4f;

    public Rigidbody rb;
    public float density;

    public bool randomDensity;
    public bool isSun;

    private void Start()
    {
        if (randomDensity)
        {
            density = Random.Range(0.01f, 0.1f); // TEMP
        }

        float radius;
        float radiusCubed;
        float volume;

        radius = transform.localScale.x / 2;
        radiusCubed = Mathf.Pow(radius, 3);
        volume = (4/3) * Mathf.PI * radiusCubed;

        rb.mass = density * volume;
    }

    void FixedUpdate()
    {
        CelestialBody[] Bodies = FindObjectsOfType<CelestialBody>();
        foreach (CelestialBody body in Bodies)
        {
            if (body != this)
            {
                Gravity(body);
            }
        }
    }

    void Gravity(CelestialBody objToAttract)
    {
        Rigidbody rbToAttact = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttact.position;
        float distance = direction.magnitude;

        float forceMagnitude = G * (rb.mass * rbToAttact.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttact.AddForce(force);
    }

    void SetVelocity()
    {
        if (!isSun)
        {
            GameObject sun = GameObject.Find("Sun");
            Rigidbody sunPos = sun.GetComponent<Rigidbody>();

            Vector3 direction = rb.position - sunPos.position;
            float distance = direction.magnitude;

            float velocity = Mathf.Sqrt((G * rb.mass) / distance);
            Vector3 force = direction.normalized * velocity;
            rb.AddForce(force);
        }
    }
}
