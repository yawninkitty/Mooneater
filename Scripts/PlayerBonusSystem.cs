using System;
using UnityEngine;
public static class PlayerBonusSystem
{
    public static int Health = 0;
    public static int Damage = 0;
    public static int Magnet = 0;

    public static event Action<int> OnBonusesChanged;
    public static void ResetToCheckpointState()
    {
        Health = DataContainer.health;
        Damage = DataContainer.damage;
        Magnet = DataContainer.magnet;
        OnBonusesChanged?.Invoke(Health);
        OnBonusesChanged?.Invoke(Damage);
        OnBonusesChanged?.Invoke(Magnet);

    }
}
