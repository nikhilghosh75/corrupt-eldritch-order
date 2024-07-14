using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassManager : MonoBehaviour
{
    [SerializeField] AnimationCharacterController playerAnimation;

    void Start()
    {
        PlayerClassBase baseClass = gameObject.GetComponent<PlayerClassBase>();
        PlayerClassNinja ninja = gameObject.GetComponent<PlayerClassNinja>();
        PlayerClassWizard wizard = gameObject.GetComponent<PlayerClassWizard>();
        PlayerClassTank tank = gameObject.GetComponent<PlayerClassTank>();

        string className = RunManager.Instance.selectedClassName;

        switch (className)
        {
            case "Default":
                baseClass.enabled = true;
                playerAnimation.ActivateCharacterGraphic(CharacterType.Default);
                break;
            case "Ninja":
                ninja.enabled = true;
                playerAnimation.ActivateCharacterGraphic(CharacterType.Ninja);
                break;
            case "Wizard":
                wizard.enabled = true;
                playerAnimation.ActivateCharacterGraphic(CharacterType.Wizard);
                break;
            case "Tank":
                tank.enabled = true;
                playerAnimation.ActivateCharacterGraphic(CharacterType.Tank);
                break;
        }
    }
}
