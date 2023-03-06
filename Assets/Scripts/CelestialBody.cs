using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    const float G = 66.74f;

    public Rigidbody rb;
    [Header("Orbital info")]
    public float density;
    public float orbitRadius;
    public float orbitTime;

    [Header("fun facts")]
    public float force;
    public float orbitalDistance;
    [SerializeField] float gravitationalForce;

    [Header("Bools")]
    public bool randomDensity;
    public bool isSun;

    private void Start()
    {
        if (randomDensity)
        {
            density = Random.Range(10f, 40f); // TEMP
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
        gravitationalForce = forceMagnitude;
    }

    void SetVelocity()
    {
        if (!isSun)
        {
            GameObject sun = GameObject.Find("Sun");
            Rigidbody sunPos = sun.GetComponent<Rigidbody>();

            //Vector3 sunDirection = rb.position - sunPos.position; // distacne to the sun
            float distance = Vector3.Distance(gameObject.transform.position, sun.transform.position);

            force = (G * rb.mass) * (sunPos.mass) / distance;
            force = Mathf.Sqrt(force) * 200;           

            float angleToSun = Vector3.Angle(transform.position, sun.transform.position);
            //Debug.Log(gameObject + "angle to sun" + angleToSun);

            //AddForceAtAngle(force, angleToSun);
            AddForceForCircularOrbit(distance);
        }
    }

    void AddForceAtAngle(float force, float angle)
    {
        float x = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float y = Mathf.Sin(angle * Mathf.PI / 180) * force;

        rb.AddForce(y, 0, x);
    }

    void AddForceForCircularOrbit(float radius)
    {
        GameObject sun = GameObject.Find("Sun");
        Rigidbody sunPos = sun.GetComponent<Rigidbody>();

        Vector3 direction = (transform.position - sun.transform.position).normalized;
        Vector3 perpendicular = new Vector3(direction.y, -direction.x, 0);
        float speed = Mathf.Sqrt(G * sunPos.mass / radius);
        Vector3 velocity = perpendicular * speed;

        rb.velocity = velocity;
        /*Debug.Log(gameObject.name + " Perpendicular: " + perpendicular);
        Debug.Log(gameObject.name + " Speed: " + speed);
        Debug.Log(gameObject.name + " Velocity: " + velocity);
        Debug.Log(" ");*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag != "sun")
        {
            if (collision.gameObject.tag == "sun")
            {
                if (gameObject.GetComponent<Rigidbody>().mass > 0.05f) Debug.LogWarning(gameObject.name + "collided with the sun!");
                Destroy(gameObject);
            }
            else
            {
                // add mass and destory if smaller object
                if (collision.gameObject.GetComponent<Rigidbody>().mass >= gameObject.GetComponent<Rigidbody>().mass)
                {
                    collision.gameObject.GetComponent<Rigidbody>().mass += gameObject.GetComponent<Rigidbody>().mass;
                    collision.gameObject.transform.localScale += (gameObject.transform.localScale / 0.6f);
                    collision.gameObject.GetComponent<CelestialBody>().density += gameObject.GetComponent<CelestialBody>().density;
                    Destroy(gameObject);
                }
                Debug.LogWarning(gameObject.name + " collided with " + collision.gameObject.name + "!");
            }
        }
    }
}
