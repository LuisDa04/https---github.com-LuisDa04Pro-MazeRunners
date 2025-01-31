using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace MazeRunners
{
    public abstract class Habilidad
    {
        public abstract void UseSkill(Jugador jugador, List<Jugador> jugadores, Casilla[,] maze);
    }

    public class EliminarTrampa : Habilidad
    {
        public override void UseSkill(Jugador jugador, List<Jugador> jugadores, Casilla[,] maze)
        {
            AnsiConsole.Markup("[blue] Usando habilidad: Eliminar Trampas.[/]\n");
            var dir = new List<(int x,int y)> { (0,1), (1,0), (0,-1), (-1,0) };

            foreach (var(dx,dy) in dir)
            {
                for (int i = 0; i <= 3; i++)
                {
                    int x = jugador.Posicion.x + dx * i;
                    int y = jugador.Posicion.y + dy * i;
                    if (IsBlock(x, y, maze) && maze[x,y] is Trampa)
                    {
                        AnsiConsole.Markup($"[red] Eliminanado trampa en la posicion ({x}, {y})[/]\n");
                        maze[x, y] = new Camino((x,y));
                    }
                }
            }
        }
    
        private bool IsBlock(int x, int y, Casilla[,] maze) => x>=0 && x<maze.GetLength(0) && y>=0 && y<maze.GetLength(1);
    }
        public class AturdirTodos : Habilidad
        {
            public override void UseSkill(Jugador jugador, List<Jugador> jugadores, Casilla[,] maze)
            {
                AnsiConsole.Markup("[blue] Usando habilidad: Aturdir a todos los jugadores.[/]\n");
                foreach (var j in jugadores)
                {
                    if (j != jugador)
                    {
                        j.Status = "Stunned";
                    }
                }
            }
        }

        public class DuplicarVelocidad : Habilidad
        {
            public override void UseSkill(Jugador jugador, List<Jugador> jugadores, Casilla[,] maze)
            {
                AnsiConsole.Markup("[blue] Usando habilidad: Duplicar velocidad.[/]\n");
                jugador.Speed*= 2;
            }
        }

        public class Teleport : Habilidad
        {
            public override void UseSkill(Jugador jugador, List<Jugador> jugadores, Casilla[,] maze)
            {
                int i = 0;
                int j = 0;

                while (maze[i,j] is Muro || maze[i,j] is Trampa)
                {
                    Random rand = new Random(); 
                    i = rand.Next(1,49);
                    j = rand.Next(1,49);
                }

                jugador.Posicion = (i,j);
                AnsiConsole.Markup($"[blue] Usando habilidad: Transportandose a la casilla {(i,j)}.[/]\n");
                
            }
        }
}