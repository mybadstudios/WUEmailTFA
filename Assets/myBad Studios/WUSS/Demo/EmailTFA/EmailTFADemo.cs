using UnityEngine;
using MBS;
using UnityEngine.SceneManagement;

public class EmailTFADemo : MonoBehaviour
{
    void Start() => WULogin.OnLoggedIn += GoToValidationScene;    
    void GoToValidationScene(CML _) => WUEmailTFA.GenerateTFAEntry(onsuccess, onfail);    
    void onsuccess(CML _) => SceneManager.LoadScene("EmailTFAValidationScene");
    void onfail(CMLData data) => Debug.LogError($"[ERROR]: {data.String("message")}");
}