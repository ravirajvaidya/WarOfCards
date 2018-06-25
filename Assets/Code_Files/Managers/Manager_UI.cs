using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_UI : MonoBehaviour
{
    private static Manager_UI instance;
    public static Manager_UI Instance
    {
        get { return instance; }
    }

    public List<Base_UI> mListUIBase;
    public Base_UI mCurrentScreen;

    public Sprite[] mCardFaces = new Sprite[4];

    #region UnityDefaults
    private void Awake()
    {
        instance = this;
        HideAllScreens();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    #endregion

    #region ScreenOpretions
    void HideAllScreens()
    {
        foreach (Base_UI ui in mListUIBase)
        {
            if (ui.mScreenType != ScreenType.MainMenu)
                ui.ScreenHide();
            else
                mCurrentScreen = ui;
        }
    }

    public void ScreenShow(ScreenType screenType)
    {
        if(mListUIBase.Exists(st => st.mScreenType == screenType))
        {
            mCurrentScreen.ScreenHide();
            Base_UI screen = mListUIBase.Find(st => st.mScreenType == screenType);
            screen.ScreenShow();
            mCurrentScreen = screen;
        }
    }

    public string GetPlayer(PlayersId pId)
    {
        string player = string.Empty;
        switch (pId)
        {
            case PlayersId.p1:
                player = "PLAYER 1";
                break;
            case PlayersId.p2:
                player = "PLAYER 2";
                break;
            case PlayersId.p3:
                player = "PLAYER 3";
                break;
            case PlayersId.p4:
                player = "PLAYER 4";
                break;
        }
        return player;
    }
    #endregion
}
