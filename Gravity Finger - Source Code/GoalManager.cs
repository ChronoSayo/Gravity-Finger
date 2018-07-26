using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalManager : MonoBehaviour
{
    public float gravity = 1;

    private List<PlayerInfo> _players;

    struct PlayerInfo
    {
        public Transform player;
        public bool inside;
        public Vector3 lightHolePosition;
    }

    void Start()
    {
        _players = new List<PlayerInfo>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerInfo temp = new PlayerInfo();
            temp.player = go.transform;
            temp.inside = false;
            temp.lightHolePosition = Vector3.zero;
            _players.Add(temp);
        }
    }

    void Update()
    {
        OneInsideLightHole();
    }

    void FixedUpdate()
    {
        if (_players.Count > 0)
        {
            foreach (PlayerInfo pi in _players)
            {
                if (pi.inside)
                {
                    pi.player.GetComponent<Rigidbody2D>().AddForce((pi.lightHolePosition - pi.player.position).normalized * gravity);
                    pi.player.GetComponent<Rigidbody2D>().AddTorque((pi.lightHolePosition - pi.player.position).magnitude / 100);
                }
            }
        }
    }

    public void Inside(int ID, Vector3 lightHolePos, bool inside)
    {
        PlayerInfo outside = GetPlayerInfoFromID(ID);
        if (outside.player != null)
        {
            PlayerInfo temp = outside;
            temp.inside = inside;
            temp.lightHolePosition = lightHolePos;
            _players[GetIndexFromID(ID)] = temp;
        }
    }

    private int GetIndexFromID(int ID)
    {
        int index = -1;
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].player.GetInstanceID() == ID)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    private PlayerInfo GetPlayerInfoFromID(int ID)
    {
        PlayerInfo temp = new PlayerInfo();
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].player.GetInstanceID() == ID)
            {
                temp = _players[i];
                break;
            }
        }
        return temp;
    }

    public bool AllInsideLightHole
    {
        get
        {
            bool allInside = true;
            if (_players.Count > 0)
            {
                foreach (PlayerInfo pi in _players)
                {
                    if (!pi.inside)
                    {
                        allInside = false;
                        break;
                    }
                }
            }
            else
                allInside = false;

            return allInside;
        }
    }

    public void OneInsideLightHole()
    {
        bool oneInside = false;
        if (_players.Count > 0)
        {
            foreach (PlayerInfo pi in _players)
            {
                if (pi.inside)
                {
                    oneInside = true;
                    break;
                }
            }
        }

        if(oneInside)
            AudioComponent.GetInstance.PlayLightHole();
        else if (!oneInside)
            AudioComponent.GetInstance.StopLightHole();
    }
}
