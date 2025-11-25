using UnityEngine;
using TMPro;

public class FlappyBirdUI : MonoBehaviour
{
    [SerializeField] TMP_Text hintText;
    [SerializeField] KeyDebuff keyDebuff;
    [SerializeField] GlobalGameManager globalGameManager;

    void OnEnable()
    {

        if (keyDebuff != null)
        {
            keyDebuff.OnKeyChanged += UpdateUI;
        }


        if (globalGameManager != null)
        {
            int currentIndex = globalGameManager.currentJumpActionIndex;
            int currentValue = GlobalDatas.ConvertInputIndexeToValue(currentIndex);
            UpdateUI(currentValue);
        }
    }

    void OnDisable()
    {

        if (keyDebuff != null)
        {
            keyDebuff.OnKeyChanged -= UpdateUI;
        }
    }

  
    public void UpdateUI(int keyValue)
    {
        string keyName = KeyName(keyValue);

 
        if (hintText != null)
        {
            hintText.text = $"Press [{keyName}] to Jump";
        }
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