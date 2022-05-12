using i5.Toolkit.Core.RocketChatClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientBootstrapper : MonoBehaviour
{

    public GameObject DemoCanvas;
    public string HostAddress = "";
    public string Username = "";
    public string Password = "";
    
    private RocketChatClient client;
    private TMP_InputField hostAddress;
    private TMP_InputField username;
    private TMP_InputField password;
    private Button login;
    private Button getChannelList;
    private TMP_InputField roomID;
    private TMP_InputField messageToPost;
    private Button postMessage;
    private Button subscribe;
    private Button unsubscribe;

    //Can be freely chosen.
    private string subscribtionUniqueID = "1";

    void Awake()
    {
        hostAddress = DemoCanvas.transform.Find("HostAddress").GetComponent<TMP_InputField>();
        username = DemoCanvas.transform.Find("Username").GetComponent<TMP_InputField>();
        password = DemoCanvas.transform.Find("Password").GetComponent<TMP_InputField>();
        login = DemoCanvas.transform.Find("Login").GetComponent<Button>();
        getChannelList = DemoCanvas.transform.Find("GetChannelListJoined").GetComponent<Button>();
        roomID = DemoCanvas.transform.Find("RoomID").GetComponent<TMP_InputField>();
        messageToPost = DemoCanvas.transform.Find("MessageToPost").GetComponent<TMP_InputField>();
        postMessage = DemoCanvas.transform.Find("PostMessage").GetComponent<Button>();
        subscribe = DemoCanvas.transform.Find("Subscribe").GetComponent<Button>();
        unsubscribe = DemoCanvas.transform.Find("Unsubscribe").GetComponent<Button>();
    }

    private void Start()
    {
        login.onClick.AddListener(LoginAsync);
        getChannelList.onClick.AddListener(GetChannelListJoinedAsync);
        postMessage.onClick.AddListener(PostMessageAsync);
        subscribe.onClick.AddListener(SubscribeAsync);
        unsubscribe.onClick.AddListener(UnsubscribeAsync);
        if(HostAddress != "")
        {
            hostAddress.text = HostAddress;
        }
        if(Username != "")
        {
            username.text = Username;
        }
        if(Password != "")
        {
            password.text = Password;
        }
    }

    private void OnDestroy()
    {
        if (ServiceManager.ServiceExists<RocketChatClient>())
        {
            ServiceManager.RemoveService<RocketChatClient>();
        }
    }

    public async void LoginAsync()
    {
        InstantiateClient();
        WebResponse<string> response = await client.LoginAsync();
        Debug.Log("Response Code: " + response.Code);
    }

    public async void GetChannelListJoinedAsync()
    {
        WebResponse<string> response = await client.GetChannelListJoinedAsync();
        Debug.Log("Response Code: " + response.Code);
        Debug.Log(response.Content);
    }

    public async void PostMessageAsync()
    {
        WebResponse<string> response = await client.PostMessageAsync(roomID.text, messageToPost.text);
        Debug.Log("Response Code: " + response.Code);
    }

    public async void SubscribeAsync()
    {
        await client.SubscribeRoomMessageAsync(roomID.text, subscribtionUniqueID);
    }

    public async void UnsubscribeAsync()
    {
        await client.UnsubscribeRoomMessageAsync(subscribtionUniqueID);
    }

    private void InstantiateClient()
    {
        client = new RocketChatClient(hostAddress.text, username.text, password.text);
        client.OnMessageReceived += message => Debug.Log("Message Received");
        ServiceManager.RegisterService(client);
    }

}
