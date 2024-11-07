using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMP_InputField usernameInputRegister; 
    public TMP_InputField passwordInputRegister;
    public Button registerButton;
    public TMP_Text RegisterFeedbackText;

    public TMP_InputField usernameInputLogIn;
    public TMP_InputField passwordInputLogIn;
    public Button logInButton;
    public TMP_Text LogInFeedbackText;

    public GameObject menu;
    public TMP_Text usernameDisplayText;
    public TMP_Text highScoreDisplayText;

    private DBManager databaseManager;

    private void Start()
    {
        databaseManager = FindFirstObjectByType<DBManager>();

        // Listeners botones
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        logInButton.onClick.AddListener(OnLoginButtonClick);
    }

    public void OnRegisterButtonClick()
    {
        string username = usernameInputRegister.text;
        string password = passwordInputRegister.text;

        string resultMessage = databaseManager.RegisterUser(username, password);
        RegisterFeedbackText.text = resultMessage;

        if (resultMessage == "Usuario registrado con éxito")
        {
            usernameInputRegister.text = "";
            passwordInputRegister.text = "";
        }
    }

    public void OnLoginButtonClick()
    {
        string username = usernameInputLogIn.text;
        string password = passwordInputLogIn.text;

        string resultMessage = databaseManager.LoginUser(username, password);
        LogInFeedbackText.text = resultMessage;

        if (resultMessage == "Inicio de sesión exitoso")
        {
            usernameInputLogIn.text = "";
            passwordInputLogIn.text = "";
            //menu.SetActive(false);

            SceneManager.LoadScene("GameScene");

            //usernameDisplayText.text = "Usuario: " + username;
            //int highScore = databaseManager.GetHighScore(username);
            //highScoreDisplayText.text = "Puntuación Máxima: " + highScore;
        }
    }
}
