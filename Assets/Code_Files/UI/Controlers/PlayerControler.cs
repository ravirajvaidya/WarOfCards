using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    public Image mCardShowFace;
    public Text mCardNo;
    public Text mCardTotalCount;
    public Text mButtonText;
    public Button mBtnDrawCard;
    public Player mThisPlayer;
    public AudioSource mButtonClicked;

    private int mCardsToDraw = 1;

    private int mCounter;
    #region UnityDefaults
    // Use this for initialization
    void OnEnable()
    {
        Manager_GamePlay.Instance.OnWarModeActivated += WarModeActivated;
        SetPlayerDefaults();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mCardTotalCount.text = mThisPlayer.mListOfCard.Count.ToString();
    }
    void OnDisable()
    {
        Manager_GamePlay.Instance.OnWarModeActivated -= WarModeActivated;
        SetPlayerDefaults();
    }
    #endregion

    #region SetDefaults
    public void SetPlayerDefaults()
    {
        mCardsToDraw = 1;
        mBtnDrawCard.image.color = Color.white;
        mCardShowFace.sprite = null;
        mCardNo.text = string.Empty;
        mCardTotalCount.text = mThisPlayer.mListOfCard.Count.ToString();
    }
    #endregion

    #region EventsCallbacks
    void WarModeActivated(int cardsToDraw)
    {
        this.mCardsToDraw = cardsToDraw;
    }
    #endregion

    #region ButtonClickEvents
    public void OnBtnClicked()
    {
        if (mThisPlayer.mPlayersId == Manager_GamePlay.Instance.mPlayersTurn)
        {
            if (mThisPlayer.mListOfCard.Count >= mCardsToDraw)
            {
                Card aCard = mThisPlayer.mListOfCard[0];
                aCard.mCardOfPlayer = mThisPlayer.mPlayersId;
                Manager_GamePlay.Instance.mListPoolOfCards.Add(aCard);
                mThisPlayer.mListOfCard.RemoveAt(0);
                mCounter++;
                mCardsToDraw--;

                if (mCounter == Manager_GamePlay.Instance.mNumberOfCardsToDraw - 1)
                    mButtonText.text = "SHOW CARD";
                else
                    mButtonText.text = "DRAW CARD";

                if (mCounter == Manager_GamePlay.Instance.mNumberOfCardsToDraw)
                {
                    mCardShowFace.sprite = Manager_UI.Instance.mCardFaces[(int)aCard.mCardType];
                    mCardNo.text = aCard.mValue.ToString();
                    Manager_GamePlay.Instance.mListOfCurrentCardsShown.Add(aCard);
                    Manager_GamePlay.Instance.SwitchPlayersTurn(mThisPlayer.mPlayersId);
                    mBtnDrawCard.image.color = Color.white;
                    mCounter = 0;
                }
                mButtonClicked.Play();
            }
            else
                Debug.Log("Opponent player : WINS");
        }
        else
        {
            Debug.Log("It is player : " + Manager_GamePlay.Instance.mPlayersTurn + " Turn");
            string popupMessage = string.Format("It is : " + Manager_UI.Instance.GetPlayer(Manager_GamePlay.Instance.mPlayersTurn)+" Turn");
            ScreenGenericPopup.Instance.ShowPopup(PopupType.info, popupMessage, 1.0f);
        }
    }
    #endregion
}