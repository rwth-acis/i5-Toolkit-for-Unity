using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.OpenAI;

public class LLMController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendRequests());   
    }

    IEnumerator SendRequests()
    {
        TextRequest firstRequest = new TextRequest("Please introduce yourself to the user. Also remember the number three.");
        yield return RequestHandler.Upload(firstRequest);
        Debug.Log(firstRequest.answer);
        StatefullTextRequest secondRequest = new StatefullTextRequest("Which number should you remember?", firstRequest.fullAnswer.id);
        yield return RequestHandler.Upload(secondRequest);
        Debug.Log(secondRequest.answer);
    }
}
