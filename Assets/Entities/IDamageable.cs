using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float Health { get; }
    void TakeDamage(float amount);
    void HealDamage(float amount);
    bool IsAlive { get; }   
}
