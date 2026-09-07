using System;
using UnityEngine;

public class XpPickup : MonoBehaviour
{
    [HideInInspector]
    public int xpAmount;
    
    private bool isFollowing = false;
    private Transform playerTransform;
    private float moveSpeed = 5f;

    public void StartFollowing(Transform player)
    {
        isFollowing = true;
        playerTransform = player;
    }

    private void Update()
    {
        if (isFollowing && playerTransform != null)
        {
            moveSpeed += Time.deltaTime * 15f;
            
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExperience playerXp = other.GetComponent<PlayerExperience>();

            if (playerXp != null)
            {
                playerXp.AddXp(xpAmount);
                
                Destroy(gameObject);
            }
        }
    }
}
