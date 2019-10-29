using UnityEngine;

public interface IEnemy
{
    void Damage(GameObject player, int attackDamage);
    void Stun();
    void Spawn();
    void Hide();
}
