using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMainMenu : Base_UI
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
    public void OnBtnPlayClicked()
    {
        Manager_UI.Instance.ScreenShow(ScreenType.SelectGameMode);
    }
    #endregion
}
