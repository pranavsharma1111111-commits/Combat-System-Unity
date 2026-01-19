using UnityEngine;

public class PlayerCombatStats : MonoBehaviour
{
    [Header("Combat Stats")]
    public int attackDamage = 1;
    public int defense = 0;

    [Header("Equipment")]
    public bool hasWeapon = false;

    public void SetAttackDamage(int value)
    {
        attackDamage = value;
    }

    public void SetDefense(int value)
    {
        defense = value;
    }
}
