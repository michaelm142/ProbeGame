using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlots : MonoBehaviour
{
    public MainMenuState state;

    public bool DeletingSaveSlot { get; set; }

    private void OnEnable()
    {
        var slot1 = transform.Find("slot1");
        var slot2 = transform.Find("slot2");
        var slot3 = transform.Find("slot3");

        var mainMenu = FindObjectOfType<MainMenu>();
        if (mainMenu != null)
            state = mainMenu.state;

        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
        var slot1file = dir.GetFiles().ToList().Find(f => f.Extension == ".slot1");
        if (slot1file != null)
        {
            slot1.transform.Find("Name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(slot1file.Name);
            slot1.GetComponent<Button>().interactable = true;
            DateTime created = slot1file.CreationTime;
            slot1.transform.Find("DateCreated").GetComponent<Text>().text = string.Format("Date Created: {0}/{1}/{2}", created.Day, created.Month, created.Year);
        }
        else
        {
            if (state == MainMenuState.LoadGame)
                slot1.GetComponent<Button>().interactable = false;
            else
                slot1.GetComponent<Button>().interactable = true;
        }

        var slot2file = dir.GetFiles().ToList().Find(f => f.Extension == ".slot2");
        if (slot2file != null)
        {
            slot2.transform.Find("Name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(slot2file.Name);
            DateTime created = slot2file.CreationTime;
            slot2.transform.Find("DateCreated").GetComponent<Text>().text = string.Format("Date Created: {0}/{1}/{2}", created.Day, created.Month, created.Year);
        }
        else
        {
            if (state == MainMenuState.LoadGame)
                slot2.GetComponent<Button>().interactable = false;

            else
                slot2.GetComponent<Button>().interactable = true;
        }

        var slot3file = dir.GetFiles().ToList().Find(f => f.Extension == ".slot3");
        if (slot3file != null)
        {
            slot3.transform.Find("Name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(slot3file.Name);
            DateTime created = slot3file.CreationTime;
            slot3.transform.Find("DateCreated").GetComponent<Text>().text = string.Format("Date Created: {0}/{1}/{2}", created.Day, created.Month, created.Year);
        }
        else
        {
            if (state == MainMenuState.LoadGame)
                slot3.GetComponent<Button>().interactable = false;
            else
                slot3.GetComponent<Button>().interactable = true;
        }
    }

    public void UpdateDeleteSaveSlot(int index)
    {
        if (!DeletingSaveSlot)
            return;
        DeletingSaveSlot = false;

        var dir = new DirectoryInfo(Environment.CurrentDirectory);
        var fileToDelete = dir.GetFiles().ToList().Find(f => f.Extension == string.Format(".slot{0}", index));
        fileToDelete.Delete();

        var slot = transform.Find("slot" + index.ToString());
        slot.Find("Name").GetComponent<Text>().text = "Username";
        slot.Find("DateCreated").GetComponent<Text>().text = "Date Created: 00/00/0000";
        slot.GetComponent<Button>().interactable = false;
    }
}
