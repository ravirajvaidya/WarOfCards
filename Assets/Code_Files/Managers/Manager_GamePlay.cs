using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum GamePlayMode
{
    TwoPlayer = 2,
    FourPlayer = 4
}
public class Manager_GamePlay : MonoBehaviour {

    private static Manager_GamePlay instance;
    public static Manager_GamePlay Instance
    {
        get { return instance; }
    }

    public delegate void StartGame();
    public event StartGame OnStartGame;

    public delegate void RoundWinnerDecleared(PlayersId pId);
    public event RoundWinnerDecleared OnRoundWinnerDecleared;

    public delegate void MatchWinnerDecleared(Player player);
    public event MatchWinnerDecleared OnMatchWinnerDecleared;

    public delegate void PlayersTurn(PlayersId pId);
    public event PlayersTurn OnPlayersTurn;

    public delegate void WarModeActivated(int cardsToDraw);
    public event WarModeActivated OnWarModeActivated;

    public GamePlayMode CurrentGamePlayMode
    {
        set
        {
            mCurrentGamePlayMode = value;
        }
    }

    private GamePlayMode mCurrentGamePlayMode = GamePlayMode.TwoPlayer;
    private int mPlayersPlayedThereTurns;
    private int mTotalCardTypes;
    private int mMaxCards;
    private int mMaxCardsPerSet;
    public List<Card> mListOfCards;
    public List<Card> mListPoolOfCards;
    public List<Player> mListOfPlayers;
    public int mNumberOfCardsToDraw = 1;
    public PlayersId mPlayersTurn;
    public List<Card> mListOfCurrentCardsShown;

    public AudioClip mWarStart;
    public AudioClip mWarMode;
    public AudioSource mAudioSource;

    #region UnityDefaults
    // Use this for initialization
    void OnEnable()
    {
        OnMatchWinnerDecleared += EndMatch;
        SetDefaults();
    }

