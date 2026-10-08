using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    // Opcional: arrastrar un Particle System para que se reproduzca al ganar
    public ParticleSystem victoryParticles;

    private bool delivered;
    private bool playerInside;

    private Renderer zoneRenderer;
    private Color idleColor;
    private Color victoryColor;

    private GUIStyle titleStyle;
    private GUIStyle hintStyle;

    private void Start()
    {
        delivered = false;
        playerInside = false;

        idleColor = new Color(1f, 0.6f, 0.1f);     // naranja: la meta se distingue en el escenario
        victoryColor = new Color(0.2f, 1f, 0.4f);  // verde: entrega completada

        zoneRenderer = GetComponent<Renderer>();
        if (zoneRenderer != null)
            zoneRenderer.material.color = idleColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        // El jugador solo no alcanza: solo se registra para mostrar una pista
        if (other.GetComponentInParent<PlayerMovement>() != null)
            playerInside = true;

        CheckDelivery(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckDelivery(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
            playerInside = false;
    }

    // La victoria depende de que el objeto sea la caja y esté suelto (no cargada)
    private void CheckDelivery(Collider other)
    {
        if (delivered) return;

        PickableBox box = other.GetComponent<PickableBox>();
        if (box == null || box.IsCarried) return;

        Victory();
    }

    private void Victory()
    {
        delivered = true;

        // Señal visual de finalización
        if (zoneRenderer != null)
            zoneRenderer.material.color = victoryColor;

        if (victoryParticles != null)
            victoryParticles.Play();

        // Termina la partida: se detiene el generador de obstáculos
        Spawner spawner = FindFirstObjectByType<Spawner>();
        if (spawner != null)
            spawner.gameObject.SetActive(false);
    }

    private void OnGUI()
    {
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = 64;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.normal.textColor = Color.green;

            hintStyle = new GUIStyle(GUI.skin.label);
            hintStyle.fontSize = 22;
            hintStyle.fontStyle = FontStyle.Bold;
            hintStyle.alignment = TextAnchor.MiddleCenter;
            hintStyle.normal.textColor = Color.white;
        }

        if (delivered)
        {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "¡VICTORIA!", titleStyle);
            GUI.Label(new Rect(0, Screen.height / 2f + 40f, Screen.width, 40f), "Entregaste la caja", hintStyle);
        }
        else if (playerInside)
        {
            GUI.Label(new Rect(0, Screen.height - 80f, Screen.width, 40f),
                "Para ganar, soltá la caja (Q) dentro de la zona", hintStyle);
        }
    }
}
