# TP1_Prg1 · Entrega de la caja

Prototipo 3D desarrollado en Unity para el **Trabajo Práctico Nº 1** de *Programación de Videojuegos I* (TUDIVJ, Facultad de Ingeniería, UNJu).

**Autora:** Esmeralda González Tolay

![Captura del escenario](Screenshots/Captura1.png)

## Descripción del juego

Controlás un personaje que recorre un escenario con plataformas, obstáculos y una plataforma móvil. El objetivo es **recoger una caja, transportarla hasta la zona de meta y soltarla dentro de ella**. Mientras avanzás, un generador lanza esferas que, si te tocan, te devuelven al inicio (y la caja vuelve a su lugar). En el camino hay un potenciador de velocidad temporal.

La victoria se activa **solo si la caja es depositada en la zona de entrega**: no alcanza con que entre el personaje.

## Versión de Unity

**2022.3.31f1**

## Controles

| Tecla | Acción |
|---|---|
| **W A S D** | Mover al personaje |
| **Espacio** | Saltar |
| **E** | Recoger la caja (a menos de 2 unidades) |
| **Q** | Soltar la caja |

## Mecánicas implementadas

| Consigna | Mecánica | Scripts |
|---|---|---|
| 1 | Escenario jugable, personaje con Rigidbody, cámara que lo sigue (esquiva obstáculos) | `PlayerMovement`, `IMovementStrategy`, `CameraFollow` |
| 2 | Plataforma móvil entre dos puntos; la pausa y el cambio de dirección se temporizan con `Invoke()` | `MovingPlatform` |
| 3 | Spawner que instancia un prefab de obstáculo con `InvokeRepeating()`; los obstáculos se eliminan con `Destroy` para no acumularse | `Spawner`, `Obstacle` |
| 4 | Recolección y transporte de la caja con `SetParent()`; al soltarla recupera su independencia y su física | `PickableBox`, `PlayerInteraction` |
| 5 | Power-Up temporal con corrutina: duración del efecto y período de recarga | `PowerUp` |
| 6 | Zona de entrega con Collider Trigger que verifica que lo depositado sea la caja; mensaje y cambio de color de victoria | `DeliveryZone` |

### Detalles de cada mecánica

- **Movimiento:** patrón *Strategy* (`SmoothMovement` y `AccelerateMovement`). El personaje se mueve con `Rigidbody` en los ejes X y Z, con salto.
- **Plataforma móvil:** al llegar a un extremo se detiene y `Invoke()` invierte la dirección tras 2 segundos. Arrastra al personaje mientras está encima.
- **Spawner:** genera una esfera cada 2 segundos y la lanza; cada instancia se destruye a los 6 segundos.
- **Caja:** al recogerla se hace hija del punto de transporte del jugador (kinematic y sin collider mientras se carga). Si el jugador se reinicia o la caja cae al vacío, vuelve a su lugar original.
- **Reinicio:** el personaje vuelve al inicio al ser tocado por una esfera o al caer al vacío.
- **Power-Up:** capacidad modificada: **velocidad** (x1.8 durante 5 s). Luego entra en recarga de 8 s en la que no puede reactivarse. Se indica con colores (verde disponible, gris recargando, jugador celeste con el efecto) y con un mensaje en pantalla.
- **Victoria:** al soltar la caja dentro de la zona, esta cambia a verde, aparece "¡VICTORIA!" y se detiene el Spawner.

## Cómo abrir y ejecutar el proyecto

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/LilEsme87/TP1_Prg1.git
   ```
2. Abrir **Unity Hub** > **Add** > **Add project from disk** y seleccionar la carpeta clonada.
3. Abrir el proyecto con la versión de Unity indicada arriba (Unity Hub ofrece instalarla si no la tenés).
4. En la ventana *Project*, abrir la escena `Assets/Scenes/SampleScene`.
5. Presionar **Play**.

## Estructura del proyecto

```
Assets/
  Materials/   Materiales del escenario y los objetos
  Prefabs/     Prefabs (obstáculo, plataforma móvil, etc.)
  Scenes/      SampleScene (escena principal)
  Scripts/     Scripts en C#
Packages/
ProjectSettings/
Screenshots/   Captura usada en este README
```

El archivo `.gitignore` excluye las carpetas autogeneradas de Unity (`Library/`, `Temp/`, `Logs/`, etc.).


## Capturas

### Escenario completo
![Escenario completo](Screenshots/Captura1.png)

### Plataformas móviles
![Plataformas móviles](Screenshots/Plataforma.png)

### Spawner de obstáculos
![Esferas generadas por el Spawner](Screenshots/Spawner.png)

### Transporte de la caja
![Jugador llevando la caja](Screenshots/Cajita.png)

### Power-Up de velocidad activo
![Power-Up de velocidad](Screenshots/PowerUp.png)

### Zona de entrega
![Pista al entrar a la zona de entrega](Screenshots/Meta1.png)

### Victoria
![Pantalla de victoria](Screenshots/Meta2.png)
