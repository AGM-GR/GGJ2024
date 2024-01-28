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
    public Image GoldenImg;
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
        Refresh(0);
    }


    public void Init(ReactiveCollection<TeethType> collection)
    {
        collection.ObserveCountChanged().Subscribe(Refresh);
        collection.ObserveAdd().Subscribe(CheckGolden);
        collection.ObserveRemove().Subscribe(CheckGolden);
        _canvasGroup.alpha = 1;
    }

    private void CheckGolden(CollectionRemoveEvent<TeethType> obj)
    {
        if (obj.Value == TeethType.Gold)
        {
            GoldenImg.enabled = false;
        }
    }

    private void CheckGolden(CollectionAddEvent<TeethType> obj)
    {
        if(obj.Value == TeethType.Gold)
        {
            GoldenImg.enabled = true;
        }
    }

    private void Refresh(int amount)
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
