using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    public string Filename { get; private set; } = "Data";
    public int Waves;
    public int Enemies;
}
