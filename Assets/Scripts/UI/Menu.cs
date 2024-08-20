using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] VoidEventChannel _deathEvent;
    [SerializeField] IntEventChannel _healthEvent;
    [SerializeField] GameObject _pauseParent;
    [SerializeField] GameObject _deathParent;
    [SerializeField] GameObject _itemParent;
    [SerializeField] Inventory _inventory;
    [SerializeField] List<Button> _slots = new();
    [SerializeField] List<Image> _marks = new();
    List<int> _selected = new();
    int _health = 3;

    void OnEnable()
    {
        InputReader.MenuEvent += ToggleMenu;
        InputReader.PauseEvent += PauseGame;
        _deathEvent.OnVoidEvent += GameOver;
        _healthEvent.OnIntEvent += GetPlayerHealth;
    }

    void OnDisable()
    {
        InputReader.MenuEvent -= ToggleMenu;
        InputReader.PauseEvent -= PauseGame;
        _deathEvent.OnVoidEvent -= GameOver;
        _healthEvent.OnIntEvent -= GetPlayerHealth;
    }

    void ToggleMenu()
    {
        _itemParent.SetActive(!_itemParent.activeSelf);
        
        if (_itemParent.activeSelf)
        {
            for (int i = 0; i < _inventory.Items.Count; i++)
            {
                _slots[i].gameObject.SetActive(true);
                _slots[i].GetComponent<Image>().color = _inventory.Items[i].Color;
            }
        }
        else
        {
            foreach (var slot in _slots)
            {
                slot.gameObject.SetActive(false);
            }
            foreach (var mark in _marks)
            {
                mark.gameObject.SetActive(false);
            }
        }
    }

    public void Select(int index)
    {
        _selected.Add(index);
        _marks[index].gameObject.SetActive(true);
    }

    public void Deselect()
    {
        foreach (var index in _selected)
        {
            _marks[index].gameObject.SetActive(false);
        }
        _selected.Clear();
    }

    public void Convert()
    {
        _healthEvent.IntEvent(_health + _selected.Count);
        _inventory.Remove(_selected);
        _selected.Clear();

        // shady way to update the inventory UI
        ToggleMenu();
        ToggleMenu();
    }

    void GetPlayerHealth(int health)
    {
        _health = health;
    }

    void PauseGame()
    {
        Time.timeScale = 0f;

        _pauseParent.SetActive(true);

        if (_itemParent.activeSelf) ToggleMenu();
    }

    void GameOver()
    {
        _deathParent.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;

        _pauseParent.SetActive(false);
    }

    public void CloseInventory()
    {
        ToggleMenu();
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
