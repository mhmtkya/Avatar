using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;

public class LootPoolManager : MonoBehaviour
{
    public static LootPoolManager Instance;
    
    [Header("Prefabs")]
    public GameObject xpGemPrefab;
    public GameObject goldGemPrefab;
    
    public IObjectPool<GameObject> xpGemPool;
    public IObjectPool<GameObject> goldGemPool;
        
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        xpGemPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(xpGemPrefab),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj)  => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 200,
            maxSize: 1000);
        
        goldGemPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(goldGemPrefab),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj)  => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 200,
            maxSize: 1000);

    }
    
}
