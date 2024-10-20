using UnityEngine;

public class EnemyBuildInfo
{
    public Sprite Sprite { get; set; }

    public Color Color { get; set; } = Color.white;

    public float Health { get; set; } = 1;
    public float Damage { get; set; } = 1;
    public float Score { get; set; } = 1;
    public float Speed { get; set; } = 1;
    public float Scale { get; set; } = 1;
}