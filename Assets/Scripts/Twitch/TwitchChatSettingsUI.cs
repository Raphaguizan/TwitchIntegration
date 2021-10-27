using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Twitch.Chat
{
    public class TwitchChatSettingsUI : MonoBehaviour
    {
        public TMP_InputField PasswordInput;
        public TMP_InputField UsernameInput;
        public TMP_InputField ChannelNameInput;
        public Toggle saveToggle;
        public TwitchChat TwitchChat;

        private readonly string passwordKey = "Password";
        private readonly string channelNameKey = "ChannelName";
        private readonly string userNameKey = "UserName";


        void Start()
        {
            // you can save the other info too in player prefs or however you save things
            if (PlayerPrefs.HasKey(passwordKey) && PlayerPrefs.HasKey(channelNameKey) && PlayerPrefs.HasKey(userNameKey))
            {

                ChannelNameInput.text = PlayerPrefs.GetString(channelNameKey);
                UsernameInput.text = PlayerPrefs.GetString(userNameKey);
                PasswordInput.text = PlayerPrefs.GetString(passwordKey);

                Debug.Log($"<color=blue> auto connected </color>");
                StartCoroutine(Initiallize());
            }
        }

        IEnumerator Initiallize()
        {
            yield return new WaitUntil(() => TwitchChat.Instance != null);
            Connect();
        }

        public void Connect()
        {

            TwitchCredentials credentials = new TwitchCredentials
            {
                ChannelName = ChannelNameInput.text.ToLower(),
                Username = UsernameInput.text.ToLower(),
                Password = PasswordInput.text
            };
            if (saveToggle.isOn)
            {
                PlayerPrefs.SetString(userNameKey, UsernameInput.text.ToLower());
                PlayerPrefs.SetString(channelNameKey, ChannelNameInput.text.ToLower());
                PlayerPrefs.SetString(passwordKey, PasswordInput.text);
            }
            TwitchChat.Connect(credentials);
        }

    }
}
