using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenType
{
    none,
    MainMenu,
    SelectGameMode,
    GamePlay,
    GameOver
}

public class Base_UI : MonoBehaviour
{
    public ScreenType mScreenType;

    #region UnityDefaults
    // Use this for initialization
    public virtual void Start () {
		
	}

    // Update is called once per frame
    public virtual void Update () {
		
	}
    #endregion

    #region ScreenOpretions
    public void ScreenShow()
    {
        this.gameObject.SetActive(true);
    }

    public void ScreenHide()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}
