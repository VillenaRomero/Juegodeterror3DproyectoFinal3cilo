using System.Collections.Generic;
using UnityEngine;

public class BoxPlayer : MonoBehaviour
{
    public string playerName;
    public bool isHuman = false;
    public bool isAlive = true;
    public bool hasShield = false;

    public Card[] hand = new Card[10];
    public int handCount = 0;

    public void AddCard(Card card)
    {
        if (handCount < hand.Length)
        {
            hand[handCount] = card;
            handCount++;
        }
    }

    public void RemoveCard(int index)
    {
        if (index < 0 || index >= handCount) return;

        for (int i = index; i < handCount - 1; i++)
        {
            hand[i] = hand[i + 1];
        }

        hand[handCount - 1] = null;
        handCount--;
    }

    public Card GetFirstCard()
    {
        if (handCount > 0) return hand[0];
        return null;
    }
}
