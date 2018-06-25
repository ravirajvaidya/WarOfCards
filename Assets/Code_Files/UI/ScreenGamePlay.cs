using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScreenGamePlay : Base_UI
{
    public Text[] mTotalNoOfCardsInPool;
    public List<PlayerControler> mListOfPlayerControler;
    public AudioClip mRoundWin;
    public AudioClip mMatchWin;
    public AudioSource mAudioSource;

    #region UnityDefaults
    // Use this for initialization
    public void OnEnable()
    {
        Manager_GamePlay.Instance.OnStartGame += StartGame;
        Manager_GamePlay.Instance.OnPlayersTurn += SetPlayersTurn;
        Manager_GamePlay.Instance.OnRoundWinnerDecleared += RoundWinnerDeclared;
        Manager_GamePlay.Instance.OnMatchWinnerDecleared += MatchWinnerDeclared;
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        UpdatePoolCount();
    }

    public void OnDisable()
    {
        Manager_GamePlay.Instance.OnStartGame -= StartGame;
        Manager_GamePlay.Instance.OnPlayersTurn -= SetPlayersTurn;
        Manager_GamePlay.Instance.OnRoundWinnerDecleared -= RoundWinnerDeclared;
        Manager_GamePlay.Instance.OnMatchWinnerDecleared -= MatchWinnerDeclared;
    }
    #endregion

    #region EventsCallbacks
    void StartGame()
    {
        int counter = 0;
        foreach (Player player in Manager_GamePlay.Instance.mListOfPlayers)
        {
            mListOfPlayerControler[counter].mThisPlayer = player;
            counter++;
        }
    }

    void SetPlayersTurn(PlayersId pId)
    {
        PlayerControler playerControler = mListOfPlayerControler.Find(p => p.mThisPlayer.mPlayersId == pId);
        playerControler.mBtnDrawCard.image.color = Color.green;
    }

    void RoundWinnerDeclared(PlayersId pId)
    {
        mAudioSource.clip = mRoundWin;
        mAudioSource.Play();
        string popupMessage = string.Format("Winner of the Round is : " + Manager_UI.Instance.GetPlayer(pId));
        ScreenGenericPopup.Instance.ShowPopup(PopupType.info, popupMessage,2.0f);
        StartCoroutine("ResetGame");
    }

    void MatchWinnerDeclared(Player player)
    {
        mAudioSource.clip = mMatchWin;
        mAudioSource.Play();
        string popupMessage = string.Format(Manager_UI.Instance.GetPlayer(player.mPlayersId) + "\nHAS WON THE GAME !!!");
        ScreenGenericPopup.Instance.ShowPopup(PopupType.ok, popupMessage, 1.0f, EndMatch);
    }
    #endregion

    #region User Defined
    IEnumerator ResetGame()
    {
        Debug.Log("ResetGame 1");
        yield return new WaitForSeconds(1.0f);
        int counter = 0;
        foreach (Player player in Manager_GamePlay.Instance.mListOfPlayers)
        {
            mListOfPlayerControler[counter].SetPlayerDefaults();
            counter++;
        }
    }

    public void UpdatePoolCount()
    {
        mTotalNoOfCardsInPool[0].text = Manager_GamePlay.Instance.mListPoolOfCards.Count.ToString();
        mTotalNoOfCardsInPool[1].text = Manager_GamePlay.Instance.mListPoolOfCards.Count.ToString();
    }

    private void EndMatch()
    {
        Manager_UI.Instance.ScreenShow(ScreenType.MainMenu);
    }
    #endregion
}
