using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScoreMode : MonoBehaviour
{
    public float timeLimit;
    public float spawnTime;

    private GameObject _player;
    private MemoryCard _memoryCard;
    private Text _timerText, _winTimerText;
    private float _timeLimit, _multiPlayerScoreScale, _tick, _gameEndTime, _addPlayerTime;
    private int _score, _boxCount;
    private bool _gameEnd;
    private List<Transform> _players;

    public static int TOTALSCORE = 0;

    void Start ()
    {
        _timerText = GameObject.Find("TimerText").GetComponent<Text>();
        _timeLimit = timeLimit;
        _winTimerText = GameObject.Find("Result").GetComponent<Text>();

        _memoryCard = GameObject.FindGameObjectWithTag("MemoryCard").GetComponent<MemoryCard>();

        _players = new List<Transform>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _players.Add(_player.transform);

        _multiPlayerScoreScale = 0;

        _tick = 0;
        _gameEndTime = 1.5f;

        InvokeRepeating("AddNewPlayer", 15, spawnTime);
    }
    
    void Update ()
    {
        TimerTick();
        ScoreCounter();
        HandleGameEnd();
    }

    public void WallHit()
    {
        _score -= 1000;
    }

    private void ScoreCounter()
    {
        if (!_gameEnd)
        {
            _score += (int)GetPlayersSpeed();
            _winTimerText.text = _score.ToString();
        }
    }

    private void TimerTick()
    {
        if (_timeLimit <= 0)
        {
            _gameEnd = true;
            _winTimerText.text = "FINISH";
            _timerText.text = "0.00";

            foreach (Transform t in _players)
                t.GetComponent<Player>().FreezeBox();

            if (_score > TOTALSCORE)
                TOTALSCORE = _score;
        }
        else
        {
            _timeLimit -= Time.deltaTime / Time.timeScale;
            MinuteFormat();
        }
    }

    private void AddNewPlayer()
    {
        _players.Add(((GameObject)Instantiate(_player, new Vector3(0, 0, 13), Quaternion.identity)).transform);
        _multiPlayerScoreScale++;
    }

    private float GetPlayersSpeed()
    {
        float playerSpeed = 0;

        for (int i = 0; i < _players.Count; i++)
        {
            Transform p = _players[i];
            float baseScale = 5;
            float proximityBonus = Mathf.Abs(p.position.x + p.position.y);
            float velocity = p.GetComponent<Rigidbody2D>().velocity.magnitude;
            if (i > 0)
            {
                float indexScaling = i * _multiPlayerScoreScale;
                float scale = baseScale / indexScaling;
                float offsetScaling = scale / 2;
                float lastScale = baseScale + offsetScaling + proximityBonus;
                playerSpeed += CalculateScaledVelocity(velocity, lastScale, proximityBonus);
            }
            else
                playerSpeed += CalculateScaledVelocity(velocity, baseScale, proximityBonus);
        }

        return playerSpeed;
    }

    private float CalculateScaledVelocity(float velocity, float scale, float proximityBonus)
    {
        float totalScale = (scale - proximityBonus);
        if (totalScale < 1)
            totalScale = 1;
        return velocity / totalScale;
    }

    private void HandleGameEnd()
    {
        if (_gameEnd)
        {
            _tick += Time.deltaTime / Time.timeScale;
            if (_tick >= _gameEndTime)
            {
                _memoryCard.SaveData();
                SceneManager.LoadScene("Menu");
            }
        }
    }

    private void MinuteFormat()
    {
        TimeSpan ts = TimeSpan.FromSeconds(_timeLimit);

        string ms = (ts.Milliseconds / 10).ToString();
        _timerText.text = string.Format("{0}:{1}:{2}", ts.Minutes, ts.Seconds, ms);
    }
}
