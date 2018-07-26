using UnityEngine;
using System.Collections;

public class AudioComponent : MonoBehaviour
{
    public AudioClip ambience, lightHole, vortex, coin, teleportal, thudWall, thudAntiGravity, thudPlayer;

    public static AudioComponent GetInstance;

    private AudioSource _ambient, _lightHole, _vortex, _thud, _coin, _teleportal, _player;
    private float _crossfadeSpeed;

    void Awake()
    {
        GetInstance = this;
    }
    
    void Start()
    {
        _ambient = gameObject.AddComponent<AudioSource>();
        _ambient.clip = ambience;
        _ambient.loop = true;
        _ambient.Play();

        _lightHole = gameObject.AddComponent<AudioSource>();
        _lightHole.clip = lightHole;
        _lightHole.loop = true;
        _lightHole.Play();

        _vortex = gameObject.AddComponent<AudioSource>();
        _vortex.clip = vortex;
        _vortex.loop = true;
        _vortex.Play();
        _vortex.mute = true;

        _coin = gameObject.AddComponent<AudioSource>();
        _thud = gameObject.AddComponent<AudioSource>();
        _player = gameObject.AddComponent<AudioSource>();
        _teleportal = gameObject.AddComponent<AudioSource>();

        _crossfadeSpeed = 0.005f;
    }

    void Update()
    {
    }

    public void PlayCoin()
    {
        _coin.PlayOneShot(coin);
    }

    public void PlayThudWall()
    {
        _thud.PlayOneShot(thudWall);
    }

    public void PlayThudAntiGravity()
    {
        _thud.PlayOneShot(thudAntiGravity);
    }

    public void PlayTeleportal()
    {
        _teleportal.PlayOneShot(teleportal);
    }

    public void PlayThudPlayer()
    {
        _player.clip = thudPlayer;
        _player.Play();
    }

    public void PlayVortex()
    {
        _vortex.mute = false;
    }

    public void StopVortex()
    {
        _vortex.mute = true;
    }

    public bool IsPlayingVortex()
    {
        return !_vortex.mute;
    }

    public void PlayLightHole()
    {
        if(_ambient.volume > 0)
            _ambient.volume -= _crossfadeSpeed;
        if(_lightHole.volume < 1)
            _lightHole.volume += _crossfadeSpeed;
    }

    public void StopLightHole()
    {
        if (_ambient.volume < 1)
            _ambient.volume += _crossfadeSpeed;
        if (_lightHole.volume > 0)
            _lightHole.volume -= _crossfadeSpeed;
    }
}
