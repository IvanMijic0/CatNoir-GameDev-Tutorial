using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Behaviours.Combat.Player;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private List<Image> hearts = new();
    [SerializeField] private Sprite emptyHeart;

    private int _maxHitPoints;
    public void SetMaxHitPoints(int hitPoints)
    {
        _maxHitPoints = hitPoints;
    }
    
    public void HealthBarMechanics(PlayerHealth playerHealth)
    {
        if (playerHealth.GetHitPoints() >= _maxHitPoints) return;
        hearts[^1].sprite = emptyHeart;
        hearts.Remove(hearts[^1]);
        _maxHitPoints--;
    }
}
