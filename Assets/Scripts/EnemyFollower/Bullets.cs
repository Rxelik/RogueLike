using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody rb;
    public float speed, destroyTime, rotateSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        Invoke("gameObject.SetActive(false)",destroyTime);
    }
    private void FixedUpdate()
    {
        Bullet();
    }

    private void Bullet()
    {
        Vector3 direction = (Vector3)_player.transform.position - rb.position;
        direction = direction.normalized;
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = new Vector3(0,0,rotateAmount * rotateSpeed);
        rb.velocity = - transform.up * speed;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            gameObject.SetActive(false);
            Debug.Log("HIT");
        }
        else
            gameObject.SetActive(false);
            Debug.Log("HIL");
    }
}