    void Awake()
    {
        instance = this;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDisable()
    {
        OnMatchWinnerDecleared -= EndMatch;
    }
    #endregion

    #region SetUpGame
    void SetDefaults()
    {
        mMaxCards = 52;
        mTotalCardTypes = Enum.GetValues(typeof(CardType)).Length;
        mMaxCardsPerSet = mMaxCards / mTotalCardTypes;
        mPlayersPlayedThereTurns = 0;
        mCurrentGamePlayMode = GamePlayMode.TwoPlayer;

        mListOfCards = new List<Card>();
        mListOfPlayers.Clear();
        mListPoolOfCards.Clear();
        GenerateCards();
    }
    void GenerateCards()
    {
        CardType cType = CardType.spade;
        int idCounter = 0;
        for (int i = 0; i < mTotalCardTypes; i++)
        {
            switch (i)
            {
                case 0:
                    cType = CardType.spade;
                    break;
                case 1:
                    cType = CardType.hearts;
                    break;
                case 2:
                    cType = CardType.clove;
                    break;
                case 3:
                    cType = CardType.diamonds;
                    break;
            }

            for (int j = 1; j <= mMaxCardsPerSet; j++)
            {
                Card aCard = new Card(idCounter,cType, j);
                mListOfCards.Add(aCard);
                idCounter++;
            }
        }

        var rand = new System.Random();
        mListOfCards = mListOfCards.OrderBy(x => rand.Next()).ToList();
    }

    public void LoadGame(int mode)
    {
        DivideCards(mode);
        OnStartGame();
        SetPlayersTurn(PlayersId.p1);
    }

    private void DivideCards(int divideInTo)
    {
        mListOfPlayers = new List<Player>();
        int counter = 0;
        for (int i = 0; i < divideInTo; i++)
        {
            Player aPlayer = new Player();
            aPlayer.mPlayersId = (PlayersId)i;
            int cardsPerPerson = (mListOfCards.Count / divideInTo);
            for (int j = 0; j < cardsPerPerson; j++)
            {
                switch (i)
                {
                    case 0:
                        aPlayer.mListOfCard.Add(mListOfCards[counter]);
                        break;
                    case 1:
                        aPlayer.mListOfCard.Add(mListOfCards[counter]);
                        break;
                    case 2:
                        aPlayer.mListOfCard.Add(mListOfCards[counter]);
                        break;
                    case 3:
                        aPlayer.mListOfCard.Add(mListOfCards[counter]);
                        break;
                }
                counter++;
            }

            mListOfPlayers.Add(aPlayer);
        }
    }
    #endregion

    #region GamePlayOperations

    private void SetPlayersTurn(PlayersId pt)
    {
        mPlayersTurn = pt;
        string popupMessage = string.Format("Turn of : " + Manager_UI.Instance.GetPlayer(mPlayersTurn));
        ScreenGenericPopup.Instance.ShowPopup(PopupType.info, popupMessage,1.0f);
        OnPlayersTurn(pt);
        //Debug.Log("SetPlayersTurn : Players Turn : " + mCurrentPlayer.mPlayerID);
    }

    public void SwitchPlayersTurn(PlayersId pId)
    {
        mPlayersPlayedThereTurns++;
        if (mPlayersPlayedThereTurns >= mListOfPlayers.Count)
        {
            CheckForRoundWinner();
        }
        else
        {
            int nextPlayerID =(int) pId;
            nextPlayerID++;
            if (nextPlayerID >= mListOfPlayers.Count)
                nextPlayerID = 0;

            SetPlayersTurn((PlayersId)nextPlayerID);
            Debug.Log("SwitchPlayersTurn : It Is Now : " + mPlayersTurn + " : Players Turn");
        }
    }

    private void CheckForRoundWinner()
    {
        int greaterCard = mListOfCurrentCardsShown.Max(c => c.mValue);
        List<Card> mLstCards = mListOfCurrentCardsShown.FindAll(c => c.mValue == greaterCard).ToList();

        mNumberOfCardsToDraw = 1;

        if (mLstCards.Count < 2)
        {
            Debug.Log("CheckForWinner : Winner Found !!!!!");
            DeclareRoundWinner(mLstCards[0].mCardOfPlayer);
        }
        else
        {
            // This is were the WAR is declared

            mNumberOfCardsToDraw = mLstCards[0].mValue;

            if (!DeclareMatchWinner())
            {
                string popupMessage = string.Format("War Mode Started\nEach Player Draw \n " + mNumberOfCardsToDraw.ToString() + " Card");
                if (mNumberOfCardsToDraw > 1)
                    popupMessage = popupMessage + "s";

                mAudioSource.clip = mWarMode;
                mAudioSource.Play();
                ScreenGenericPopup.Instance.ShowPopup(PopupType.ok, popupMessage, 1.0f, EnterWarMode);
                Debug.Log("CheckForWinner : mNumberOfCardsToDraw : " + mNumberOfCardsToDraw);
            }
        }
    }

    private void DeclareRoundWinner(PlayersId pId)
    {
        Debug.Log("DeclareWinner : pId : " + pId);
        OnRoundWinnerDecleared(pId);
        Player aPlayer = mListOfPlayers.Find(p => p.mPlayersId == pId);
        aPlayer.mListOfCard.AddRange(mListPoolOfCards);
        mListOfCurrentCardsShown.Clear();
        mListPoolOfCards.Clear();
        if (!DeclareMatchWinner())
        {
            StartCoroutine("LoadNextRound", pId);
        }
    }

    private IEnumerator LoadNextRound(PlayersId pId)
    {
        yield return new WaitForSeconds(1f);
        mPlayersPlayedThereTurns = 0;
        SetPlayersTurn(pId);
    }

    private bool DeclareMatchWinner()
    {
        bool isMatchOver = false; ;
        Player aPlayer = mListOfPlayers.Find(p => p.mListOfCard.Count >= mMaxCards);
        if (aPlayer != null)
        {
            isMatchOver = true;
        }
        else
        {
            foreach (Player player in mListOfPlayers)
            {
                if (player.mListOfCard.Count < mNumberOfCardsToDraw)
                {
                    aPlayer = mListOfPlayers.Find(p => p.mPlayersId != player.mPlayersId);
                    isMatchOver = true;
                }
            }
        }

        if (isMatchOver)
        {
            OnMatchWinnerDecleared(aPlayer);
            return isMatchOver;
        }

        return isMatchOver;
    }
    #endregion

    #region Callbacks
    private void EnterWarMode()
    {
        mAudioSource.clip = mWarStart;
        mAudioSource.Play();
        mListOfCurrentCardsShown.Clear();
        mPlayersPlayedThereTurns = 0;
        OnWarModeActivated(mNumberOfCardsToDraw);
        SetPlayersTurn(PlayersId.p1);
    }

    private void EndMatch(Player player)
    {
        SetDefaults();
    }
    #endregion
}
