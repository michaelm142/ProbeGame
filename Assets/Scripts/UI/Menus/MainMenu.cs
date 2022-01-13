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

    public MainMenuState state { get; set; }

    public SaveSlots SaveSlots;
    public GameObject NameEntryScreen;

    public Text version;

    // Start is called before the first frame update
    void Start()
    {
        version.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ContinueGame()
    {
        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
        var saveFiles = dir.GetFiles().ToList().FindAll(f => f.Extension.IndexOf(".slot") != -1);
        saveFiles.Sort(delegate (FileInfo a, FileInfo b)
        {
            if (b.CreationTime.Ticks > a.CreationTime.Ticks)
                return -1;

            return 0;
        });

        FindObjectOfType<LoadSave>().LoadProgress(saveFiles[0].FullName);
    }

    public void BeginNewGame()
    {
        state = MainMenuState.NewGame;
    }

    public void BeginLoadGame()
    {
        state = MainMenuState.LoadGame;
    }

    public void Cancel()
    {
        state = MainMenuState.Default;
        NameEntryScreen.transform.Find("InputField").GetComponent<InputField>().text = string.Empty;
    }

    public void SelectSaveSlot(int index)
    {
        // create save file
        if (state == MainMenuState.NewGame)
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
            var existingFile = dir.GetFiles().ToList().Find(f => f.Extension == string.Format(".slot{0}", index));
            if (existingFile != null && existingFile.Exists)
                existingFile.Delete();
            var newFile = new FileInfo(string.Format("{0}\\{1}{2}", Environment.CurrentDirectory,
                NameEntryScreen.transform.Find("InputField").GetComponent<InputField>().text, string.Format(".slot{0}", index)));
            newFile.Create();

            var inventoryObj = new GameObject("PlayerInventory");
            var inventory = inventoryObj.AddComponent<PlayerInventory>();
            inventory.activeSaveFile = newFile;

            SceneManager.LoadScene("StoreScreen");
        }
        else
            FindObjectOfType<LoadSave>().LoadProgress(index);
    }

    public void SetUserName(InputField inputField)
    {
        userName = inputField.text;
        SaveSlots.state = state;
        SaveSlots.gameObject.SetActive(true);
    }

    public void SetResolution(Dropdown d)
    {
        var resolutions = Screen.resolutions;

        Screen.SetResolution(resolutions[d.value].width, resolutions[d.value].height, FindObjectOfType<WindowModeToggle>().GetComponent<Toggle>().isOn);
    }


    public void Quit()
    {
        Application.Quit();
    }

}
public enum MainMenuState
{
    Default,
    LoadGame,
    NewGame
}
