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
        public string? Name { get; set; }
        
        public bool IsBlock(int x, int y, Casilla[,] maze)
        {
            return x>=0 && x<maze.GetLength(0) && y>=0 && y<maze.GetLength(1);
        }
    }

    public class EliminarTrampa : Habilidad
    {
        public EliminarTrampa()
        {
            Name = "Eliminar Trampa";
        }
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
                        AnsiConsole.Markup($"[red] Eliminando trampa en la posicion ({x}, {y})[/]\n");
                        maze[x, y] = new Camino((x,y));
                    }
                    
                }
            }
        }
    }

        public class AturdirTodos : Habilidad
        {
            public AturdirTodos()
            {
                Name = "Aturdir a todos";
            }
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
            public DuplicarVelocidad()  
            {
                Name = "Duplicar Velocidad";
            }
            public override void UseSkill(Jugador jugador, List<Jugador> jugadores, Casilla[,] maze)
            {
                AnsiConsole.Markup("[blue] Usando habilidad: Duplicar velocidad.[/]\n");
                jugador.Speed*= 2;
            }
        }

        public class Teleport : Habilidad
        {
            public Teleport()
            {
                Name = "Teleport";
            }
            public override void UseSkill(Jugador jugador, List<Jugador> jugadores, Casilla[,] maze)
            {
                Random rand = new Random();
                var currentPos = jugadores.Select(j => j.Posicion).ToList();

                int newX, newY;
                do
                {
                    newX = rand.Next(1, maze.GetLength(0) - 2);
                    newY = rand.Next(1, maze.GetLength(1) - 2);
                }
                while (!IsBlock(newX, newY, maze));

                if (!currentPos.Contains((newX,newY)))
                {
                    jugador.Posicion = (newX,newY);
                }
                else
                {
                    UseSkill(jugador,jugadores,maze);
                }

                AnsiConsole.Markup($"[blue] Usando habilidad: Transportandose a la casilla {(newX,newY)}.[/]\n");
                
            }
        }
        
}
