using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Interactable current;

    void Update()
    {
        if (current != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E on: " + current.gameObject.name);
            current.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null)
        {
            Debug.Log("Entered interactable: " + other.gameObject.name);
            current = interactable;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null && interactable == current)
        {
            Debug.Log("Exited interactable: " + other.gameObject.name);
            current = null;
        }
    }
}
