using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;

public class Myi18nTranslateTool : SerializedMonoBehaviour
{
	[SerializeField]Text text;
	[SerializeField]LeanLocalizedText i18nTextTranslatorScript;
    // Start is called before the first frame update
    void Start()
	{
		if(!text || !i18nTextTranslatorScript){
			text = GetComponent<Text>();
			i18nTextTranslatorScript = GetComponent<LeanLocalizedText>();
		}
    }

	public string TranslateText(string keyword){
		i18nTextTranslatorScript.TranslationName = keyword;
		while(true){
			if(text.text != "Waiting..."){
				break;
			}
		}	
		return text.text;
	}
}
