using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] GameObject char1;
    [SerializeField] GameObject char2;

    private Player char1PlayerScript;
    private Player char2PlayerScript;
    private CharMovement char1MovementScript;
    private CharMovement char2MovementScript;
    private CamFollow camFollowScript;

    private bool char1IsActive;

    void Start()
    {
        char1PlayerScript = char1.GetComponent<Player>();
        char1MovementScript = char1.GetComponent<CharMovement>();
        char2PlayerScript = char2.GetComponent<Player>();
        char2MovementScript = char2.GetComponent<CharMovement>();
        camFollowScript = Camera.main.GetComponent<CamFollow>();

        char1IsActive = false;
        char1PlayerScript.enabled = false;
        char1MovementScript.enabled = false;
        camFollowScript.followTransform = char2.transform;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(char1IsActive)
            {
                char2MovementScript.enabled = true;
                char2PlayerScript.enabled = true;
                char1PlayerScript.enabled = false;
                char1MovementScript.enabled = false;
                char1IsActive = false;
                camFollowScript.followTransform = char2.transform;
            }

            else
            {
                char2MovementScript.enabled = false;
                char2PlayerScript.enabled = false;
                char1PlayerScript.enabled = true;
                char1MovementScript.enabled = true;
                char1IsActive = true;
                camFollowScript.followTransform = char1.transform;
            }
        }
    }
}
