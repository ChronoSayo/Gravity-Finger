using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class CallVortex : MonoBehaviour
{
    public Shader vortexShader;

    private List<Transform> _players;
    private List<VortexData> _vortices;

    public struct VortexData
    {
        public Vortex vortex;
        public float slow;
    }

    void Awake()
    {
        _vortices = new List<VortexData>();
    }

    void Start()
    {
        _players = new List<Transform>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            _players.Add(go.transform);
    }
    
    void Update ()
    {
        PingPong();
        PlayerInVortex();
    }

    private void PlayerInVortex()
    {
        bool inside = false;
        foreach(Transform t in _players)
        {
            foreach (VortexData vd in _vortices)
            {
                if (Vector2.Distance(t.position, GetComponent<Camera>().ViewportToWorldPoint(vd.vortex.center)) < 3)
                {
                    inside = true;
                    break;
                }
            }
        }

        if (!AudioComponent.GetInstance.IsPlayingVortex() && inside)
            AudioComponent.GetInstance.PlayVortex();
        if (AudioComponent.GetInstance.IsPlayingVortex() && !inside)
            AudioComponent.GetInstance.StopVortex();
    }

    private void PingPong()
    {
        if (_vortices.Count > 0)
        {
            foreach (VortexData v in _vortices)
                v.vortex.angle = (1440 * Mathf.PingPong(Time.time / v.slow, 1)) - 720;
        }
    }

    public VortexData AddVortex(Vector2 radius, Vector2 center, float slow, bool enabled = true)
    {
        VortexData temp = new VortexData();
        Vortex v = gameObject.AddComponent<Vortex>();
        v.shader = vortexShader;
        v.enabled = enabled;
        v.radius = radius;
        v.center = center;

        temp.vortex = v;
        temp.slow = slow;
        _vortices.Add(temp);

        return temp;
    }
}
