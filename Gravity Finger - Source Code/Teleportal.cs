using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleportal : MonoBehaviour
{
    private Transform _in, _exit;

    void Start ()
    {
        foreach (Transform t in transform.parent)
        {
            if(t.name == "Exit")
                _exit = t;
            else if (t.name == "In")
                _in = t;
        }
    }

    void Update()
    {
        int speed = 30;
        _in.Rotate(Vector3.forward * speed * Time.deltaTime);
        _exit.Rotate(Vector3.back * speed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            AudioComponent.GetInstance.PlayTeleportal();
            col.transform.position = _exit.position;
        }
    }
}
