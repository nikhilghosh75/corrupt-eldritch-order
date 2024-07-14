using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class RebindControlStartEvent
{
    public string actionName;

    public RebindControlStartEvent(string _actionName)
    {
        actionName = _actionName;
    }
}

public class RebindControlEndEvent
{
    public InputActionReference action;
    public string actionName;

    public RebindControlEndEvent(InputActionReference _action, string _actionName)
    {
        action = _action;
        actionName = _actionName;
    }
}

public class RemappableControlButton : MonoBehaviour
{
    public InputActionAsset allActions;
    public InputActionReference action;
    public TMP_Text text;
    public List<Image> images;
    public string actionName;
    public PlayerInput playerInput;

    InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    // Start is called before the first frame update
    void Start()
    {
        if (playerInput == null)
        {
            playerInput = PlayerInput.GetPlayerByIndex(0);
        }

        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            text.enabled = false;
            foreach (Image image in images)
            {
                if (image.sprite != null)
                {
                    image.enabled = true;
                }
            }
        }
        else
        {
            text.enabled = true;
            foreach (Image image in images)
                image.enabled = false;
        }
    }

    string NormalizeBindingPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return path;

        // Ensure the path starts with '<' and contains '>/'
        if (!path.StartsWith("<"))
        {
            path = path.Remove(0, 1);
            path = "<" + path;
        }
            
        if (!path.Contains(">/"))
            path = path.Replace("/", ">/");

        return path;
    }


    bool IsDuplicateBinding(InputAction action, string newBinding)
    {
        string bindingGroup = playerInput.currentControlScheme == "Gamepad" ? "Gamepad" : "Keyboard";

        newBinding = NormalizeBindingPath(newBinding); // Normalize the new binding path

        foreach (var binding in allActions.bindings)
        {
            if (binding.groups.Contains(bindingGroup))
            {
                string existingBindingPath = NormalizeBindingPath(binding.effectivePath);
                if (existingBindingPath == newBinding && !binding.isComposite)
                {
                    return true;
                }
            }
        }
        return false;
    }


    InputActionRebindingExtensions.RebindingOperation CreateRebindingOperation()
    {
        if (rebindingOperation != null)
            rebindingOperation.Dispose();

        rebindingOperation = action.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse/*") // Excluding mouse controls, adjust as needed
            .OnMatchWaitForAnother(0.05f)
            .OnPotentialMatch(operation =>
            {
                // Capture the potential new binding path
                string newBindingPath = operation.selectedControl.path;
                if (IsDuplicateBinding(action.action, newBindingPath))
                {
                    // Duplicate found, cancel the operation
                    Debug.LogError("Duplicate binding detected. Operation cancelled.");
                    operation.Cancel(); // This cancels the current operation
                }
            })
            .OnComplete(operation =>
            {
                Debug.Log("rebinded");
                OnRemappingComplete();
            })
            .OnCancel(operation =>
            {
                Debug.Log("cancelled");
                EventBus.Publish(new RebindControlEndEvent(action, actionName));
            });

        return rebindingOperation;
    }

    public void StartRemapping()
    {
        rebindingOperation = CreateRebindingOperation();
        rebindingOperation = rebindingOperation.Start();

        //text.text = "";
        EventBus.Publish(new RebindControlStartEvent(actionName));
    }


    void OnRemappingComplete()
    {
        rebindingOperation.Dispose();
        UpdateDisplay();
        EventBus.Publish(new RebindControlEndEvent(action, actionName));
    }

    public void UpdateDisplay()
    {
        string bindingGroup = playerInput.currentControlScheme == "Gamepad" ? "Gamepad" : "Keyboard";
        string str = "";
        List<string> paths = new List<string>();

        for(int i = 0; i < action.action.bindings.Count; i++)
        {
            InputBinding binding = action.action.bindings[i];
            if (binding.groups.Contains(bindingGroup))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += " / ";
                }

                string path = InputControlPath.ToHumanReadableString(binding.effectivePath,
                        InputControlPath.HumanReadableStringOptions.OmitDevice);
                str += path;
                paths.Add(path);
            }
        }

        text.text = str;
        for(int i = 0; i < paths.Count; i++)
        {
            if (ControllerUIDictionary.Instance.controllerUIDictionary.ContainsKey(str))
            {
                images[i].sprite = ControllerUIDictionary.Instance.controllerUIDictionary[str];
            }
        }

        for (int i = paths.Count; i < images.Count; i++)
        {
            images[i].sprite = null;
        }
    }
}
