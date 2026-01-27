using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData", menuName = "ScriptableObjects/PlanetData")]
public class PlanetData : ScriptableObject
{
    [Header("Planet Settings")]
    public float MaxHealth = 100f;
    public Material PlanetMaterial;

    [Header("Rotation Settings")]
    public float RotationSpeed = 30f;
}
