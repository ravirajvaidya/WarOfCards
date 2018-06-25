using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum PopupType
{
    none,
    info,
    ok
}

public class ScreenGenericPopup : MonoBehaviour
{
    private static ScreenGenericPopup instance;

    public static ScreenGenericPopup Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject mPopupWindow;
    public Text mPopupMessage;
    public Button mBtnOk;

    public Action OnOkClicked = null;

    #region UnityDefaults
    // Use this for initialization
    void Awake()
    {
        instance = this;
        HidePopup();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {

    }
    #endregion

    #region PopupControles
    public void ShowPopup(PopupType ptype, string message, float time=1.0f, Action callBack = null)
    {
        mPopupWindow.SetActive(true);
        mPopupMessage.text = message;
        mBtnOk.gameObject.SetActive(false);

        if (callBack != null)
            OnOkClicked = callBack;

        StopCoroutine("HidePopupIn");

        switch (ptype)
        {
            case PopupType.info:
                StartCoroutine("HidePopupIn",time);
                break;
            case PopupType.ok:
                mBtnOk.gameObject.SetActive(true);
                break;
        }
    }

    void HidePopup()
    {
        mPopupWindow.SetActive(false);
        mPopupMessage.text = string.Empty;
    }

    IEnumerator HidePopupIn(float time)
    {
        yield return new WaitForSeconds(time);
        HidePopup();
    }
    #endregion

    #region ButtonCallbacks
    public void OnBtnOkClicked()
    {
        HidePopup();
        if(OnOkClicked != null)
        OnOkClicked();
    }
    #endregion
}
