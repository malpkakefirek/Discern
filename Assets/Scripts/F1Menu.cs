using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class F1Menu : MonoBehaviour
{
    private GameObject[] rooms;
    private bool isMenuOpen = false;
    private int subMenu = 0;
    [SerializeField] TextMeshProUGUI menuText;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameController;
    private CharacterController playerController;
    private Dictionary<KeyCode, GameObject> roomKeyBindings = new Dictionary<KeyCode, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        rooms = GameObject.FindGameObjectsWithTag("AnomalyRoom");
        for (int i = 0; i < rooms.Length; i++)
        {
            KeyCode roomKey = GetObjectKeyCode(i);
            roomKeyBindings.Add(roomKey, rooms[i]);
        }
        playerController = player.GetComponent<CharacterController>();
        InvokeRepeating("RefreshFunction", 0f, 1f); //refresh every 1 sec
    }
    KeyCode GetObjectKeyCode(int roomIndex)
    {
        if (roomIndex < 9)
        {
            return KeyCode.Alpha1 + roomIndex;
        }
        else
        {
            return KeyCode.F1 + (roomIndex - 9);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isMenuOpen)
            {
                closeMenu();
            }
            else
            {
                panel.SetActive(true);
                isMenuOpen = true;
            }
        }

        if ( isMenuOpen)
        {
            if (subMenu == 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) // spawn Anomaly
                {
                    menuText.text = "";
                    foreach (var room in roomKeyBindings)
                    {
                        menuText.text += room.Key + " " + room.Value.name + "\n";
                    }
                    /*int count = 1;
                    foreach (GameObject room in rooms)
                    {
                        menuText.text += count + " - " + room.name+"\n";
                        count++;
                    }*/
                    subMenu = 1;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2)) // fix Anomaly
                {
                    menuText.text = "";
                    foreach (var room in roomKeyBindings)
                    {
                        menuText.text += room.Key + " " + room.Value.name + "\n";
                    }
                    /*int count = 1;
                    foreach (GameObject room in rooms)
                    {
                        menuText.text += count + " - " + room.name + "\n";
                        count++;
                    }*/
                    subMenu = 2;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3)) // teleport Player
                {
                    //menuText.text = "1 - Spawn\n";
                    menuText.text = "";
                    foreach (var room in roomKeyBindings)
                    {
                        menuText.text += room.Key + " " + room.Value.name + "\n";
                    }
                    /*int count = 2;
                    foreach (GameObject room in rooms)
                    {
                        menuText.text += count + " - " + room.name + "\n";
                        count++;
                    }*/
                    subMenu = 3;
                }
            }
            else if (subMenu == 1)
            {

                foreach (var kvp in roomKeyBindings)
                {
                    if (Input.GetKeyDown(kvp.Key))
                    {
                        GameObject room = kvp.Value;
                        if (room != null)
                        {
                            if (!room.GetComponent<AnomalyRoom>().activeAnomaly)
                            {
                                room.GetComponent<AnomalyRoom>().spawnRandomAnomaly();
                            }
                            else
                            {
                                Debug.Log("Anomaly in this room already exists");
                            }

                            closeMenu();
                            break;
                        }
                    }
                }
                /*for (int i = 1; i <= 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        if (i - 1 < rooms.Length)
                        {
                            GameObject room = rooms[i - 1];
                            if (room != null)
                            {
                                if (!room.GetComponent<AnomalyRoom>().activeAnomaly)
                                {
                                    room.GetComponent<AnomalyRoom>().spawnRandomAnomaly();
                                }
                                else
                                {
                                    Debug.Log("Anomaly in this room already exists");
                                }
                                    
                                closeMenu();
                            }
                        }
                        break;
                    }
                }*/
            }
            else if (subMenu == 2)
            {
                foreach (var kvp in roomKeyBindings)
                {
                    if (Input.GetKeyDown(kvp.Key))
                    {
                        GameObject room = kvp.Value;
                        if (room != null)
                        {
                            //room.GetComponent<AnomalyRoom>().getActiveAnomaly().fixAnomaly();
                            gameController.GetComponent<GameController>().attempAnomalyFix(room, room.GetComponent<AnomalyRoom>().activeAnomalyName);
                            closeMenu();
                            break;
                        }
                    }
                }
                /*for (int i = 1; i <= 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        if (i-1 < rooms.Length)
                        {
                            GameObject room = rooms[i-1];
                            if (room != null)
                            {
                                //room.GetComponent<AnomalyRoom>().getActiveAnomaly().fixAnomaly();
                                gameController.GetComponent<GameController>().attempAnomalyFix(room, room.GetComponent<AnomalyRoom>().activeAnomalyName);
                                closeMenu();
                            }
                        }
                        break;
                    }
                }*/
            }else if (subMenu == 3)
            {

                foreach (var kvp in roomKeyBindings)
                {
                    if (Input.GetKeyDown(kvp.Key))
                    {
                        GameObject room = kvp.Value;
                        if (room != null)
                        {
                            playerController.enabled = false; // Temporarily disable the CharacterController to teleport
                            player.transform.position = new Vector3(room.GetComponent<AnomalyRoom>().spawnPointX, room.GetComponent<AnomalyRoom>().spawnPointY, room.GetComponent<AnomalyRoom>().spawnPointZ);
                            playerController.enabled = true;
                            closeMenu();
                            break;
                        }
                    }
                }
                /*if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    playerController.enabled = false; // Temporarily disable the CharacterController to teleport
                    player.transform.position = new Vector3(1, 2, 1); // to specify
                    playerController.enabled = true;
                    closeMenu();
                }
                else
                {
                    for (int i = 2; i <= 9; i++)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                        {
                            if (i - 2 < rooms.Length)
                            {
                                GameObject room = rooms[i - 2];
                                if (room != null)
                                {
                                    playerController.enabled = false; // Temporarily disable the CharacterController to teleport
                                    player.transform.position = new Vector3(room.GetComponent<AnomalyRoom>().spawnPointX, room.GetComponent<AnomalyRoom>().spawnPointY, room.GetComponent<AnomalyRoom>().spawnPointZ);
                                    playerController.enabled = true;
                                    closeMenu();
                                }
                            }
                            break;
                        }
                    }
                }*/
            }
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
        menuText.text = "1 - Spawn Anomaly\n2 - Fix Anomaly\n3 - Teleport";
        subMenu = 0;
        isMenuOpen = false;
    }
}
