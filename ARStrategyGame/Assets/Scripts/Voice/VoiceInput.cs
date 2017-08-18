using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;
public class VoiceInput : MonoBehaviour {

    [SerializeField]
    private string[] m_Keywords;

    private SpeechRecognizerManager srManager;

    public MouseInputHandler mouseInputHandler;
    public Text debugText;

    public const int UNIT_TYPE_SWORD = 1;
    public const int UNIT_TYPE_AXE = 2;
    public const int UNIT_TYPE_SPEAR = 3;

    private const String UNIT_NAME_SWORD = "Sword";
    private const String UNIT_NAME_SPEAR = "Spear";
    private const String UNIT_NAME_AXE = "Axe";
    private const String MOVE = "Move";
    private const String GO = "Go";
    private const String ATTACK = "Attack";

    #region MONOBEHAVIOUR

	// Use this for initialization
	void Start () {
        m_Keywords = new string[9] { ATTACK, MOVE, GO, "Defend", "Test", "Hello", UNIT_NAME_SWORD, UNIT_NAME_SPEAR, UNIT_NAME_AXE };
		if (Application.platform != RuntimePlatform.Android) {
			DebugLog ("Speech recognition is only available on Android platform.");
			return;
		}

		if (!SpeechRecognizerManager.IsAvailable ()) {
			DebugLog ("Speech recognition is not available on this device.");
			return;
		}
		srManager = new SpeechRecognizerManager (gameObject.name);
    }

	void OnDestroy ()
	{
		if (srManager != null)
			srManager.Release ();
	}

    #endregion

    #region SPEECH_CALLBACKS
    void OnSpeechEvent (string e)
	{
		switch (int.Parse (e)) {
		case SpeechRecognizerManager.EVENT_SPEECH_READY:
			DebugLog ("Ready for speech");
            srManager.StartListening(5);
			break;
		case SpeechRecognizerManager.EVENT_SPEECH_BEGINNING:
			DebugLog ("User started speaking");
			break;
		case SpeechRecognizerManager.EVENT_SPEECH_END:
			DebugLog ("User stopped speaking");
            srManager.StartListening(5);
			break;
		}
	}

	void OnSpeechResults (string results)
	{

		// Need to parse
		string[] texts = results.Split (new string[] { SpeechRecognizerManager.RESULT_SEPARATOR }, System.StringSplitOptions.None);
        
        //Check if any of the texts are one of the commands
        for(int i = 0; i < texts.Length; i++) {
            switch(texts[i]) {
                case UNIT_NAME_SWORD: 
                    mouseInputHandler.SetSelectedUnitType(MouseInputHandler.UNIT_TYPE_SWORD);
                    break;
                case UNIT_NAME_AXE: 
                    mouseInputHandler.SetSelectedUnitType(MouseInputHandler.UNIT_TYPE_AXE);
                    break;
                case UNIT_NAME_SPEAR: 
                    mouseInputHandler.SetSelectedUnitType(MouseInputHandler.UNIT_TYPE_SPEAR);
                    break;
                case MOVE:
                case ATTACK:
                case GO:
                     mouseInputHandler.MoveCenter();
                     break;
            }
        }
	}

    void OnSpeechError (string error)
	{
        //None
        switch (int.Parse (error)) {
		case SpeechRecognizerManager.ERROR_AUDIO:
			DebugLog ("Error during recording the audio.");
			break;
		case SpeechRecognizerManager.ERROR_CLIENT:
			DebugLog ("Error on the client side.");
			break;
		case SpeechRecognizerManager.ERROR_INSUFFICIENT_PERMISSIONS:
			DebugLog ("Insufficient permissions. Do the RECORD_AUDIO and INTERNET permissions have been added to the manifest?");
			break;
		case SpeechRecognizerManager.ERROR_NETWORK:
			DebugLog ("A network error occured. Make sure the device has internet access.");
			break;
		case SpeechRecognizerManager.ERROR_NETWORK_TIMEOUT:
			DebugLog ("A network timeout occured. Make sure the device has internet access.");
			break;
		case SpeechRecognizerManager.ERROR_NO_MATCH:
			DebugLog ("No recognition result matched.");
			break;
		case SpeechRecognizerManager.ERROR_NOT_INITIALIZED:
			DebugLog ("Speech recognizer is not initialized.");
			break;
		case SpeechRecognizerManager.ERROR_RECOGNIZER_BUSY:
			DebugLog ("Speech recognizer service is busy.");
			break;
		case SpeechRecognizerManager.ERROR_SERVER:
			DebugLog ("Server sends error status.");
			break;
		case SpeechRecognizerManager.ERROR_SPEECH_TIMEOUT:
			DebugLog ("No speech input.");
			break;
		default:
			break;
		}

        srManager.StartListening(5);
	}

    #endregion

	private void DebugLog (string message)
	{
		Debug.Log (message);
		debugText.text = message + "\n" + debugText.text;
	}
}