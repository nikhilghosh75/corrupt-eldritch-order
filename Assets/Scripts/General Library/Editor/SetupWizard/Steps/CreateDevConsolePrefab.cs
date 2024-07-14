#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WSoft.Tools.Console;
using WSoft.UI;

/*
 * Written by Nikhil Ghosh '24
 */

public class CreateDevConsolePrefab : SetupWizardStep
{
    public CreateDevConsolePrefab()
    {
        name = "Create Dev Console Prefab";
    }

    public override void Do()
    {
        GameObject prefabBase = new GameObject("Developer Console");

        DeveloperConsoleBehaviour console = SetupConsoleBehaviour(prefabBase);

        GameObject canvasObject = new GameObject("Canvas");
        canvasObject.transform.SetParent(prefabBase.transform);
        Canvas canvas = SetupCanvas(canvasObject);

        GameObject backgroundObject = new GameObject("Background");
        backgroundObject.transform.SetParent(canvasObject.transform);
        SetupBackground(backgroundObject);

        GameObject inputFieldObject = new GameObject("InputField (TMP)");
        inputFieldObject.transform.SetParent(backgroundObject.transform);
        TMP_InputField inputField = SetupInputField(inputFieldObject);

        GameObject textAreaObject = CreateRectMaskObject(backgroundObject);
        GameObject textObject = new GameObject("Text (TMP)");
        textObject.transform.SetParent(textAreaObject.transform);
        TMP_Text text = SetupText(textObject);
        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(1, 0.9f);

        console.uiCanvas = canvasObject;
        console.inputField = inputField;
        console.autocompleteText = text;

        PrefabUtility.SaveAsPrefabAsset(prefabBase, "Assets/Prefabs/Developer Console.prefab");
    }

    public override bool HasBeenDone()
    {
        return AssetDatabase.FindAssets("Developer Console", new string[] { "Assets/Prefabs" }).Length >= 1;
    }

    DeveloperConsoleBehaviour SetupConsoleBehaviour(GameObject gameObject)
    {
        DeveloperConsoleBehaviour console = gameObject.AddComponent<DeveloperConsoleBehaviour>();
        console.openKey = Key.Backquote;
        console.closeKey = Key.Backquote;
        console.autocompleteKey = Key.Tab;

        return console;
    }

    Canvas SetupCanvas(GameObject gameObject)
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.sortingOrder = 16000;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.targetDisplay = 1;

        CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
        canvasScaler.scaleFactor = 1;
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        GraphicRaycaster graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

        return canvas;
    }

    void SetupBackground(GameObject gameObject)
    {
        RectTransform backgroundRect = gameObject.AddComponent<RectTransform>();
        // backgroundRect.anchorMin = new Vector2(0, 0);
        // backgroundRect.anchorMax = new Vector2(1, 1);
        backgroundRect.SetFillParent();
        Image backgroundImage = gameObject.AddComponent<Image>();
        backgroundImage.sprite = null;
        backgroundImage.color = new Color(0, 0, 0, 0.5f);
    }

    TMP_InputField SetupInputField(GameObject gameObject)
    {
        Image backgroundImage = gameObject.AddComponent<Image>();
        backgroundImage.sprite = null;
        backgroundImage.color = new Color(0, 0, 0, 1f);

        RectTransform inputRect = gameObject.GetComponent<RectTransform>();
        inputRect.SetFillParent();
        inputRect.anchorMin = new Vector2(0, 0.9f);
        inputRect.anchorMax = new Vector2(1, 1);

        TMP_InputField inputField = gameObject.AddComponent<TMP_InputField>();
        inputField.fontAsset = TMP_FontAsset.CreateFontAsset(Resources.GetBuiltinResource<Font>("Arial.ttf"));

        GameObject rectMaskObject = CreateRectMaskObject(gameObject);

        GameObject textObject = new GameObject("Text (TMP)");
        textObject.transform.SetParent(rectMaskObject.transform);
        TMP_Text text = SetupText(textObject);

        inputField.textComponent = text;
        inputField.pointSize = 48;

        return inputField;
    }

    GameObject CreateRectMaskObject(GameObject parentObject)
    {
        GameObject rectMaskObject = new GameObject("Text Area");
        rectMaskObject.transform.SetParent(parentObject.transform);
        rectMaskObject.AddComponent<RectTransform>().SetFillParent();
        rectMaskObject.AddComponent<RectMask2D>();

        return rectMaskObject;
    }

    TMP_Text SetupText(GameObject gameObject)
    {
        TMP_Text text = gameObject.AddComponent<TextMeshProUGUI>();
        Font builtInFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.font = TMP_FontAsset.CreateFontAsset(builtInFont);

        gameObject.GetComponent<RectTransform>().SetFillParent();

        return text;
    }
}
#endif