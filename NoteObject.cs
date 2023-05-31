using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    // Variable given to GameObjects in NoteHolder if coordinates coincide with button
    public bool canBePressed;

    // Key code to press for this note
    public KeyCode keyToPress;

    // Effects to instantiate when the note is hit or missed
    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    // Used to check if the note has been marked as hit before a message is sent to NoteMissed function
    public bool obtained;

    void Start()
    {
        obtained = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress)) // Check if the key is pressed
        {
            if (canBePressed) // Check if the note can be pressed (arrow is in the trigger zone)
            {
                obtained = true; // Mark the note as obtained

                gameObject.SetActive(false); // Deactivate the note GameObject

                // Following if statements are used to instantiate hit effects and calling the corresponding hit methods in the GameManager instance
                if (Mathf.Abs(transform.position.y) > 0.25) // Check the position of the note to determine the hit accuracy
                {
                    Debug.Log("Hit");
                    GameManager.instance.NormalHit();
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation); // (Instantiate brings in object to unity scene (object, position, rotation))
                }
                else if (Mathf.Abs(transform.position.y) > 0.05f)
                {
                    Debug.Log("Good");
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("Perfect");
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                }
            }
        }
    }

    // Check if notes are overlapping the trigger zone

    // Note is overlapping the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator") 
        {
            canBePressed = true; 
        }
    }

    // Note is not overlapping the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator") 
        {
            canBePressed = false;

            if (!obtained) // Check if the note has not been obtained (missed)
            {
                GameManager.instance.NoteMissed(); // Call the NoteMissed() method in the GameManager instance
                Instantiate(missEffect, transform.position, missEffect.transform.rotation); // Instantiate the miss effect
            }
        }
    }
}
