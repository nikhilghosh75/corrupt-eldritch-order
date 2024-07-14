using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public List<Transform> spawnPoints;


    InputActionAsset playerInputActionAsset;
    private PlayerInputManager playerInputManager;
    private List<GameObject> playerList;
    private ReadOnlyArray<InputDevice> inputDevices;
    private ReadOnlyArray<InputControlScheme> validControlSchemes;

    // This logic can be moved to another script in the future, presumably
    // in the player setup / customization screen.
    void Start()
    {
        // Grab the InputActionAsset from the player prefab
        playerInputActionAsset = playerPrefab.GetComponent<PlayerInput>().actions;

        playerList = new List<GameObject>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.playerPrefab = playerPrefab;

        // Grab all active input devices then check for ones that match our valid
        // control schemes.
        inputDevices = InputSystem.devices;
        validControlSchemes = playerInputActionAsset.controlSchemes;
        int validInputDevices = 0;

        bool keyboardFound = false, mouseFound = false;

        foreach (InputDevice device in inputDevices)
        {
            foreach (InputControlScheme scheme in validControlSchemes)
            {
                // Logic to detect both keyboard and mouse
                if (device is Keyboard)
                    keyboardFound = true;
                else if (device is Mouse)
                    mouseFound = true;

                if (scheme.SupportsDevice(device))
                {
                    validInputDevices++;
                    continue; // At least one control scheme supports this device
                }
            }
        }

        if (keyboardFound && mouseFound)
            validInputDevices--;

        if (validInputDevices > 0) 
        {
            SpawnPlayer();
        }

        if (validInputDevices > 1)
        {
            SpawnPlayer();
        }
    }

    // When players are manually spawned, the PlayerInputManager is notified
    // and automatically keeps track of their PlayerInput components
    void SpawnPlayer()
    {
        if (playerList.Count < spawnPoints.Count)
        {
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = spawnPoints[playerList.Count].position;
            playerList.Add(player);
        }
        else
        {
            Debug.LogWarning("Not enough spawn points for another Player");
        }
    }
}
