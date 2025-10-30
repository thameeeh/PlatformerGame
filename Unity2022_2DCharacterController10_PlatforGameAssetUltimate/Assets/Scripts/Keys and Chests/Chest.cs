using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private string _requiredKeyId = "first_key";
    [SerializeField] private bool _openOnTouch = true;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _lootToSpawn;

    private bool _isOpen;

    private void Reset()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void TryOpen()
    {
        if (_isOpen) return;
        
        if (RuntimeGameDataManager.instance.HasKey(_requiredKeyId))
        {
            Open();
        }
        else
        {
            Debug.Log($"Need {_requiredKeyId} key.");
        }
    }

    private void Open()
    {
        _isOpen = true;

        if(_animator != null)
        {
            _animator.SetTrigger("Open");
        }
    }
}
