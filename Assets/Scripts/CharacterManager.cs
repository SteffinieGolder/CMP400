using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] GameObject char1;
    [SerializeField] GameObject char2;

    private Player char1PlayerScript;
    private Player char2PlayerScript;
    private CharMovement char1MovementScript;
    private CharMovement char2MovementScript;
    private CharBehaviourBase char1BehaviourScript;
    private CharBehaviourBase char2BehaviourScript;

    private CamFollow camFollowScript;

    public bool char1IsActive;
    public Player activePlayer;

    void Start()
    {
        char1PlayerScript = char1.GetComponent<Player>();
        char1MovementScript = char1.GetComponent<CharMovement>(); 
        char1BehaviourScript = char1.GetComponent<CharBehaviourBase>();

        char2PlayerScript = char2.GetComponent<Player>();
        char2MovementScript = char2.GetComponent<CharMovement>();
        char2BehaviourScript = char2.GetComponent<CharBehaviourBase>();

        camFollowScript = Camera.main.GetComponent<CamFollow>();

        SetChar2Active();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(char1IsActive)
            {
                SetChar2Active();
            }

            else
            {
                SetChar1Active();
            }
        }
    }

    void SetChar1Active()
    {
        char1IsActive = true; 
        char2PlayerScript.enabled = false;
        char2MovementScript.enabled = false;
        char2BehaviourScript.enabled = false;
        char1PlayerScript.enabled = true;
        char1MovementScript.enabled = true;
        char1BehaviourScript.enabled = true;
        char1BehaviourScript.ResetEnergyBar();
        camFollowScript.followTransform = char1.transform;
        GameManager.instance.uiManager.SwitchToolbar(char1IsActive);
        activePlayer = char1PlayerScript;
    }

    void SetChar2Active()
    {
        char1IsActive = false;
        char1PlayerScript.enabled = false;
        char1MovementScript.enabled = false;
        char1BehaviourScript.enabled = false;
        char2PlayerScript.enabled = true;
        char2MovementScript.enabled = true;
        char2BehaviourScript.enabled = true;
        char2BehaviourScript.ResetEnergyBar();
        camFollowScript.followTransform = char2.transform;
        GameManager.instance.uiManager.SwitchToolbar(char1IsActive);
        activePlayer = char2PlayerScript;
    }
}
