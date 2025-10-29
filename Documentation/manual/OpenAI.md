# OpenAI API Bindings
## Setup
In order to use the API bindings, you need to add your own API key to your project.
Create a folder called **Resources** in your Assets folder if it doesn't already exist.
Then, create a file called **openAI.txt** somewhere else and paste your API key in it.
Finally, import the file to your Resources folder.
Never commit this file to source control!
If you are using git, it is recommended to add this file to your .gitignore.

## Usage
Currently, only text messages via the responses API are supported.
The first message in a conversation always needs to be a regular TextRequest object.
On creation, you need to supply a text message, and later you can optionally add meta instructions or a different model (default is gpt4o-mini).
Then hand the request to the RequestHandler and start the Upload enumerator as coroutine.
On finish, you can either quickly access the answer text through the answer field or access the entire answer object through the fullAnswer field.

The next request can either be a TextRequest again, in which case it is stateless and nothing from your previous request is remembered, or a StatefullTextRequest.
For a StatefullTextRequest you need to supply the response ID from the previous request, which you find in the fullAnswer field.
