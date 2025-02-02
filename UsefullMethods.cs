using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazeRunners
{
    public class UsefullMethods
    {
        public int ancho;
        public int altura;
        public (int,int) winner;
        public Casilla[,] maze;
        public List<Jugador> tokens;
        public List<Jugador> jugadores;
        public Random rand = new Random();
        public int current = 0;
        public UsefullMethods(List<Jugador> Jugadores)
        {
            jugadores = Jugadores;
            ancho = 31;
            altura = 31;
            maze = new Casilla[ancho,altura];
            winner = (ancho/2,altura/2);
            tokens = new List<Jugador>
            {
            new Jugador((0,0), 3, "Man1", new EliminarTrampa()),
            new Jugador((0,0), 3, "Man2", new Teleport()),
            new Jugador((0,0), 3, "Man3", new AturdirTodos()),
            new Jugador((0,0), 3, "Man4", new DuplicarVelocidad()),
            new Jugador((0,0), 3, "Man5", new Teleport())
            };
            MazeGenerator generator = new MazeGenerator(ancho, altura);
            maze = generator.GenerateMaze();
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

        public void ShowPlayer()
        {
            Console.WriteLine($"Jugador actual: {jugadores[current].Name}");
            Console.WriteLine($"Posición actual: {jugadores[current].Posicion}");
            Console.WriteLine($"Velocidad: {jugadores[current].Speed}");
            Console.WriteLine();
        }

        public void HandleInput(int velocidad)
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

        public void SwitchTurn()
        {
            current = (current + 1) % jugadores.Count;
        }

        public bool CheckWin()
        {
            return jugadores[current].Posicion == winner;
        }

    }
}