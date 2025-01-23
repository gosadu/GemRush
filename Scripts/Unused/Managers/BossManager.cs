using UnityEngine;

public class BossManager : MonoBehaviour
{
    public int maxHP = 500;
    public int currentHP = 500;

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        Debug.Log("[BossManager] Boss took " + dmg + " damage, HP=" + currentHP);
    }
}
