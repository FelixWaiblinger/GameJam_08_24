using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameData _data;
    [SerializeField] TMP_Text _highscore;
    [SerializeField] Button _playButton;
    [SerializeField] Button _quitButton;
    [SerializeField] Transform[] _objects = new Transform[5];
    float[] _turnTime = new float[5]{1f, 1.5f, 2f, 2.5f, 3f};
    float[] _currentTime = new float[5]{0, 0, 0, 0, 0};
    float[] _sign = new float[5]{-1, 1, -1, -1, 1};

    void Start()
    {
        ReaderWriterJSON.LoadFromJSON(ref _data);

        _highscore.text = "Highscore\nWaves: " + _data.Waves + "\nEnemies: " + _data.Enemies;
    }

    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            _currentTime[i] += Time.deltaTime;

            if (_currentTime[i] > _turnTime[i])
            {
                _sign[i] *= -1;
                _currentTime[i] = 0;
                _objects[i].GetChild(0).localScale = new Vector3(1, _sign[i], 1);
            }

            _objects[i].position += Vector3.right * _sign[i];
        }
    }

    public void Play()
    {
        _playButton.interactable = false;
        _quitButton.gameObject.SetActive(false);

        StartCoroutine(SceneTransition());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator SceneTransition()
    {
        for (int i = 0; i < 30; i++)
        {
            _playButton.transform.localScale -= 0.03f * Vector3.one;

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Game");
    }
}
