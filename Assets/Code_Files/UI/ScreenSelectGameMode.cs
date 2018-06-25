using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSelectGameMode : Base_UI
{
    #region UnityDefaults
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
    #endregion

    #region ButtonClickEvents
    public void OnBtnSelectGameModeClicked(int mode)
    {
        Manager_UI.Instance.ScreenShow(ScreenType.GamePlay);
        Manager_GamePlay.Instance.LoadGame(mode);
    }
    #endregion
}