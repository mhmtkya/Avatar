using UnityEngine;
using UnityEngine.Rendering;

public class LootPoolManager : MonoBehaviour
{
    public static LootPoolManager Instance;
    
    [Header("Prefabs")]
    public GameObject xpGemPrefab;
    public GameObject goldGemPrefab;
    
    public ObjectPool<GameObject> xpPool;
    public ObjectPool<GameObject> goldPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }
    
}
