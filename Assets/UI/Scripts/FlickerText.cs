using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class FlickerText : MonoBehaviour
{
    [SerializeField] private UIDocument _UiDocument;
    [SerializeField, Range(0.5f, 3f)] private float _flickerInterval = 1f;

    private List<VisualElement> _textElements = new List<VisualElement>();
    private string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

    private void Start()
    {
        if (_UiDocument == null)
        {
            Debug.LogError("Please assign a UIDocument to the FlickerText script.");
            return;
        }

        VisualElement root = _UiDocument.rootVisualElement;
        _textElements.AddRange(root.Query<Label>().ToList());
        _textElements.AddRange(root.Query<Button>().ToList());


        //Look for element numbers 
        if (_textElements.Count > 0)
        {
            StartCoroutine(FlickerRotine());
        }
        else
        {
            Debug.LogError("No text elements found in the UIDocument.");
        }
    }

    private IEnumerator FlickerRotine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(0.5f, _flickerInterval));
            if (_textElements.Count == 0) continue;

            //Gets a Random Element
            VisualElement randomElement = _textElements[Random.Range(0, _textElements.Count)];
            string _originalText = GetText(randomElement);

            //If the element has no text, skip it
            if (string.IsNullOrEmpty(_originalText)) continue;
            
            int randomIndex = Random.Range(0, _originalText.Length);
            char randomChar = _characters[Random.Range(0, _characters.Length)];

            //replace a random character with a random character
            char[] newText = _originalText.ToCharArray();
            newText[randomIndex] = randomChar;
            string flickerText = new string(newText);

            //apply the flickering effect
            SetText(randomElement, flickerText);
            yield return new WaitForSecondsRealtime(0.1f);
            SetText(randomElement, _originalText);

        }


    }

    private string GetText(VisualElement element)
    {
        if (element is Label label) return label.text;
        if (element is Button button) return button.text;
        return "";
    }

    private void SetText(VisualElement element, string text)
    {
        if (element is Label label) label.text = text;
        if (element is Button button) button.text = text;
    }
}
