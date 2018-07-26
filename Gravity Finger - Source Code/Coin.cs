using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private float _y;
    private float _startY;
    private float _x;

    void Start()
    {
        _y = 0.25f;
        _x = 0.5f;

        _startY = transform.position.y;
    }

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(_x, 0, 0));
        transform.position = new Vector3(transform.position.x, _startY + Mathf.PingPong(Time.time / 6, _y), transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.transform.GetComponent<Player>().AddCoin();
            AudioComponent.GetInstance.PlayCoin();
            Destroy(gameObject);
        }
    }
}
