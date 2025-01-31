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
                new Jugador((0,0), 3, "Man1", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man2", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man3", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man4", false, AsignarHabilidad()),
                new Jugador((0,0), 3, "Man5", false, AsignarHabilidad())
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
                while(true)
                {
                    DisplayMaze();

                    Console.ReadLine();

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

                if (jugadores.Count == 2)
                {
                    List<(int,int)> Dir = new List<(int,int)> {(1,1), (ancho - 2,1), (1,altura - 2), (ancho - 2, altura -2)};
                    int Pos = rand.Next(Dir.Count);

                    jugadores[0].Posicion = Dir[Pos];
                    Dir.RemoveAt(Pos);
                    jugadores[1].Posicion = Dir[Pos];
                }
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
            Console.Clear();
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    maze[i,j].Display();
                    if ((i,j) == jugadores[0].Posicion)
                    {
                        jugadores[0].Display(maze, jugadores[0].Posicion);
                    }
                    if ((i,j) == jugadores[1].Posicion)
                    {
                        jugadores[1].Display(maze, jugadores[1].Posicion);
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
                        jugadores[current].Mover((-1,0), maze);
                        DisplayMaze();
                        break;
                    case ConsoleKey.S:
                        jugadores[current].Mover((1,0), maze);
                        DisplayMaze();
                        break;
                    case ConsoleKey.A:
                        jugadores[current].Mover((0,-1), maze);
                        DisplayMaze();
                        break;
                    case ConsoleKey.D:
                        jugadores[current].Mover((0,1), maze);
                        DisplayMaze();
                        break;
                    case ConsoleKey.P:
                        jugadores[current].Habilidad.UseSkill(jugadores[current], jugadores, maze);
                        DisplayMaze();
                        break;
                }
                count++;

            }
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