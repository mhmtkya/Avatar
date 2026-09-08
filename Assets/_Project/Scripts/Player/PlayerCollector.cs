using System;
using UnityEngine;



public class PlayerCollector : MonoBehaviour
{
    public LayerMask itemLayer;
    
    private PlayerStats playerStats;
    private float checkInterval = 0.15f;
    private float timer;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            checkForItems();
            timer = checkInterval;
        }
    }

    private void checkForItems()
    {
        float currentRange = playerStats.GetStat(StatType.PickupRange);
        
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, currentRange, itemLayer);

        foreach (Collider2D itemCollider in items)
        {
            
            XpPickup xp = itemCollider.GetComponent<XpPickup>();
            if (xp != null)
            {
                xp.StartFollowing(transform);
                continue;
            }
            
            GoldPickup gold = itemCollider.GetComponent<GoldPickup>();
            if (gold != null)
            {
                gold.StartFollowing(transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && playerStats != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, playerStats.GetStat(StatType.PickupRange));
        }
    }
}
