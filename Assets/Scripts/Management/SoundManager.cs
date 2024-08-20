using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] BoolEventChannel _modeEvent;
    [SerializeField] AudioSource _chill;
    [SerializeField] AudioSource _fight;
    [SerializeField] float _blendSpeed;
    [SerializeField] float _chillMaxVolume;
    [SerializeField] float _fightMaxVolume;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        _modeEvent.OnBoolEvent += ChangeMusic;
        SceneManager.sceneLoaded += ChangeMusic;
    }

    void OnDisable()
    {
        _modeEvent.OnBoolEvent -= ChangeMusic;
        SceneManager.sceneLoaded -= ChangeMusic;
    }

    void ChangeMusic(bool fight)
    {
        var sign = fight ? 1 : -1;
        StartCoroutine(VolumeCoroutine(_fight, sign, _fightMaxVolume));
        StartCoroutine(VolumeCoroutine(_chill, -sign, _chillMaxVolume));
    }

    void ChangeMusic(Scene next, LoadSceneMode mode)
    {
        ChangeMusic(next.name == "Game");
    }

    IEnumerator VolumeCoroutine(AudioSource source, float sign, float maxVolume)
    {
        if (sign > 0) source.UnPause();

        while ((sign > 0) ? (source.volume < maxVolume) : (source.volume > 0))
        {
            source.volume = Mathf.Clamp(source.volume + _blendSpeed * sign, 0, maxVolume);
            yield return new WaitForFixedUpdate();
        }

        if (sign < 0) source.Pause();
    }
}
