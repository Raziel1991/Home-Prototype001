using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HoverScrambler : MonoBehaviour
{
    [SerializeField] private UIDocument _UiDocument;
    [SerializeField, Range(0.05f, 1f)] private float _scrambleDuration = 0.5f;
    [SerializeField, Range(0.05f, 1f)] private float _scrambleInterval = 0.1f;
    private string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:',.<>?/`~";
    private Dictionary<Button, string> _buttonTexts = new Dictionary<Button, string>();
    private Dictionary<Button, Coroutine> _buttonCoroutines = new Dictionary<Button, Coroutine>();

    private void Start()
    {
        if (_UiDocument == null)
        {
            Debug.LogError("Please assign a UIDocument to the HoverScrambler script.");
            return;
        }

        var root = _UiDocument.rootVisualElement;
        var buttons = root.Query<Button>().ToList();
        foreach (var button in buttons)
        {
            _buttonTexts[button] = button.text;
            button.RegisterCallback<PointerEnterEvent>(evt => StartScramble(button));
            button.RegisterCallback<PointerLeaveEvent>(evt => StopScramble(button));
        }
    }

    private void StartScramble(Button button)
    {
        if (_buttonCoroutines.ContainsKey(button))
        {
            return;
        }

        Coroutine scrambleRoutine = StartCoroutine(ScrambleText(button));
        _buttonCoroutines[button] = scrambleRoutine;
    }

    private void StopScramble(Button button)
    {
        if (_buttonCoroutines.TryGetValue(button, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            _buttonCoroutines.Remove(button);
            button.text = _buttonTexts[button];
        }
    }

    private IEnumerator ScrambleText(Button button)
    {
        string originalText = _buttonTexts[button]; 
        float elapsedTime = 0f;
        while (elapsedTime < _scrambleDuration)
        {
            char[] scrambled = originalText.ToCharArray();
            for (int i = 0; i < scrambled.Length; i++)
            {
                if (Random.value < 0.5f)
                {
                    scrambled[i] = _characters[Random.Range(0, _characters.Length)];
                }
            }
            button.text = new string(scrambled);
            yield return new WaitForSecondsRealtime(_scrambleInterval); // Real-time wait
            elapsedTime += _scrambleInterval; 
        }
        StopScramble(button); // Clean up when duration ends
    }
}