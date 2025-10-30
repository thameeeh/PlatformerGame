using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    //public TMP_Text _countText;
    private int _dataStamp = 0;
    [SerializeField] private GameObject playerLifeGroup;
    [SerializeField] private GameObject playerIcon;

    [SerializeField] private GameObject coinGroup;
    [SerializeField] private GameObject coinIcon;

    // Update is called once per frame
    void Update()
    {
        int dataStamp = RuntimeGameDataManager.instance.GetDataStamp();
        if (dataStamp != _dataStamp)
        {
            int count = RuntimeGameDataManager.instance.GetCount();
            //_countText.text = count.ToString();
            UpdateGroupIcon(playerLifeGroup, playerIcon, RuntimeGameDataManager.instance.GetPlayerLife());

            UpdateGroupIcon(coinGroup, coinIcon, RuntimeGameDataManager.instance.GetCoins());

            _dataStamp = dataStamp;
        }
    }

    // Internal helper: adjust targetGroup to have exactly count instances of prefab
    private void UpdateGroupIcon(GameObject targetGroup, GameObject prefab, int count)
    {
        if (targetGroup == null || prefab == null) return;

        int currentCount = targetGroup.transform.childCount;

        // Add more if needed
        while (currentCount < count) {
            GameObject newBox = Instantiate(prefab, targetGroup.transform);
            currentCount++;
        }

        // Remove extras if needed
        while (currentCount > count) {
            Transform child = targetGroup.transform.GetChild(currentCount - 1);
            DestroyImmediate(child.gameObject); // Use DestroyImmediate for editor and playmode compatibility
            currentCount--;
        }
    }
}