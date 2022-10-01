using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event", order = 0)]
public class GameEvent : ScriptableObject
{
    public string description;
    public float probability = 1f;

    [Header("Effects")]
    [Range(-0.5f, 0.5f)]
    public float tempPowerDemand = 0f;
    [Range(-1f, 1f)]
    public float tempWind = 0f;
    [Range(-1f, 1f)]
    public float rain = 0f;
    [Range(0f, 1f)]
    public float tempHeat = 0f;
    [Range(0f, 1f)]
    public float deltaCarbonTax = 0.0f;
    [Range(-0.25f, 0.5f)]
    public float deltaGasPrice = 0f;
}