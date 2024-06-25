using UnityEngine;
using MBS;
using TMPro;

public class EmailTFAValidation : MonoBehaviour
{
    public TMP_InputField CodeInputField;
    public TextMeshProUGUI StatusMessage;

    public void SubmitCode() => WUEmailTFA.ValidateKey(CodeInputField.text, OnSuccess, OnFail);    
    public void OnSuccess(CML _) => StatusMessage.text = "Code validated! You can now load your game's main scene";
    public void OnFail(CMLData data) => StatusMessage.text = $"[ERROR]: {data.String("message")}";
}