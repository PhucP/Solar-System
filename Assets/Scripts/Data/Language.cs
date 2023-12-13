using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Language : MonoBehaviour
{
    [SerializeField] private Localize localization;
    
    private string _title;
    private string _term;
    
    private void OnEnable()
    {
        _title = GetComponent<TMP_Text>().gameObject.name;
        _term = $"txt_{_title}";
#if UNITY_EDITOR
        if (!LocalizationManager.Sources[0].ContainsTerm(_term))
        {
            LocalizationManager.Sources[0].AddTerm(_term, eTermType.Text);
            var termData = LocalizationManager.Sources[0].GetTermData(_term);
            termData.SetTranslation(0, _title);
        }
#endif
        if (localization != null)
        {
            localization.SetTerm(_term);
        }
    }
}
