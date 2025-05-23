using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationSetter : MonoBehaviour
{
    private bool active = false;

    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(ID);
    }

    public void ChangeLocale(int localeId)
    {
        if (active) return;
        StartCoroutine(SetLocale(localeId));
    }

    IEnumerator SetLocale(int _localeId)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeId];
        PlayerPrefs.SetInt("LocaleKey", _localeId);
        active = false;
    }
}
