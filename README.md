### WUEmailTFA
A simple extension to add TFA to your project. Not perfect but definitely simple to implement

### High level overview: How this works
When the WordPress for Unity Bridge logs in, we send a message to the website saying "Create a random code". The person who just logged in then receives this code via their registered email address.
Back in Unity we simply go to a scene where we prompt the user to enter the code and we then say "Validate the code". The function takes 2 actions as parameters: one to execute on failure and one to execute if the code is correct.

So basically, in step 1 you call WUEmailTFA.GenerateTFAEntry() and then go to a new scene.
In the new scene you wait for the user to enter something into an InputField and then call ValidateKey() to determine if you should load the new scene or show an error message. 

That's all there is to it.

### About the functions

#WUEmailTFA.GenerateTFAEntry(string gamename, Action<CML> OnSuccess, Action<CMLData> OnFail)

This function takes 3 parameters in this first release. The first parameter being the name of the game as you want it to be displayed in the email the user receives. I forgot I have this value stored on the website and can simply get that value automatically so in a future update this field will not be required any longer.

The other two fields are optional but, as the names suggest, one is executed when the code was successfully generated while the other gets called if something went wrong. It is safest to make use of them but you can safely skip them if you want (not recommended, though)

#WUEmailTFA.ValidateKey(string key, Action<CML> OnSuccess, Action<CMLData> OnFail)

This takes as the first parameter the value the user typed into the input field. 
The second and third parameters trigger when the code was validated or failed to validate, respectively. You CAN ignore these two delegates but it doesn't make much sense to do so. When you ask the server to validate the code it will respond by triggering one delegate or the other so it makes sense to have both implemented.

### Installing this extension into WordPress For Unity Bridge
1. Since this is an EXPANSION to the WUBridge asset it stands to reason that it needs the WUBridge to be installed first since it relies on code rom that asset in order to function. To that end, in Unity, install the WUBridge asset FIRST, then install this asset into the same project. 

If you install this one first you will receive a LOT of errors but they will all go away as soon as you install the WUBridge so no harm done if you do this the wrong way around

2. In WordPress, you need to install the wub_emailtfa plugin which can be found inside your Unity project under Assets\myBad Studios\WUSS\Wordpress\Plugins. This only needs to be installed to your website once, obviously, and not "per project" so if you have already installed it you are free to skip this step. 

Having said that, this time around it IS VERY IMPORTANT that you install the wub_login plugin that you received with the WUBridge asset FIRST! Again, this plugin relies on code that exists in the wub_login plugin and if it is not found WordPress itself will break and require you to FTP into your website to rename the plugin folder before you can fix it. 

Personally I think this is a major flaw in WordPress ("If a plugin is broken you don't have access to your site to fix it, now go fix it somehow" is a stupid idea) but it is what it is so just avoid this issue by simply installing wub_login first (as was described in the instructions you received wit the WUBridge asset also)

That is it. That is all it takes. You are ready to go.

### Implementing the kit in game
The demo provided shows how to integrate the extension into your new or existing game.

It boils down to this:

1. in the scene where you place the login prefab, add this in start:
<strong>WULogin.OnLoggedIn += GoToValidationScene;</strong>
This will tell your project to wait until the user has logged in and then call the function "GoToValidationScene" (or whatever function you want to call instead)

2. Inside the function GoToValidationScene I call:
<strong>WUEmailTFA.GenerateTFAEntry("My Awesome Game", onsuccess, onfail);</strong>
This tells the game only to continue if the code was successfully generated on the server by calling the onsuccess delegate or, if something went wrong, it will call the onfail delegate.

Now all that remains is to define those two delegates. I do that like this:
    void onsuccess(CML _) => SceneManager.LoadScene("EmailTFAValidationScene");
    void onfail(CMLData data) => Debug.LogError($"[ERROR]: {data.String("message")}");

And that is all for the first phase of integration. Now we move to the "EmailTFAValidationScene scene and inside there I have these three lines of code:
<strong>public void SubmitCode() => WUEmailTFA.ValidateKey(CodeInputField.text, OnSuccess, OnFail);<br>
public void OnSuccess(CML _) => StatusMessage.text = "Code validated! You can now load your game's main scene";<br>
public void OnFail(CMLData data) => StatusMessage.text = $"[ERROR]: {data.String("message")}";</strong>

The first function is called when the user clicks a button on screen and passes the code the player provided to the server. It then calls either OnSuccess or OnFail based on wether the code was valid for this player for this game at the current time.

In this case I either show the user a message that should read "The code was invalid" or I could simply start the rest of the game (but in this case I just print a message to tell the developer to do that)

That is all you need to do...

###BEFORE YOU DO ANYTHING, THOUGH
You may want to unzip the wub_emailtfa.zip file and make some modifications to the PHP class. At the moment I hardcode a very simple message to email to the player so you may want to edit what that says. In a future release I might create a GUI for the developer to use to that end but for now it is hardcoded into the plugin

Additionally, the timeout for the generated code is set to 10 minutes at the top of the same class. If you want to change that value, feel free to do so. There is simply a constant value at the top of the class to value and you are good to go.

Then simply zip up the folder again and you are ready to upload it to your website.

###The end

That's all there is to it. Enjoy! :D
