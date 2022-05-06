using i5.Toolkit.Core.RocketChatClient;
using UnityEngine;

public class ClientBootstrapper : MonoBehaviour
{
    public string hostAddress = "acis-chat.dbis.rwth-aachen.de";
    [Tooltip("The username, e.g. firstname.lastname")]
    public string username = "Your Username";
    public string password = "Your Password";

    private RocketChatClient client;

    // Start is called before the first frame update
    void Start()
    {
        client = new RocketChatClient(hostAddress, username, password);
        StartCoroutine(client.Login());
        //StartCoroutine(client.PostMessage("Your Room ID"));
        //StartCoroutine(client.Me());
        //client.SubscribeRoomMessage("Your Room ID", "1");
    }
}
