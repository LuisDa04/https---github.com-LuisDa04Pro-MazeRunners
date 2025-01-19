// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Spectre.Console;

// namespace MazeRunners
// {
//     public class Program
//     {
//         public static void Main(string[] args)
//         {
//             GameManager gameManager = new GameManager();
//             gameManager.StartGame();
//         }
//     }

//     public class GameManager
//     {
//         private List<Jugador> jugadores = new List<Jugador>();
//         private int current;
//         private Casilla[,] maze = new Casilla[50,50];
//         private MazeGenerator build = new MazeGenerator(50,50);
//         private (int x,int y) winner;
//         private Random rand = new Random();

//         public void StartGame()
//         {
//             Console.WriteLine("Estoy en Start Game");
//             int ancho = 50;
//             int altura = 50;

//             build = new MazeGenerator(ancho, altura);
//             maze = build.GenerateMaze();

//             winner = (ancho/2,altura/2);

//             jugadores = new List<Jugador>
//             {
//                 new Jugador((1,1), 3, "Chuchito", AsignarHabilidad()),
//                 new Jugador((ancho - 2,altura - 2), 3, "DragonBall", AsignarHabilidad()),
//                 new Jugador((ancho - 2,1), 3, "Chuchurron", AsignarHabilidad()),
//                 new Jugador((1,altura - 2), 3, "Pelly", AsignarHabilidad())
//             };
//             current = 0;

//             while(true)
//             {
//                 Console.WriteLine("Estoy en el bucle");
//                 Console.Clear();
//                 DisplayMaze();
//                 HandleInput();
//                 if (CheckWin())
//                 {
//                     AnsiConsole.Markup($"[green] Felicidades! El jugador {current + 1} llego al final del laberinto.[/]");
//                     break;
//                 }
//                 SwitchTurn();
//             }
//         }

//         private Habilidad AsignarHabilidad()
//         {
//             var habilidades = new List<Habilidad>
//             {
//                 new DetectarTrampa(),
//                 new AturdirTodos(),
//                 new DuplicarVelocidad(),
//                 new EnviarJugadorAtras()
//             };
            
//             int index = rand.Next(habilidades.Count);
//             return habilidades[index];
//         }

//         public void DisplayMaze()
//         {
//             for (int i = 0; i < maze.GetLength(1); i++)
//             {
//                 for (int j = 0; j < maze.GetLength(0); j++)
//                 {
//                     bool player = false;
//                     for (int k = 0; k < jugadores.Count; i++)
//                     {
//                         if (jugadores[k].Posicion == (i,j))
//                         {
//                             jugadores[k].Display(i == current);
//                             player = true;
//                             break;
//                         }
//                     }
//                     if (!player) maze[i,j].Display();
//                 }
//                 Console.WriteLine();
//             }
//             AnsiConsole.Markup($"[yellow][/]");
//         }

//         private void HandleInput()
//         {
//             var key = Console.ReadKey(true).Key;

//             switch (key)
//             {
//                 case ConsoleKey.W:
//                     jugadores[current].Mover((0,-1), maze);
//                     break;
//                 case ConsoleKey.S:
//                     jugadores[current].Mover((0,1), maze);
//                     break;
//                 case ConsoleKey.A:
//                     jugadores[current].Mover((-1,0), maze);
//                     break;
//                 case ConsoleKey.D:
//                     jugadores[current].Mover((1,0), maze);
//                     break;
//             }
//         }

//         private void SwitchTurn()
//         {
//             current = (current + 1) % jugadores.Count;
//         }

//         private bool CheckWin()
//         {
//             return jugadores[current].Posicion == winner;
//         }
//     }
// }