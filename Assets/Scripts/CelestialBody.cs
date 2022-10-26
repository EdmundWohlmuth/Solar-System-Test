using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    const float G = 6.674f;

    public Rigidbody rb;
    [Header("Orbital info")]
    public float density;
    public float orbitRadius;
    public float orbitTime;

    [Header("fun facts")]
    public float force;
    public float orbitalDistance;

    [Header("Bools")]
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
        SetVelocity();
    }

    void FixedUpdate()
    {
        CelestialBody[] Bodies = FindObjectsOfType<CelestialBody>();
        foreach (CelestialBody body in Bodies)
        {
            if (body != this) // to stop body from calcualting is gravitational affect on itself
            {
                Gravity(body);
            }
        }
    }

    void Gravity(CelestialBody objToAttract)
    {
        Rigidbody rbToAttact = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttact.position; // find direction to celectial body
        float distance = direction.magnitude;
        orbitalDistance = distance; // distance to the sun

        float forceMagnitude = G * (rb.mass * rbToAttact.mass) / Mathf.Pow(distance, 2); // calculate gravitational force
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttact.AddForce(force); // apply gravity
    }

    void SetVelocity()
    {
        if (!isSun)
        {
            GameObject sun = GameObject.Find("Sun");
            Rigidbody sunPos = sun.GetComponent<Rigidbody>();

            Vector3 sunDirection = rb.position - sunPos.position; // distacne to the sun
            float distance = sunDirection.magnitude;

            //velocity = (2 * (Mathf.PI) * distance) / orbitTime; // lucas calculation

            force = Mathf.Sqrt((G * (sunPos.mass * rb.mass)) / distance); // circular orbit
            force = force * 400;

            float angleToSun = Vector3.Angle(transform.position, sun.transform.position);
            Debug.Log(gameObject + "angle to sun" + angleToSun);

            AddForceAtAngle(force, angleToSun);
        }
    }

    void AddForceAtAngle(float force, float angle)
    {
        float x = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float y = Mathf.Sin(angle * Mathf.PI / 180) * force;

        rb.AddForce(y, 0, x);
    }
}
