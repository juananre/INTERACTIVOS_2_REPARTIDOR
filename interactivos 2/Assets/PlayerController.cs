using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float speed = 100f;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // mueve el objeto hacia la derecha
        transform.Translate(Vector3.fwd * Time.deltaTime * speed);

    }
}