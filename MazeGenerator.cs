using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeRunners;
using Spectre.Console;

namespace MazeRunners
{
    public class MazeGenerator
    {
        public int ancho;
        public int altura;
        public Casilla[,] maze;
        public Random rand = new Random();
        public (int x,int y) winner;

        public MazeGenerator(int ancho, int altura)
        {
            this.ancho = ancho;
            this.altura = altura;
            maze = new Casilla[ancho, altura];

            winner = (ancho/2,altura/2);
            rand = new Random();
        }

        public Casilla[,] GenerateMaze()
        {
            for (int i = 0; i < ancho; i++)
            {
                for (int j = 0; j < altura; j++)
                {
                    maze[i,j] = new Muro((i,j));
                }
            }


            NuevoCamino(1,1);
            
            if (maze[1,altura - 3] is Muro)
            {
                maze[1,altura - 3] = new Camino((1,ancho - 3));
            }
            if (maze[ancho - 3,1] is Muro)
            {
                maze[ancho - 3,1] = new Camino((ancho - 3,1));
            }
            if (maze[ancho - 3,altura - 2] is Muro)
            {
                maze[ancho - 3,altura - 2] = new Camino((ancho - 3,altura - 2)) ;
            }
            ConectarCasilla(maze, winner);
            
            PonerTrampas();

            return maze;
        }

        public void NuevoCamino(int ancho, int altura)
    {
        maze[ancho, altura] = new Camino((ancho, altura));
        var move = new(int, int)[]
        {
            (-2, 0),
            (2, 0),
            (0, 2),
            (0, -2)
        };

        move = move.OrderBy(x => rand.Next()).ToArray();

        foreach(var (x, y) in move)
        {
            int nx = ancho + x;
            int ny = altura + y;

            if (IsBlock(nx,ny) && maze[nx,ny] is Muro)
            {
                maze[ancho + x / 2, altura + y / 2] = new Camino((ancho + x / 2, altura + y / 2));
                NuevoCamino(nx, ny); 
            }
        }
    }

        public void ConectarCasilla(Casilla[,] maze, (int x, int y) winner)
        {
            if (!(maze[winner.x,winner.y] is Camino))
            {
                maze[winner.x,winner.y] = new Camino(winner);
                return;
            }

            var dir = new List<(int, int)> {(0,1) , (1,0) , (0,-1) , (-1,0)};
            Shuffle(dir);

            foreach (var (dx, dy) in dir)
            {
                int nx = winner.x + dx;
                int ny = winner.y + dy;

                if (IsBlock(nx,ny) && maze[nx,ny] is Muro)
                {
                    maze[winner.x + dx, winner.y + dy] = new Camino((winner.x + dx/2, winner.y + dy/2));
                    break;
                }
            }
            maze[winner.x,winner.y] = new Winner(winner);
        }

        public void PonerTrampas()
        {
            int count = 10;

            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(2, ancho - 3);
                    y = rand.Next(2, altura - 3);
                } 
                while ((maze[x,y] is Camino));

                int TipoTrampa = rand.Next(3);
                switch (TipoTrampa)
                {
                    case 0:
                        maze[x,y] = new SlowTrap((x,y));
                        break;
                    case 1:
                        maze[x,y] = new TeleportTrap((x,y));
                        break;
                    case 2:
                        maze[x,y] = new StunTrap((x,y));
                        break;
                }
            }
        }

        public bool IsBlock(int x, int y) => x > 0 && x < ancho && y > 0 && y < altura;

        public void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;            
            }
        }
    }
}
