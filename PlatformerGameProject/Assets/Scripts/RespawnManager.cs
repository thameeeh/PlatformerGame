using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 3f;
    private GameObject player;
    private CharacterController2D playerController;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<CharacterController2D>();
    }

    private void OnEnable()
    {
        playerController.OnPlayerDied += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        playerController.OnPlayerDied -= HandlePlayerDeath;
    }

    private void Start()
    {
        RuntimeGameDataManager.instance.SetPlayerLife(3); // Ensure player life is initialized
        RuntimeGameDataManager.instance.SetCoins(0);
    }

    private void HandlePlayerDeath()
    {
        {
            
        }
        int playerLife = RuntimeGameDataManager.instance.GetPlayerLife();
        RuntimeGameDataManager.instance.SetPlayerLife(playerLife - 1);
        player.SetActive(false);
        if (playerLife <= 0) {
            // Handle game over logic here
            Debug.Log("Game Over!");
            // Optionally, you can reload the scene or show a game over screen
            return;
        }
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reset player position and state
        player.transform.position = respawnPoint.position;
        player.SetActive(true);
    }
}
