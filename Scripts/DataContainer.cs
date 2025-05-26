using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataContainer
{
    // Ключи для PlayerPrefs
    private const string CoinAmountKey = "PlayerCoinAmount";
    private const string HealthBonusKey = "PlayerHealthBonus";
    private const string DamageBonusKey = "PlayerDamageBonus";
    private const string MagnetBonusKey = "PlayerMagnetBonus";
    private const string LevelIndexKey = "PlayerLevelIndex";

    //уровни
    public static int levelIndex = 0;

    // Чекпоинты и враги
    public static int checkpointIndex = 0;
    public static List<string> killedEnemies = new List<string>();
    public static List<string> pendingKilledEnemies = new List<string>();

    // Moonlight
    public static List<string> collectedMoonlight = new List<string>();
    public static List<string> pendingCollectedMoonlight = new List<string>();

    // Валюта
    public static int coinAmount = 0;
    public static int pendingCoinAmount = 0;

    //Бонусы
    public static int health = 0;
    public static int pendingHealth = 0;

    public static int damage = 0;
    public static int pendingDamage = 0;

    public static int magnet = 0;
    public static int pendingMagnet = 0;

    public static void UpdateCheckpoint(int newIndex)
    {
        if (newIndex != checkpointIndex)
        {
            checkpointIndex = newIndex;

            // Враги
            killedEnemies.AddRange(pendingKilledEnemies);
            pendingKilledEnemies.Clear();

            // Moonlight
            collectedMoonlight.AddRange(pendingCollectedMoonlight);
            pendingCollectedMoonlight.Clear();

            // Валюта
            coinAmount = pendingCoinAmount;

            //Бонусы
            health = pendingHealth;
            damage = pendingDamage;
            magnet = pendingMagnet;
        }
    }

    public static void ClearPendings()
    {
        LoadPlayerData();
        killedEnemies.Clear();
        collectedMoonlight.Clear();
        pendingKilledEnemies.Clear();
        pendingCollectedMoonlight.Clear();

    }

    public static void UpdateLevel(int newIndex)
    {
        if (newIndex != levelIndex)
        {
            Debug.Log($"Обновился уровень {newIndex}");
            levelIndex = newIndex;

            coinAmount = pendingCoinAmount;
            health = pendingHealth;
            damage = pendingDamage;
            magnet = pendingMagnet;

            killedEnemies = new List<string>();
            pendingKilledEnemies = new List<string>();
            collectedMoonlight = new List<string>();
            pendingCollectedMoonlight = new List<string>();


            SavePlayerData(); // Автоматически сохраняем данные при смене уровня
        }
    }

    // Сохраняет данные игрока в PlayerPrefs
    public static void SavePlayerData()
    {
        PlayerPrefs.SetInt(CoinAmountKey, coinAmount);
        PlayerPrefs.SetInt(HealthBonusKey, health);
        PlayerPrefs.SetInt(DamageBonusKey, damage);
        PlayerPrefs.SetInt(MagnetBonusKey, magnet);
        PlayerPrefs.SetInt(LevelIndexKey, levelIndex);
        PlayerPrefs.Save();

        Debug.Log("Данные игрока сохранены");
    }

    // Загружает данные игрока из PlayerPrefs
    public static void LoadPlayerData()
    {
        coinAmount = PlayerPrefs.GetInt(CoinAmountKey, 0); // 0 - значение по умолчанию, если ключ не найден
        health = PlayerPrefs.GetInt(HealthBonusKey, 0);
        damage = PlayerPrefs.GetInt(DamageBonusKey, 0);
        magnet = PlayerPrefs.GetInt(MagnetBonusKey, 0);
        levelIndex = PlayerPrefs.GetInt(LevelIndexKey, 0);

        // Обновляем pending значения, чтобы они соответствовали загруженным
        pendingCoinAmount = coinAmount;
        pendingHealth = health;
        pendingDamage = damage;
        pendingMagnet = magnet;

        Debug.Log($"Данные игрока загружены {coinAmount}");
    }

    // Очищает все сохраненные данные (для тестирования или сброса прогресса)
    public static void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        coinAmount = 0;
        health = 0;
        damage = 0;
        magnet = 0;
        levelIndex = 0;

        pendingCoinAmount = 0;
        pendingHealth = 0;
        pendingDamage = 0;
        pendingMagnet = 0;

        Debug.Log("Все данные игрока очищены");
    }
}