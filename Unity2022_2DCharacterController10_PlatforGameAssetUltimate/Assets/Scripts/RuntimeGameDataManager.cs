using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// step 1
public class RuntimeGameDataManager : MonoBehaviour
{
    static public RuntimeGameDataManager instance;
    private int _count = 0;
    private int _dataStamp = 0;
    public int _playerLife = 3;
    public int _coins = 0;

    private HashSet<string> _keys = new HashSet<string>();
    public event Action<string> OnKeyAcquired;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddCount(int c)
    {
        _count += c;
        _dataStamp += 1;
    }

    public int GetCount()
    {
        return _count;
    }

    public int GetDataStamp()
    {
        return _dataStamp;
    }

    public void SetPlayerLife(int c)
    {
        _playerLife = c;
        if (_playerLife < 0)
            _playerLife = 0;
        _dataStamp += 1;
    }

    public int GetPlayerLife()
    {
        return _playerLife;
    }

    public void SetCoins(int c)
    {
        _coins = c;
        if (_coins < 0)
            _coins = 0;
        _dataStamp += 1;
    }

    public int GetCoins()
    {
        return _coins;
    }

    public void AddCoins(int delta)
    {
        _coins += delta;
        if (_coins < 0)
            _coins = 0;
        _dataStamp += 1;
    }

    public bool HasKey(string id) => _keys.Contains(id);
    public void AddKey(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (_keys.Add(id))
        {
            Debug.Log($"Added key: {id}");
            _dataStamp += 1;
            OnKeyAcquired?.Invoke(id);
        }
    }
}
