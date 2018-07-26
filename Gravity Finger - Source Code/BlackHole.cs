using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
    public float gravity;
    
    private Camera _camera;
    private GameObject[] _players;

    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");

        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        _camera.GetComponent<CallVortex>().AddVortex(new Vector2((transform.localScale.x / 100) * 4.5f,
            (transform.localScale.x / 100) * 4), _camera.WorldToViewportPoint(transform.position + (Vector3.up * 0.6f)), 10);
    }

    void FixedUpdate()
    {
        foreach (GameObject go in _players)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (go.transform.position - transform.position), 
                Vector2.Distance(transform.position, go.transform.position));
            bool hitAntiGravity = hit && hit.transform.tag == "Anti Gravity" && hit.transform.tag != "Player" && hit.transform.tag != "Emerald";

            if (!hitAntiGravity)
            {
                go.GetComponent<Rigidbody2D>().AddForce((transform.position - go.transform.position).normalized * gravity);
                go.GetComponent<Rigidbody2D>().AddTorque((transform.position - go.transform.position).magnitude / 100);
            }
        }
    }
}
