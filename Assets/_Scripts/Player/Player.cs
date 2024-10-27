using System;
using System.IO;
using UnityEngine;

public class Player : Actor
{
    public override ActorData ActorData => new PlayerData(this);

    protected override void CustomStart()
    {
        OnHit += _ => ScoreManager.Instance.UpdateText();

        // Update the score text
        ScoreManager.Instance.UpdateText();
    }

    protected override void Die()
    {
        // Set the game over text to active
        ScoreManager.Instance.SetGameOverText(true);
    }

    protected override void CustomLoad(string path)
    {
    }
}

[Serializable]
public class PlayerData : ActorData
{
    public PlayerData(Player player) : base(player)
    {
    }
}