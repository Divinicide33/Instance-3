using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : UIElement
{
    [SerializeField] private Stats playerStats;
    public static Action<Stats> onUpdateHpMax { get; set; }
    [SerializeField] private Transform healthPanel;
    [SerializeField] GameObject hpImage;
    [SerializeField] private Sprite hpFilled;
    [SerializeField] private Sprite hpHollow;
    [SerializeField] private List<Image> images;
    
    protected override void OnEnable() 
    {
        base.OnEnable();
        onUpdateHpMax += ChangeHpMax;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        onUpdateHpMax -= ChangeHpMax;
    }

    private void ChangeHpMax(Stats stats)
    {
        playerStats = stats;
        
        if (images.Count == playerStats.healthMax)
        {
            UpdateDisplay();
            return;
        } 

        if (images.Count < playerStats.healthMax)
        {
            int nb = playerStats.healthMax - images.Count;

            for (int i = 0; i < nb; i++)
            {
                GameObject newHpGo = Instantiate(hpImage, healthPanel);
                images.Add(newHpGo.GetComponent<Image>());
            }
        }
        else
        {
            int nb =  images.Count - playerStats.healthMax;

            for (int i = 0; i < nb; i++)
            {
                images.Remove(images[images.Count]);
            }
        }

        UpdateDisplay();
    }

    protected override void Show()
    {
        healthPanel.gameObject.SetActive(true);
    }

    protected override void Hide()
    {
        healthPanel.gameObject.SetActive(false);
    }

    protected override void UpdateDisplay()
    {
        UpdateImage();
    }

    private void UpdateImage()
    {
        int index = 0;

        if (playerStats.healthMax < playerStats.health) playerStats.health = playerStats.healthMax;

        foreach (Image image in images)
        {
            if (index < playerStats.healthMax && index < playerStats.health) 
            {
                image.enabled = true;
                image.sprite = hpFilled;
            }
            else if (index < playerStats.healthMax)
            {
                image.enabled = true;
                image.sprite = hpHollow;
            }
            else 
            {
                image.enabled = false;
                Debug.Log("More Image than hp max");
            }

            index++;
        }
    }
}
