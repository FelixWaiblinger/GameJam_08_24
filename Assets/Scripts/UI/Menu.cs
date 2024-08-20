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
    [SerializeField] GameObject _lock1;
    [SerializeField] GameObject _lock2;
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
        _selected.Clear();
    }

    void Reload()
    {
        // shady way to update the inventory UI
        ToggleMenu();
        ToggleMenu();
    }

    public void Select(int index)
    {
        if (_selected.Contains(index)) return;

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

        Reload();
    }

    public void Unlock()
    {
        if (_health < 6 || _inventory.Size == 18) return;

        _healthEvent.IntEvent(_health - 5);
        _inventory.Size += 6;

        if (_inventory.Size == 12) _lock1.SetActive(false);
        else if (_inventory.Size == 18) _lock2.SetActive(false);

        Reload();
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
