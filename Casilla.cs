﻿﻿﻿using Spectre.Console;

namespace MazeRunners
{
    public abstract class Casilla
    {
        public (int x, int y) Posicion {get; set;}
        public string? Tipo {get; set;}

        public abstract void Display();
        public abstract void DisplayPlayer();
    }

    public class Muro : Casilla
    {
        public Muro ((int, int) posicion)
        {
            Posicion = posicion;
            Tipo = "Muro";
        }
        public override void Display()
        {
            AnsiConsole.Markup("⬜ ");
        }
        public override void DisplayPlayer()
        {
            AnsiConsole.Markup("🧍");
        }
    }

    public class Camino : Casilla
    {
        public Camino((int, int) posicion)
        {
            Posicion = posicion;
            Tipo = "Camino";
        }
        public override void Display()
        {
            AnsiConsole.Markup("   ");
        }
        public override void DisplayPlayer()
        {
            AnsiConsole.Markup("🧍");
        }
    }

    public class Winner : Camino
    {
        public Winner((int, int) posicion) : base(posicion)
        {
            Posicion = posicion;
            Tipo = "Winner";
        }
        public override void Display()
        {
            AnsiConsole.Markup("⭐ ");
        }
        public override void DisplayPlayer()
        {
            AnsiConsole.Markup("🧍");
        }
    }

    public abstract class Trampa : Casilla
    {   
        public Trampa((int, int) posicion)
        {
            Posicion = posicion;
            Tipo = "Trampa";
        }
        public abstract void Active(Jugador jugador, Casilla[,] maze);

        public override void Display()
        {
            AnsiConsole.Markup("💀 ");
        }
        public override void DisplayPlayer()
        {
            AnsiConsole.Markup("🧍");
        }
    }

    public class SlowTrap : Trampa
    {
        public int SpeedReduction { get; set; }

        public SlowTrap((int,int) posicion) : base(posicion)
        {
            SpeedReduction =1;
        }

        public override void Active(Jugador jugador, Casilla[,] maze)
        {
            jugador.Speed = Math.Max(jugador.Speed - SpeedReduction, 0);
            AnsiConsole.Markup($"[red] Cuidado! Caiste en una trampa y tu velocidad fue reducida en {SpeedReduction}.[/]");
        }
    }

    public class TeleportTrap : Trampa
    {
        private Random rand = new Random();

        public TeleportTrap((int,int) posicion) : base(posicion) {}

        public override void Active(Jugador jugador, Casilla[,] maze)
        {
            int x, y;
            do
            {
                x = rand.Next(1, maze.GetLength(0) - 1);
                y = rand.Next(1, maze.GetLength(1) - 1);
            }
            while (!(maze[x,y] is Camino));

            jugador.Posicion = (x,y);
            AnsiConsole.Markup($"[red] Cuidado! Caiste en una trampa y tu jugador fue teleportado a una nueva ubicacion.[/]");
        }
    }

    public class StunTrap : Trampa
    {
        public int StunDuration {get; set; }

        public StunTrap((int,int) posicion) : base(posicion)
        {
            StunDuration = 1;
        }

        public override void Active(Jugador jugador, Casilla[,] maze)
        {
            jugador.Speed = 0;
            jugador.Status = "Stunned";
            AnsiConsole.Markup($"[red] Cuidado! Caiste en una trampa y tu jugador fue stuneado por {StunDuration}.[/]");

        }
    }
}