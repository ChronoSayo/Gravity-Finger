using UnityEngine;
using System.Collections;

public class BlackHoleArea : MonoBehaviour
{
    public Vector2 gravity;
    
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Transform player = col.transform;
            Vector2 v = new Vector3(gravity.x, gravity.y, player.position.z);
            player.GetComponent<Rigidbody2D>().AddForce((gravity - v).normalized + gravity);
            player.GetComponent<Rigidbody2D>().AddTorque((gravity - v).magnitude / 100);
        }
    }
}
