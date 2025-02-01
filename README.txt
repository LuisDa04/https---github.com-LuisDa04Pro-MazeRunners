# MazeRunners

Un juego basico de laberinto en consola con habilidades especiales.

## Ejecutar el proyecto

1. Abre Visual Studio Code.
2. Navega hasta la carpeta del proyecto.
3. Presiona Ctrl+Shift+B para compilar y ejecutar el proyecto.
4. Una vez compilado, ejecuta el programa.
5. Sigue las instrucciones en pantalla para comenzar el juego.

## Controles y Reglas

### Controles
- Al ingresar al juego debe elegir los personajes para comenzar su aventura

Controles de ambos jugadores:
- W: Mover hacia arriba
- S: Mover hacia abajo  
- A: Mover hacia la izquierda
- D: Mover hacia la derecha
- P: Usar habilidad especial

### Reglas

- El objetivo es llegar al punto final del laberinto. (la estrella)
- Cada jugador tiene una habilidad especial.
- Las habilidades pueden usarse una vez por ronda.
- Los jugadores aturdidos deben esperar su próximo turno.
- Antes de cada accion leer bien la consola generalmente debe presionar Enter antes de realizar alguna accion.

## Resumen del Codigo

- Se utiliza el algoritmo de Kruskal para generar un laberinto de Casillas, instanciando las casillas desde una clase abstracta, de la cual heredan casi todos los objetos del laberinto.
- El juego inicia en las esquinas del laberinto, y los jugadores deben usar un array de direcciones para moverse por el laberinto.
- Las habilidades estan bien implementadas en una clase abstracta bien organizadas.
- La clase Jugador alberga unos pocos metodos como el movimiento y la activacion de trampas.
- La clase Game Manager es la que controla el flujo del juego y genera la interaccion del programa con el usuario, mediante "Console.ReadKey()" y estructuras como "switch".

### Estructuras del juego
- Casillas: pueden ser Camino, Trampa o Muro.
--Trampas:
---TeleportTrap: teletransporta a un jugador a un lugar aleatorio.
---SlowTrap: reduce la velocidad del jugador.
---StunTrap: stunea al jugador (finaliza su turno)

-Jugadores
--Habilidades:
---Teleport: teletransporta al jugador a un lugar aleatorio.
---EliminarTrampas: elimina trampas en 3 casillas alrededor.
---DuplicarVelocidad: exactamente lo que dice su nombre.
---AturdirTodos: aplica stun a todos los jugadores contrarios.



