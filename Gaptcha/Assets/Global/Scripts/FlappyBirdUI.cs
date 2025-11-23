using UnityEngine;
using TMPro;

public class FlappyBirdUI : MonoBehaviour
{
    [SerializeField] TMP_Text hintText;
    [SerializeField] KeyDebuff keyDebuff;

    void OnEnable()
    {
        keyDebuff.OnKeyChanged += UpdateUI;

        int mapped = keyDebuff.IsActive ? keyDebuff.MappedKey : InputValue.SPACE;
        UpdateUI(mapped);
    }

    void OnDisable()
    {
        keyDebuff.OnKeyChanged -= UpdateUI;
    }

    public void UpdateUI(int mappedKey)
    {
        string keyName = KeyName(mappedKey);
        hintText.text = $"Press [{keyName}] to Jump";
    }

    string KeyName(int v)
    {
        if (v == InputValue.LEFT) return "LEFT";
        if (v == InputValue.RIGHT) return "RIGHT";
        if (v == InputValue.UP) return "UP";
        if (v == InputValue.DOWN) return "DOWN";
        return "SPACE";
    }
}
