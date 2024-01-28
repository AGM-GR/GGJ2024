using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;

public class PlayerAreaWidget : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public CharacterData characterData;
    [Space]
    public Image PortraitImg;
    public List<Image> BgImgs;
    //public TextMeshProUGUI NameText;
    public List<GameObject> TeethGOs;

    public void Start()
    {
        PortraitImg.sprite = characterData.Portrait;
        BgImgs.ForEach(i => i.color = characterData.Color);
        //NameText.text = characterData.Name.ToString();

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }


    public void Init(ReactiveProperty<int> reactiveAmount)
    {
        reactiveAmount.Subscribe(RefreshAmount);
        _canvasGroup.alpha = 1;
    }

    private void RefreshAmount(int amount)
    {
        TeethGOs.ForEach(t => t.SetActive(false));

        for (int i = 0; i < TeethGOs.Count; i++)
        {
            if (i + 1 <= amount)
            {
                TeethGOs[i].SetActive(true);
            }
        }
    }
}
