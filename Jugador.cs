using Spectre.Console;

namespace MazeRunners
{
    public class Jugador
    {
        public (int x,int y) Posicion;
        public int Speed {get; set;}
        public string Name {get; set;}
        public string Status {get; set;}
        public int Cooldown {get; set;} = 3;
        public Habilidad Habilidad {get; set;}

        public Jugador((int,int) salida, int speed, string name, Habilidad habilidad)
        {
            Speed = speed;
            Name = name;
            Posicion = salida;
            Status = "Normal";
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
                
            while (IsBlock(newX, newY, maze) && !(maze[newX,newY] is Muro))
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

        private bool IsBlock(int x,int y, Casilla[,] maze) => x >= 0 && y >= 0 && x < maze.GetLength(0) && y < maze.GetLength(1);
    }
}