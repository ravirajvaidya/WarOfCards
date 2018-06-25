using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayersId
{
    none = -1,
    p1 = 0,
    p2,
    p3,
    p4
}

[System.Serializable]
public class Player
{
    public PlayersId mPlayersId;
    public List<Card> mListOfCard;

    public Player()
    {
        this.mPlayersId = PlayersId.none;
        this.mListOfCard = new List<Card>();
    }
}