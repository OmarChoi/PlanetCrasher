using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData", menuName = "ScriptableObjects/PlanetData")]
public class PlanetData : ScriptableObject
{
    [Header("Planet Settings")]
    public float maxHealth = 100f;
    public Sprite planetImage;
    public Material crackMaterial;
}
