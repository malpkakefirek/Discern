using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.Rendering;

public class F1Menu : MonoBehaviour
{
    private GameObject[] rooms;
    private bool isMenuOpen = false;
    private int subMenu = 0;
    private int index = 0;
    [SerializeField] TextMeshProUGUI menuText;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameController;
    private CharacterController playerController;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        rooms = GameObject.FindGameObjectsWithTag("AnomalyRoom");
        playerController = player.GetComponent<CharacterController>();
        InvokeRepeating("RefreshFunction", 0f, 1f); //refresh every 1 sec
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isMenuOpen)
            {
                if (subMenu == 0)
                {
                    closeMenu();
                }
            }
            else
            {
                index = 0;
                refreshTextMain();
                panel.SetActive(true);
                isMenuOpen = true;
            }
        }

        if ( isMenuOpen)
        {
            if (subMenu == 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && index != 0)
                {
                    index -= 1;
                    refreshTextMain();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && index != 2)
                {
                    index += 1;
                    refreshTextMain();
                }
                if (Input.GetKeyDown(KeyCode.Return)) // spawn Anomaly
                {
                    subMenu = index + 1;
                    index = 0;
                    refreshText();
                }
            }
            else if (subMenu == 1)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && index != 0)
                {
                    index -= 1;
                    refreshText();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && index != rooms.Length-1)
                {
                    index += 1;
                    refreshText();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    GameObject room = rooms[index];
                    if (room != null)
                    {
                        if (!room.GetComponent<AnomalyRoom>().activeAnomaly)
                        {
                            room.GetComponent<AnomalyRoom>().spawnRandomAnomaly(player.transform.position);
                        }
                        else
                        {
                            Debug.Log("Anomaly in this room already exists");
                        }      
                        closeMenu();
                    }
                }
                
            }
            else if (subMenu == 2)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && index != 0)
                {
                    index -= 1;
                    refreshText();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && index != rooms.Length - 1)
                {
                    index += 1;
                    refreshText();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    GameObject room = rooms[index];
                    if (room != null)
                    {
                        gameController.GetComponent<GameController>().attempAnomalyFix(room, room.GetComponent<AnomalyRoom>().activeAnomalyName);
                        closeMenu();
                    }
                }
            }else if (subMenu == 3)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && index != 0)
                {
                    index -= 1;
                    refreshText();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && index != rooms.Length - 1)
                {
                    index += 1;
                    refreshText();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    GameObject room = rooms[index];
                    if (room != null)
                    {
                        playerController.enabled = false; // Temporarily disable the CharacterController to teleport
                        player.transform.position = new Vector3(room.GetComponent<AnomalyRoom>().spawnPointX, room.GetComponent<AnomalyRoom>().spawnPointY, room.GetComponent<AnomalyRoom>().spawnPointZ);
                        playerController.enabled = true;
                        closeMenu();
                    }
                }
            }
        }
    }

    private void refreshText()
    {
        menuText.text = "";
        for (int i = 0; i < rooms.Length; i++)
        {
            if (i == index)
            {
                menuText.text += "<color=yellow>" + rooms[i].name + "</color>\n";
            }
            else
            {
                menuText.text += rooms[i].name + "\n";
            }
        }
    }
    private void refreshTextMain()
    {
        menuText.text = "";
        if(index == 0)
        {
            menuText.text += "<color=yellow>Spawn Anomaly</color>\n";
        }
        else
        {
            menuText.text += "Spawn Anomaly\n";
        }
        if (index == 1)
        {
            menuText.text += "<color=yellow>Fix Anomaly</color>\n";
        }
        else
        {
            menuText.text += "Fix Anomaly\n";
        }
        if (index == 2)
        {
            menuText.text += "<color=yellow>Teleport</color>";
        }
        else
        {
            menuText.text += "Teleport";
        }
        
    }
    void RefreshFunction()
    {
        if (isMenuOpen)
        {

        }
    }

    private void closeMenu()
    {
        panel.SetActive(false);
        menuText.text = "<color=yellow>Spawn Anomaly<color>\n";
        menuText.text += "Fix Anomaly\nTeleport";
        subMenu = 0;
        index = 0;
        isMenuOpen = false;
    }
}
