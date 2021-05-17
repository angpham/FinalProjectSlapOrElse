using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Card[] deck;

    public enum CardSuit
    {
        Hearts = 0,
        Diamonds = 1,
        Clubs = 2,
        Spades = 3,
        None = 4
    }

    public enum CardValue
    {
        Ace = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7,
        Nine = 8,
        Ten = 9,
        Jack = 10,
        Queen = 11,
        King = 12,
        None = 13
    }
}
