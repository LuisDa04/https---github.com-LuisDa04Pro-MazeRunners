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
        public int ancho = 31;
        public int altura = 31;
        public List<Jugador> jugadores = new List<Jugador>();
        public List<Jugador> tokens = new List<Jugador>();
        public int current;
        public Casilla[,] maze = new Casilla[31,31];
        public MazeGenerator build = new MazeGenerator(31,31);
        public (int x,int y) winner;
        public Random rand = new Random();

        public void StartGame()
        {

            build = new MazeGenerator(ancho, altura);
            maze = build.GenerateMaze();

            winner = (ancho/2,altura/2);
            
            (int,int)[] array = {(1,1), (ancho - 2, altura - 2)};


            tokens = new List<Jugador>
            {
                new Jugador((0,0), 3, "Man1", new EliminarTrampa()),
                new Jugador((0,0), 3, "Man2", new Teleport()),
                new Jugador((0,0), 3, "Man3", new AturdirTodos()),
                new Jugador((0,0), 3, "Man4", new DuplicarVelocidad()),
                new Jugador((0,0), 3, "Man5", new Teleport())
            };
            current = 0;

            Console.WriteLine
            (" Bienvenido al MazeRunners \n Este es un prototipo de juego de laberinto en consola \n Las instrucciones se encuentran en el README \n Disfruta del juego");
            Console.WriteLine(" Toque cualquier tecla para comenzar");

            Console.ReadLine();
            Console.WriteLine("Elige a los jugadores\n");

            while(true)
            {
                ElegirJugadores();
                Console.WriteLine($"{jugadores[0].Posicion}");
                Console.WriteLine($"{jugadores[1].Posicion}");

                while(true)
                {
                    DisplayMaze();
                    Console.WriteLine();
                    ShowPlayer();

                    HandleInput(jugadores[current].Speed);
                    if (CheckWin())
                    {
                        AnsiConsole.Markup($"[green] Felicidades! El jugador {current + 1} llego al final del laberinto.[/]");
                        break;
                    }
                    Console.WriteLine("Si terminaste el turno pulsa Enter");
                    Console.ReadLine();
                    SwitchTurn();
                }
            }
        }

        public void ElegirJugadores()
        {
            while (jugadores.Count < 2)
            {
                for (int i = 0; i < tokens.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {tokens[i].Name} Skill: {tokens[i].Habilidad.Name}\n");
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
                    default:
                        throw new ArgumentException("Por favor presiona los numeros dados para elegir el jugador");
                }
                tokens.RemoveAt(key - 1);
                
                List<(int,int)> Dir = new List<(int,int)> {(1,1), (ancho - 2,1), (1,altura - 2), (ancho - 2, altura -2)};
                
                for (int i = 0; i < jugadores.Count; i++)
                {
                    bool assign = false;
                    (int,int)? position = null;

                    while (!assign)
                    {
                        int Pos = rand.Next(Dir.Count);
                        position = Dir[Pos];

                        if (position.HasValue)
                        {
                            assign = true;
                        }
                        else
                        {
                            Dir[Pos] = Dir.Last();
                            Dir.RemoveAt(Dir.Count - 1);
                        }
                    }

                    jugadores[i].Posicion = position.Value;
                }
            }
        }

        public void DisplayMaze()
        {
            Console.Clear();
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if ((i,j) == jugadores[0].Posicion)
                    {
                        maze[i,j].DisplayPlayer();
                    }
                    else if ((i,j) == jugadores[1].Posicion)
                    {
                        maze[i,j].DisplayPlayer();
                    }
                    else
                    {
                    maze[i,j].Display();
                    }
                }
                Console.WriteLine();
            }
        }

        public void ShowPlayer()
        {
            Console.WriteLine($"Jugador actual: {jugadores[current].Name}");
            Console.WriteLine($"Posición actual: {jugadores[current].Posicion}");
            Console.WriteLine($"Velocidad: {jugadores[current].Speed}");
            Console.WriteLine();
        }

        private void HandleInput(int velocidad)
        {
            Console.WriteLine("Pulse Enter si desea iniciar su turno");
            Console.ReadLine();
            int count = 0;
            int pcount = 0;
            int maxUses = 1;
            var tempSpeed = jugadores[current].Speed;

            while (count < velocidad)
            {
                var temp = jugadores[current].Posicion;
                var key = Console.ReadKey(true).Key;

                if (jugadores[current].Status == "Stunned")
                {
                    break;
                }
                
                switch (key)
                {
                    case ConsoleKey.W:
                        jugadores[current].Mover((-1,0), maze);
                        Console.Clear();
                        DisplayMaze();
                        break;
                    case ConsoleKey.S:
                        jugadores[current].Mover((1,0), maze);
                        Console.Clear();
                        DisplayMaze();
                        break;
                    case ConsoleKey.A:
                        jugadores[current].Mover((0,-1), maze);
                        Console.Clear();
                        DisplayMaze();
                        break;
                    case ConsoleKey.D:
                        jugadores[current].Mover((0,1), maze);
                        Console.Clear();
                        DisplayMaze();
                        break;
                    case ConsoleKey.P:
                        if (pcount < maxUses)
                        {
                            jugadores[current].Habilidad.UseSkill(jugadores[current], jugadores, maze);
                            if (jugadores[current].Speed == 6)
                            {
                                velocidad = 6;
                            }
                            Console.Clear();
                            DisplayMaze();
                            pcount++;
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    default:
                        throw new ArgumentException("Por favor presiona WASD para moverte o P para activar el poder");
                }

                if (jugadores[current].Posicion != temp)
                {
                    count++;
                }
                if (key == ConsoleKey.P && jugadores[current].Posicion != temp)
                {
                    count--;
                }
                ShowPlayer();
                
                if (CheckWin())
                {
                    break;
                }

            }
            jugadores[current].Speed = tempSpeed;
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