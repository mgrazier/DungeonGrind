using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	private GameManager _GameManager;
	
	private int _buttonPosX  = (int)(Screen.width  * 0.165f);
	private int _buttonPosY  = (int)(Screen.height * 0.10f);
	private int _buttonSizeX = (int)(Screen.width  * 0.66f);
	private int _buttonSizeY = (int)(Screen.height * 0.10f);
	private int _offsetY     = (int)(Screen.height * 0.05f);
	
	//private string _buttonNewGameString = "New Game (Erase Save)";
	private int   _confirmTry           = 0;
	private GameObject _Menu_Panel;
	private string     _Menu_PanelString;
	
	private GameObject _Button_NewGame;
	private GameObject _Button_LoadGame;
	private GameObject _Label_SaveInfo;
	
	//private bool _isGameStarted = false;
	// Use this for initialization
	void Start () {
		
		_GameManager   = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameManager>();
		_GameManager.ChangeState("Menu");
		
		_Menu_PanelString = "UI Root (2D)/Camera/Anchor/MenuPanel";
		_Menu_Panel = transform.FindChild(_Menu_PanelString).gameObject;
		if(_Menu_Panel == null){Debug.Log ("[MenuManager.Start()] - Can't find MenuPanel");}
		
		_Button_NewGame = _Menu_Panel.transform.FindChild("Button (New Game)").gameObject;
		if(_Button_NewGame == null){Debug.Log ("[MenuManager.Start()] - Can't find Button(New Game)");}
		
		_Button_LoadGame = _Menu_Panel.transform.FindChild("Button (Load Game)").gameObject;
		if(_Button_LoadGame == null){Debug.Log ("[MenuManager.Start()] - Can't find Button(Load Game)");}
		
		_Label_SaveInfo = _Menu_Panel.transform.FindChild("Label (SaveInfo)").gameObject;
		if(_Button_LoadGame == null){Debug.Log ("[MenuManager.Start()] - Can't find Label (SaveInfo)");}
		
		SaveLoadSystem.TestForSave();
		if(SaveLoadSystem.IsSaveExist) //If save exist
		{
			_Button_LoadGame.transform.FindChild ("Label").GetComponent<UILabel>().text = "Load Game"; // Display the Load Game text
		}
		else
		{
			_Button_LoadGame.transform.FindChild ("Label").GetComponent<UILabel>().text = "[AAAAAA]Save not found. Can't load.[-]"; // Display the No Game found text
		}
		
		//UIEventListener.Get(_Button_NewGame ).onClick += NewGameDelegate;  // Add the NewGame  Delegate to the New  Game Button
		//UIEventListener.Get(_Button_LoadGame).onClick += LoadGameDelegate; // Add the LoadGame Delegate to the Load Game button
		
		DisplaySaveInformation();
	}
	
	// Remove Listener on disable of the gameobject
	/*void OnDisable()
	{
		//UIEventListener.Get(_Button_NewGame ).onClick -= NewGameDelegate;
		//UIEventListener.Get(_Button_LoadGame).onClick -= LoadGameDelegate;
	}*/
	
	void DisplaySaveInformation()
	{
		string _SaveInfo;
		
		if(SaveLoadSystem.IsSaveExist) //If save exist
		{
			_SaveInfo  = "==> SAVE FOUND <== \n \n";
			_SaveInfo += "Total Skill Level : " + Character.CalculateSavedSkillLevel()   + "\n";
			_SaveInfo += "Dungeon level     : " + PlayerPrefs.GetInt ("MaxDungeonLevel") + "\n\n";
			
			_SaveInfo += "Influence Points  : " + PlayerPrefs.GetInt ("InfluencePoints") + "\n";
			_SaveInfo += "Save Version      : " + PlayerPrefs.GetString ("SavedVersion") + "\n";
			_SaveInfo += "Current Version   : " + _GameManager.CurVersion;
			
			/* This is the old OnGUI approach
			GUI.Label(new Rect(_buttonPosX, _boxPosY           , _buttonSizeX, 25.0f),"==> SAVE FOUND <== ");
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 1f*25.0f, _buttonSizeX, 25.0f),"Total Skill Level : " + Character.CalculateSavedSkillLevel());
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 2f*25.0f, _buttonSizeX, 25.0f),"Dungeon level : " + PlayerPrefs.GetInt ("MaxDungeonLevel"));
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 3f*25.0f, _buttonSizeX, 25.0f),"Influence : "     + PlayerPrefs.GetInt ("InfluencePoints"));
			*/
		}								   
		else							
		{	
			_SaveInfo = "Save not found. Start a new game.";
			//GUI.Label(new Rect(_buttonPosX, _boxPosY + 0.5f*_offsetY, _buttonSizeX, 25.0f),"Save not found. Start a new game.");
		}
		
		_Label_SaveInfo.GetComponent<UILabel>().text = _SaveInfo; // Display the SaveInfo on the menu
	}
	
	// Delegate called when the New Game button is clicked
	void NewGameDelegate(GameObject _ButtonClicked)
	{
		
		if(_confirmTry == 0 && SaveLoadSystem.IsSaveExist == true)
		{
			_confirmTry++;
			_Button_NewGame.transform.FindChild("Label").GetComponent<UILabel>().text = "Are you sure you want to erase your save?";
		}
		else if(_confirmTry == 1)
		{
			_confirmTry++;
			_Button_NewGame.transform.FindChild("Label").GetComponent<UILabel>().text = "THIS WILL RESET YOUR SAVE, ARE YOU SURE?";
		}
		else
		{
			PlayerPrefs.DeleteAll();
			NewGame();
		}
	}
	
	// Delegate called when the Load Game button is clicked
	void LoadGameDelegate(GameObject _ButtonClicked)
	{
		if(SaveLoadSystem.IsSaveExist == true) //If save exist
		{
			StartGame();
		}
	}
	
	/*void OnGUI()
	{
		float _boxPosX;
		float _boxPosY;
		if(GUI.Button(new Rect(_buttonPosX, _buttonPosY, _buttonSizeX, _buttonSizeY), _buttonNewGameString))
		{
			if(PlayerPrefs.GetInt ("IsSaveExist") == 1)
			{
				
			}
			else
			{
				
				NewGame();
			}	
		}
		if(SaveLoadSystem.IsSaveExist) //If save exist
		{
			GUI.enabled = false;
		}
		
		if(GUI.Button(new Rect(_buttonPosX, _buttonPosY + _buttonSizeY + _offsetY, _buttonSizeX, _buttonSizeY), "Load Game"))
		{
			StartGame();
		}
		GUI.enabled = true;
		
		
		// Saved stats Display
		_boxPosX = _buttonPosX;
		_boxPosY = _buttonPosY + 2 * _buttonSizeY + 2 *_offsetY;
		GUI.Box(new Rect(_buttonPosX, _boxPosY, _buttonSizeX, _buttonSizeY*3),"");
		
		if(SaveLoadSystem.IsSaveExist) //If save exist
		{
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 0.5f*_offsetY, _buttonSizeX, 25.0f),"Save not found. Start a new game.");
		}								   
		else							
		{								   
			GUI.Label(new Rect(_buttonPosX, _boxPosY           , _buttonSizeX, 25.0f),"==> SAVE FOUND <== ");
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 1f*25.0f, _buttonSizeX, 25.0f),"Total Skill Level : " + Character.CalculateSavedSkillLevel());
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 2f*25.0f, _buttonSizeX, 25.0f),"Dungeon level : " + PlayerPrefs.GetInt ("MaxDungeonLevel"));
			GUI.Label(new Rect(_buttonPosX, _boxPosY + 3f*25.0f, _buttonSizeX, 25.0f),"Influence : "     + PlayerPrefs.GetInt ("InfluencePoints"));
		}
	}*/
	
	
	void NewGame()
	{
		//if(_isGameStarted == false) // TODO: This should be removed. It is there because the button double click instead of single-clicking. This should be fixed.
		//{
			//_isGameStarted = true;
			PlayerPrefs.DeleteAll();
			if(Input.GetKey(KeyCode.LeftShift))
			{
				_GameManager.MaxDungeonLevel = 20;
				Character.InfluencePoints = 500;
				Inventory.RessourceList[(int)RessourceName.Wood].CurValue = 1000;
				Inventory.RessourceList[(int)RessourceName.Rock].CurValue = 1000;
				Inventory.RessourceList[(int)RessourceName.Coin].CurValue = 1000;
			}
			StartGame();
		//}
		
	}
	void StartGame()
	{
		Application.LoadLevel("Camp");
		GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<PlayerHUD>().enabled = true;
		GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>().IniGame();
	}
}
