using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace MazeRunners
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame();
        }
    }

    public class GameManager
    {
        public List<Jugador> jugadores = new List<Jugador>();
        public List<Jugador> tokens = new List<Jugador>();
        public int current;
        public Casilla[,] maze = new Casilla[51,51];
        public MazeGenerator build = new MazeGenerator(51,51);
        public (int x,int y) winner;
        public Random rand = new Random();

        public void StartGame()
        {
            int ancho = 51;
            int altura = 51;

            build = new MazeGenerator(ancho, altura);
            maze = build.GenerateMaze();

            winner = (ancho/2,altura/2);
            
            (int,int)[] array = {(1,1), (ancho - 2, altura - 2)};

            tokens = new List<Jugador>
            {
                new Jugador((0,0), 3, "Man1", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man2", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man3", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man4", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man5", false, AsignarHabilidad())
            };
            current = 0;

            while(true)
            {
                Console.WriteLine
                (" Bienvenido al MazeRunners \n Este es un prototipo de juego de laberinto en consola \n Las instrucciones se encuentran en el README \n Disfruta del juego");
                Console.WriteLine(" Toque cualquier tecla para comenzaer");

                Console.ReadLine();
                Console.WriteLine("Elige a los jugadores");
                while (jugadores.Count < 2)
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}: {tokens[i].Name}\n");
                    }
                        
                    int key = int.Parse(Console.ReadLine());
                    switch(key)
                    {
                        case 1:
                            jugadores.Add(tokens[0]);
                            break;
                        case 2:
                            jugadores.Add(tokens[1]);
                            break;
                        case 3:
                            jugadores.Add(tokens[2]);
                            break;
                        case 4:
                            jugadores.Add(tokens[3]);
                            break;
                        case 5:
                            jugadores.Add(tokens[4]);
                            break;
                    }
                    tokens.RemoveAt(key - 1);
                }
                tokens.Clear();
                jugadores[0].Posicion = (1,1);
                jugadores[1].Posicion = (ancho -2, altura - 2);

                Console.Clear();
                DisplayMaze();

                Console.ReadLine();

                HandleInput(jugadores[current].Speed);
                if (CheckWin())
                {
                    AnsiConsole.Markup($"[green] Felicidades! El jugador {current + 1} llego al final del laberinto.[/]");
                    break;
                }
                Console.ReadLine();
                SwitchTurn();
            }
        }

        
        private Habilidad AsignarHabilidad()
        {
            var habilidades = new List<Habilidad>
            {
                new EliminarTrampa(),
                new AturdirTodos(),
                new DuplicarVelocidad(),
                new Teleport()
            };
            
            int index = rand.Next(habilidades.Count);
            return habilidades[index];
        }

        public void DisplayMaze()
        {

            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    maze[i,j].Display();
                    for (int k = 0; k < jugadores.Count; k++)
                    {
                        jugadores[k].Display(maze, jugadores[k].Posicion);
                    }
                }
                Console.WriteLine();
            }
        }

        private void HandleInput(int velocidad)
        {
            int count = 0;
            while (count < velocidad)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        jugadores[current].Mover((0,-1), maze);
                        break;
                    case ConsoleKey.S:
                        jugadores[current].Mover((0,1), maze);
                        break;
                    case ConsoleKey.A:
                        jugadores[current].Mover((-1,0), maze);
                        break;
                    case ConsoleKey.D:
                        jugadores[current].Mover((1,0), maze);
                        break;
                    case ConsoleKey.P:
                        jugadores[current].Habilidad.UseSkill(jugadores[current], jugadores, maze);
                        break;
                }
                count++;
                
            }
        }
        private bool Turn()
        {
            return current == 0 ? jugadores[0].IsPlaying = true : jugadores[1].IsPlaying = true;
        }

        private void SwitchTurn()
        {
            current = (current + 1) % jugadores.Count;
        }

        private bool CheckWin()
        {
            return jugadores[current].Posicion == winner;
        }
    }
}