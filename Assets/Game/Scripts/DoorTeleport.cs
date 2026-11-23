using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DoorTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform destination;

    [Header("Interaction Hint")]
    [SerializeField] private GameObject interactionHintCanvas;
    [SerializeField] private TMP_Text interactionHintText;
    [SerializeField] private string hintText = "[E] Войти";

    private bool playerNearby;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == playerRigidbody)
        {
            playerNearby = true;
            ShowHint();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody == playerRigidbody)
        {
            playerNearby = false;
            HideHint();
        }
    }

    private void Update()
    {
        if (!playerNearby || playerRigidbody == null || destination == null)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            playerNearby = false;
            HideHint();

            playerRigidbody.position = destination.position;
        }
    }

    private void ShowHint()
    {
        if (interactionHintText != null)
            interactionHintText.text = hintText;

        if (interactionHintCanvas != null)
            interactionHintCanvas.SetActive(true);
    }

    private void HideHint()
    {
        if (interactionHintCanvas != null)
            interactionHintCanvas.SetActive(false);
    }
}
