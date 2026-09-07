using System;
using UnityEngine;



public class PlayerCollector : MonoBehaviour
{
    public LayerMask xpLayer;
    
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
            checkForXp();
            timer = checkInterval;
        }
    }

    private void checkForXp()
    {
        float currentRange = playerStats.GetStat(StatType.PickupRange);
        
        Collider2D[] xps = Physics2D.OverlapCircleAll(transform.position, currentRange, xpLayer);

        foreach (Collider2D xpCollider in xps)
        {
            XpPickup xp = xpCollider.GetComponent<XpPickup>();

            if (xp != null)
            {
                xp.StartFollowing(transform);
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
