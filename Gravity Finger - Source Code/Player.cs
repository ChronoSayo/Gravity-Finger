using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using Tobii.EyeTracking;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Gravity _gravityScript;
    private Timer _timerScript;
    private ScoreMode _scoreModeScript;
    private int _currentCoins;
    private float _multiTick, _cheatTime, _levelTime;
    private bool _mainMode;

    public static int COINS;
    public static bool EYE_TRACKER = false;

    private const int MAXSPEED = 40;

    void Start()
    {   
        _gravityScript = GameObject.Find("Gravity").GetComponent<Gravity>();

        _mainMode = SceneManager.GetActiveScene().name.Contains("Level") || SceneManager.GetActiveScene().name.Contains("level");

        if (_mainMode)
            _timerScript = GameObject.Find("Timer").GetComponent<Timer>();
        else if(SceneManager.GetActiveScene().name == "ScoreMode")
            _scoreModeScript = GameObject.Find("Score Mode").GetComponent<ScoreMode>();

        _rigidbody = GetComponent<Rigidbody2D>();

        _multiTick = 0;
        _cheatTime = 4;
        _levelTime = 1.5f;
    }

    void Update()
    {
        Reset();
        GameEnd();
        ChooseLevel();
        Quit();

        Cheats();
    }

    void FixedUpdate()
    {
        AddingForce();
        MaxSpeedLimit();
    }

    private void Cheats()
    {
        if (Input.GetKey(KeyCode.Backspace) || Input.touchCount == 4)
        {
            if (MultiTouchTimer(_cheatTime))
            {
                if(_mainMode)
                    SceneManager.LoadScene("ScoreMode");
                else
                    SceneManager.LoadScene("Menu");
            }
        }
        if (Input.GetKey(KeyCode.Return) || Input.touchCount == 5)
        {
            if(MultiTouchTimer(_cheatTime))
                Time.timeScale = Time.timeScale == 1 ? 2 : 1;
        }
    }

    private void ChooseLevel()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            NextLevel(false);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextLevel(true);
    }

    private void NextLevel(bool next)
    {
        int i = SceneManager.GetActiveScene().buildIndex;
        if (next)
        {
            i++;
            i = i % (SceneManager.sceneCountInBuildSettings - 1);
        }
        else
        {
            i--;
            i = i < 0 ? SceneManager.sceneCountInBuildSettings - 2 : i;
        }
        SceneManager.LoadScene(i);
    }

    private void GameEnd()
    {
        if (!_mainMode)
            return;
        if (_timerScript.GameEnd && _rigidbody.constraints != RigidbodyConstraints2D.FreezeAll)
        {
            FreezeBox();
            COINS += _currentCoins;
        }
    }

    public void FreezeBox()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Reset()
    {
        if (Input.GetKey(KeyCode.Space) || Input.touchCount == 2)
        {
            if(MultiTouchTimer(_levelTime))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Quit()
    {
        if (Input.GetKey(KeyCode.Escape) || Input.touchCount == 3)
        {
            if (MultiTouchTimer(_levelTime))
            {
                if (!_mainMode)
                    Application.Quit();
                else
                    SceneManager.LoadScene("Menu");
            }
        }
    }

    private bool MultiTouchTimer(float time)
    {
        _multiTick += Time.deltaTime;
        if (_multiTick >= time)
            return true;
        return false;
    }

    private void AddingForce()
    {
        bool addForce = (EYE_TRACKER && EyeTracking.GetGazePoint().IsValid) ||
            (!EYE_TRACKER && _gravityScript.ButtonClick);
        if (addForce)
        {
            _rigidbody.AddForce((_gravityScript.NewPosition - transform.position).normalized * _gravityScript.GetGravity);
            _rigidbody.AddTorque((_gravityScript.NewPosition - transform.position).magnitude / 100);
        }
    }

    public void MaxSpeedLimit()
    {
        _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, MAXSPEED);
    }

    public void AddCoin()
    {
        _currentCoins++;
    }

    private int GetCoins
    {
        get { return _currentCoins; }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Anti Gravity")
            AudioComponent.GetInstance.PlayThudAntiGravity();
        else if (col.transform.tag == "Player")
            AudioComponent.GetInstance.PlayThudPlayer();
        else
            AudioComponent.GetInstance.PlayThudWall();

        if (SceneManager.GetActiveScene().name == "ScoreMode")
        {
            if (col.transform.tag != "Player")
                _scoreModeScript.WallHit();
        }
    }
}
