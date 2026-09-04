using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game Data/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHealth;
    public float moveSpeed;
    public float damageToPlayer;
    public int xpDropAmount;
    public int goldDropAmount;
}