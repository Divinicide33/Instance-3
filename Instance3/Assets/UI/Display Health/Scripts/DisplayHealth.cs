using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : UIElement<DisplayHealth>
{
    [SerializeField] private Stats playerStats;
    public static Action<Stats> onUpdateHpMax { get; set; }
    [SerializeField] private Transform healthPanel;
    [SerializeField] private GameObject hpImage;
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
            Debug.Log($"images.Count = {images.Count}");
            return;
        } 

        if (images.Count < playerStats.healthMax)
        {
            int nb = playerStats.healthMax - images.Count;
            Debug.Log($"if (images.Count < playerStats.healthMax) : nb = {nb}");

            for (int i = 0; i < nb; i++)
            {
                GameObject newHpGo = Instantiate(hpImage, healthPanel);
                images.Add(newHpGo.GetComponent<Image>());
            }
        }
        else
        {
            int nb =  images.Count - playerStats.healthMax;
            Debug.Log($"else : nb = {nb}");

            for (int i = 0; i < nb; i++)
            {
                Debug.Log($"Destroy(images[images.Count - 1]); = {images[images.Count - 1]}");
                Destroy(images[images.Count - 1]);
                //images.Remove(images[images.Count]);

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

        if (playerStats == null) 
            return;

        if (playerStats.healthMax < playerStats.health)
            playerStats.health = playerStats.healthMax;

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
