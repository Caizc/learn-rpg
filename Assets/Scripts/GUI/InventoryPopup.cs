using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    // [SerializeField]
    // private AudioClip sound;

    [SerializeField]
    private Image[] itemIcons;
    [SerializeField]
    private Text[] itemLabels;

    [SerializeField]
    private Text currentItemLabel;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private Button useButton;

    private string _currentItem;

    public void Refresh()
    {
        List<string> itemList = Managers.Inventory.GetItemList();

        int length = itemIcons.Length;

        for (int i = 0; i < length; i++)
        {
            if (i < itemList.Count)
            {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);

                string item = itemList[i];

                Sprite sprite = Resources.Load<Sprite>("Icons/" + item);

                itemIcons[i].sprite = sprite;
                itemIcons[i].SetNativeSize();

                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;

                if (item == Managers.Inventory.equippedItem)
                {
                    message = "Equipped\n" + message;
                }

                itemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) => { OnItem(item); });

                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear();
                trigger.triggers.Add(entry);
            }
            else
            {
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }

        if (!itemList.Contains(_currentItem))
        {
            _currentItem = null;
        }

        if (null == _currentItem)
        {
            currentItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            currentItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);

            if ("health" == _currentItem)
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }

            currentItemLabel.text = _currentItem + ":";
        }
    }

    public void OnItem(string item)
    {
        _currentItem = item;
        Refresh();
    }

    public void OnEquip()
    {
        Managers.Inventory.EquipItem(_currentItem);
        Refresh();
    }

    public void OnUse()
    {
        Managers.Inventory.ConsumeItem(_currentItem);
        Refresh();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    // public void OnSoundToggle()
    // {
    //     Managers.Audio.soundMute = !Managers.Audio.soundMute;
    //     Managers.Audio.PlaySound(sound);
    // }

    // public void OnSoundValue(float volume)
    // {
    //     Managers.Audio.soundVolume = volume;
    // }

    // public void OnPlayMusic(int selector)
    // {
    //     switch (selector)
    //     {
    //         case 1:
    //             Managers.Audio.PlayLevelMusic();
    //             break;
    //         case 2:
    //             Managers.Audio.PlayIntroMusic();
    //             break;
    //         default:
    //             Managers.Audio.StopMusic();
    //             break;
    //     }
    // }

    // public void OnMusicToggle()
    // {
    //     Managers.Audio.musicMute = !Managers.Audio.musicMute;
    //     Managers.Audio.PlaySound(sound);
    // }

    // public void OnMusicValue(float volume)
    // {
    //     Managers.Audio.musicVolume = volume;
    // }
}
