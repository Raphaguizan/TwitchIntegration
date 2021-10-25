using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TwitchChatSettingsUI : MonoBehaviour
{
    public TMP_InputField PasswordInput;
    public TMP_InputField UsernameInput;
    public TMP_InputField ChannelNameInput;
    public Toggle saveToggle;
    public TwitchChat TwitchChat;


    void Start(){
        // you can save the other info too in player prefs or however you save things
        if(PlayerPrefs.HasKey("Password") && PlayerPrefs.HasKey("ChannelName") && PlayerPrefs.HasKey("UserName"))
        {
            TwitchCredentials credentials = new TwitchCredentials
            {
                ChannelName = PlayerPrefs.GetString("ChannelName"),
                Username = PlayerPrefs.GetString("UserName"),
                Password = PlayerPrefs.GetString("Password")
            };
            TwitchChat.Connect(credentials, new CommandCollection());
        }
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Connect(){
       
        TwitchCredentials credentials = new TwitchCredentials{
            ChannelName = ChannelNameInput.text.ToLower(),
            Username = UsernameInput.text.ToLower(),
            Password = PasswordInput.text
        };
        if (saveToggle.isOn)
        {
            PlayerPrefs.SetString("UserName", UsernameInput.text.ToLower());
            PlayerPrefs.SetString("ChannelName", UsernameInput.text.ToLower());
            PlayerPrefs.SetString("Password", PasswordInput.text);
        }
        TwitchChat.Connect(credentials, new CommandCollection());
    }

}
