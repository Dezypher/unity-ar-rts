using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject[] mainmenuButtons;
    public GameObject title;

    public void StartHost()
    {
        //start hosting
        title.GetComponent<UnityEngine.UI.Text>().text = "IP address";
        this.gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = "Cancel";
        foreach (GameObject button in mainmenuButtons)
        {
            button.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
