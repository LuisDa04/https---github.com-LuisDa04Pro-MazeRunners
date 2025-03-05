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
        public int current;

        public void StartGame()
        {
            Console.WriteLine
            (" Bienvenido al MazeRunners \n Este es un prototipo de juego de laberinto en consola \n Las instrucciones se encuentran en el README \n Disfruta del juego");
            Console.WriteLine(" Toque cualquier tecla para comenzar");

            Console.ReadLine();
            Console.WriteLine("Elige a los jugadores\n");

            UsefullMethods methods = new UsefullMethods(jugadores);

            while(true)
            {
                methods.ElegirJugadores();

                while(true)
                {
                    methods.DisplayMaze();
                    Console.WriteLine();
                    methods.ShowPlayer();

                    methods.HandleInput(jugadores[current].Speed);
                    if (methods.CheckWin())
                    {
                        Console.Clear();
                        AnsiConsole.Markup($"[green] Felicidades! El jugador {current + 1} llego al final del laberinto.[/]");
                        break;
                    }
                    Console.WriteLine("Si terminaste el turno pulsa Enter");
                    Console.ReadLine();
                    methods.SwitchTurn();
                }
            }
        }
    }
}