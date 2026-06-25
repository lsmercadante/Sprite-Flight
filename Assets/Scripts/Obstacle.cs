using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    Rigidbody2D rb;
    public float minSpeed = 150f;
    public float maxSpeed = 300f;
    public float maxSpinSpeed = 10f;
    public GameObject bounceEffect;

    public float absoluteMaxSpeed = 20.0f;

    public float massMultiplier = 1.0f;
    public bool forceSmall = false;

    public float smallSizeMax = 0.75f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float randomSize;
        if (forceSmall)
        {
            randomSize = Random.Range(minSize, smallSizeMax);
        }
        else
        { randomSize = Random.Range(minSize, maxSize);}
        float randomSpeed = Random.Range(minSpeed, maxSpeed)/randomSize;
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.mass = Mathf.Pow(randomSize, 3f) * massMultiplier;

        rb.AddForce(randomDirection * randomSpeed);
        rb.AddTorque(randomTorque);


        transform.localScale = new Vector3(randomSize, randomSize, 1);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject effect = Instantiate(bounceEffect, contactPoint, Quaternion.identity);
        Destroy(effect, 0.5f);
    }
}
