using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string userName;

    private State state;

    public GameObject SaveSlotScreen;
    public GameObject NameEntryScreen;

    private FileInfo activeSaveFile;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BeginGameplay()
    {
        SceneManager.LoadScene(1);
    }

    public void BeginNewGame()
    {
        state = State.NewGame;
    }

    public void BeginLoadGame()
    {
        state = State.LoadGame;
    }

    public void Cancel()
    {
        state = State.Default;
        NameEntryScreen.transform.Find("InputField").GetComponent<InputField>().text = string.Empty;
    }

    public void SelectSaveSlot(int index)
    {
        // create save file
        if (state == State.NewGame)
        {
            activeSaveFile = new FileInfo(Environment.CurrentDirectory + "//" + userName + string.Format(".slot{0}", index));
            if (!activeSaveFile.Exists)
                activeSaveFile.Create();
        }

        BeginGameplay();
    }

    public void SetUserName(UnityEngine.UI.InputField inputField)
    {
        userName = inputField.text;
        SaveSlotScreen.SetActive(true);
        UpdateSaveSlots();
    }

    public void SetResolution(Dropdown d)
    {
        var resolutions = Screen.resolutions;

        Screen.SetResolution(resolutions[d.value].width, resolutions[d.value].height, FindObjectOfType<WindowModeToggle>().GetComponent<Toggle>().isOn);
    }

    public void UpdateSaveSlots()
    {
        var slot1 = SaveSlotScreen.transform.Find("slot1");
        var slot2 = SaveSlotScreen.transform.Find("slot2");
        var slot3 = SaveSlotScreen.transform.Find("slot3");

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
            if (state == State.LoadGame)
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
            if (state == State.LoadGame)
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
            if (state == State.LoadGame)
                slot3.GetComponent<Button>().interactable = false;
            else
                slot3.GetComponent<Button>().interactable = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    private enum State
    {
        Default,
        LoadGame,
        NewGame
    }
}
