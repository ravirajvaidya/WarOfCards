using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    spade = 0,
    hearts,
    clove,
    diamonds
}

[System.Serializable]
public class Card
{
    public int mCardId;
    public PlayersId mCardOfPlayer = PlayersId.none;
    public CardType mCardType;
    public int mValue;

    public Card(int cardId,CardType cardType, int value)
    {
        this.mCardId = cardId;
        this.mCardType = cardType;
        this.mValue = value;
    }
}