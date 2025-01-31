using Spectre.Console;

namespace MazeRunners
{
    public class Jugador
    {
        public (int x,int y) Posicion;
        public int Speed {get; set;}
        public string Name {get; set;}
        public string Status {get; set;}
        public bool IsPlaying {get; set;}
        public Habilidad Habilidad {get; set;}

        public Jugador((int,int) salida, int speed, string name, bool isPlaying, Habilidad habilidad)
        {
            Speed = speed;
            Name = name;
            Posicion = salida;
            Status = "Normal";
            IsPlaying = isPlaying;
            Habilidad = habilidad;
        }

        public void Mover((int x,int y) dir, Casilla[,] maze)
        {
            if (Status == "Stunned")
            {
                AnsiConsole.Markup("[red]No te puedes mover.[/]");
                Status = "Normal";
                return;
            }

            (int newX,int newY) = (Posicion.x + dir.x, Posicion.y + dir.y);
                
            while (IsBlock(newX, newY, maze) && maze[newX,newY] is Camino)
            {
                Posicion = (newX,newY);
                HayTrampa(maze);
                break;
            } 
        }

        private void HayTrampa(Casilla[,] maze)
        {
            if (maze[Posicion.x,Posicion.y] is Trampa Trampa)
            {
                Trampa.Active(this, maze);
                maze[Posicion.x,Posicion.y] = new Camino(Posicion);
            }
        }

        public void Display(Casilla[,] maze, (int,int) Posicion)
        {
            AnsiConsole.Markup("🧍 ");
        }

        // public void Display(bool currentPlayer)
        // {
        //     AnsiConsole.Markup(currentPlayer ? "[blue] 🧍 [/]" : "[cyan] 💤 [/]");
        // }

        private bool IsBlock(int x,int y, Casilla[,] maze) => x >= 0 && y >= 0 && x < maze.GetLength(0) && y < maze.GetLength(1);
    }
}