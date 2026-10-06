using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private enum PowerUpState { Ready, Active, Cooldown }

    private PowerUpState state;

    private float speedMultiplier;  // cuánto aumenta la velocidad
    private float effectDuration;   // segundos que dura el efecto
    private float cooldownTime;     // segundos de recarga

    private float stateEndTime;     // para mostrar el tiempo restante en pantalla

    private Renderer powerUpRenderer;
    private Color readyColor;
    private Color cooldownColor;
    private Color boostPlayerColor;

    private GUIStyle labelStyle;

    private void Start()
    {
        speedMultiplier = 1.8f;
        effectDuration = 5f;
        cooldownTime = 8f;

        readyColor = new Color(0.2f, 1f, 0.4f);       // verde: disponible
        cooldownColor = new Color(0.4f, 0.4f, 0.4f);  // gris: recargando
        boostPlayerColor = Color.cyan;                // el jugador se ve celeste con el efecto

        powerUpRenderer = GetComponent<Renderer>();
        SetPowerUpColor(readyColor);
        state = PowerUpState.Ready;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo se activa si está disponible: no se puede reactivar durante la recarga
        if (state != PowerUpState.Ready) return;

        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player == null) return;

        StartCoroutine(PowerUpRoutine(player));
    }

    private IEnumerator PowerUpRoutine(PlayerMovement player)
    {
        PlayerStats stats = player.Stats;
        Renderer playerRenderer = player.GetComponentInChildren<Renderer>();
        Color playerOriginalColor = playerRenderer != null ? playerRenderer.material.color : Color.white;

        // 1) ACTIVO: aumenta la velocidad
        state = PowerUpState.Active;
        stats.Velocity = stats.BaseVelocity * speedMultiplier;
        if (playerRenderer != null) playerRenderer.material.color = boostPlayerColor;
        SetPowerUpColor(cooldownColor);
        stateEndTime = Time.time + effectDuration;

        yield return new WaitForSeconds(effectDuration);

        // 2) FIN DEL EFECTO: se restablece el estado original
        stats.ResetVelocity();
        if (playerRenderer != null) playerRenderer.material.color = playerOriginalColor;

        // 3) RECARGA: no se puede volver a usar
        state = PowerUpState.Cooldown;
        stateEndTime = Time.time + cooldownTime;

        yield return new WaitForSeconds(cooldownTime);

        // 4) DISPONIBLE de nuevo
        state = PowerUpState.Ready;
        SetPowerUpColor(readyColor);
    }

    private void SetPowerUpColor(Color color)
    {
        if (powerUpRenderer != null)
            powerUpRenderer.material.color = color;
    }

    // Mensaje en pantalla para que el jugador sepa el estado
    private void OnGUI()
    {
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 22;
            labelStyle.fontStyle = FontStyle.Bold;
        }

        float remaining = Mathf.Max(0f, stateEndTime - Time.time);
        string text;

        switch (state)
        {
            case PowerUpState.Active:
                text = "¡VELOCIDAD x" + speedMultiplier + "!  " + remaining.ToString("0.0") + " s";
                labelStyle.normal.textColor = Color.cyan;
                break;
            case PowerUpState.Cooldown:
                text = "Power-Up recargando...  " + remaining.ToString("0.0") + " s";
                labelStyle.normal.textColor = Color.gray;
                break;
            default:
                text = "Power-Up disponible";
                labelStyle.normal.textColor = Color.green;
                break;
        }

        GUI.Label(new Rect(20, 20, 600, 40), text, labelStyle);
    }
}
