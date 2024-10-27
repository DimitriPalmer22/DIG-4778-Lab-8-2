using System;
using UnityEngine;

public class Player : Actor
{
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
}