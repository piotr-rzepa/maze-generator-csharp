using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace maze_generator
{
    public class Node
    {
        private int x_coord;
        private int y_coord;
        private bool node_visited;
                                            //gora  dol  lewo  prawo
        public bool[] walls = new bool[] { true, true, true, true };

        public double f { get; set; }   //Wartosc, na podstawie ktorej algorytm A* wyybierze kolejny wierzcholek (minimalizowanie funkcji) 
        public double g { get; set; }    //Droga miedzy punktem poczatkowym a tym wierzcholkiem
        public double h { get; set; }    //Droga od tego wierzcholka, do punktu koncowego (heurystyka)

        public List<Node> neighbors = new List<Node>();

        public Node parent { get; set; }

        public int X
        {
            get => x_coord;
            set => x_coord = value;
        }
    public int Y
        {
            get => y_coord;
            set => y_coord = value;
        }
    public bool set_visited
        {
            get => node_visited;
            set => node_visited = value;
        }
    public Node(int x, int y)
        {
            this.x_coord = x;
            this.y_coord = y;
            this.node_visited = false;
            this.f = 0;
            this.g = 0;
            this.h = 0;
            this.parent = null;
        }
    //Konstruktor kopiujacy (uzywany m.in. przy sasiadach)
    public Node(Node copy)
        {
            this.x_coord = copy.x_coord;
            this.y_coord = copy.y_coord;
            this.node_visited = copy.node_visited;
            this.walls = copy.walls;
        }
    public void delete_wall(int index)
        {
            this.walls[index] = false;
        }
        public Node check_neighb(ref List<Node> grid, int cols, int rows, Random rdm)
        {
            List<Node> temp_neighb = new List<Node>();
            Node top, right, bottom, left;

            //Tablica do manipulowania sasiadami -> jezeli go nie ma poszczegolna wartosc = null (koniecznie w przypadku blokow na brzegach labiryntu)
            int?[] neighb_flag = new int?[4]; //Zamiast przechowywyac wartosc ujemna, uzylem typu mogacego przechowywac wartosc null

            //Anonimowa funkcja do zwracania indeksu
            Func<int, int, int?> index = (int i, int j) =>
            {
                if (i < 0 || j < 0 || i > cols - 1 || j > rows - 1)
                    return null;
                else return i + (j * cols);
            };

            //Obliczanie indeksow sasiadow danego bloku
            neighb_flag[0] = index(this.x_coord, this.y_coord - 1);
            neighb_flag[1] = index(this.x_coord + 1, this.y_coord);
            neighb_flag[2] = index(this.x_coord, this.y_coord + 1);
            neighb_flag[3] = index(this.x_coord - 1, this.y_coord);

            //Jezeli sasiad istnieje (wartosc flagi != null) -> sasiadowi przpisujemy numer bloku zgodny z jego indeksem w gridzie
            if (neighb_flag[0] != null) top = grid[(int)neighb_flag[0]];
            else top = null;
            if (neighb_flag[1] != null ) right = grid[(int)neighb_flag[1]];
            else right = null;
            if (neighb_flag[2] != null) bottom = grid[(int)neighb_flag[2]];
            else bottom = null;
            if (neighb_flag[3] != null) left = grid[(int)neighb_flag[3]];
            else left = null;

            //         (i, j - 1)
            //(i - 1, j)   (i,j)     (i + 1, j)          tak wyglada rozstaw oraz koordynaty wszystkich 4 sasiadow
            //         (i, j + 1)


            //Jezeli dany sasiad istnieje i nie zostal odwiedzony, dodajemy go do listy dostepnych sasiadow
            if (top != null && !top.node_visited) { temp_neighb.Add(top); neighbors.Add(top); }

            if (right != null && !right.node_visited) { temp_neighb.Add(right); neighbors.Add(right); }

            if (bottom != null && !bottom.node_visited) { temp_neighb.Add(bottom); neighbors.Add(bottom); }

            if (left != null && !left.node_visited) { temp_neighb.Add(left); neighbors.Add(left); }

            //Losujemy jednego z sasiadow, ktory zostanie odwiedzony i stanie sie blokiem wyjsciowym
            if (temp_neighb.Count() > 0)
            {
                //Random random1 = new Random();
                //int random_neighb = random1.Next() % neighb.Count;    //Losowanie sasiada w tym miejscu, skutkowaloby dlugimi korytarzami
                int random_neighb = rdm.Next() % temp_neighb.Count;
                //DEBUG
                //Console.WriteLine("Blok o wspolrzednych {0},{1} ma sasiadow: top={2}, right={3}, bottom={4}, left={5}, wybieram sasiada={6}, liczba sasiadow={7}, wylosowana liczba={8}", this.x_coord, this.y_coord, neighb_flag[0], neighb_flag[1], neighb_flag[2], neighb_flag[3], grid.IndexOf(neighb.ElementAt(random_neighb)), neighb.Count(), random_neighb);
                return temp_neighb.ElementAt(random_neighb);
            }
            else return null;
        }

    public void draw(Graphics gph, int block_width, Color col) //Funkcja rysujaca sciany wokol bloku
        {
            //Rysowanie bloku bez podzialu na poszczegolne sciany
            //gph.DrawRectangle(new Pen(Color.White), this.x_coord * block_width, this.y_coord * block_width, block_width, block_width); 
            int X = this.x_coord * block_width;
            int Y = this.y_coord * block_width;

            if (this.node_visited) //Jezeli zwiedzilismy komorke, zmieniamy kolor dla lepszego podgladu
                gph.FillRectangle(new SolidBrush(Color.White), X, Y, block_width, block_width);
            
            if (walls[0]) //GORNA SCIANA
                gph.DrawLine(new Pen(col), new Point(X, Y), new Point(X + block_width, Y)); 

            if (walls[1]) //DOLNA SCIANA
                gph.DrawLine(new Pen(col), new Point(X, Y + block_width), new Point(X + block_width, Y + block_width));

            if (walls[2]) //LEWA SCIANA
                gph.DrawLine(new Pen(col), new Point(X, Y), new Point(X, Y + block_width));

            if (walls[3]) //PRAWA SCIANA
                gph.DrawLine(new Pen(col), new Point(X + block_width, Y), new Point(X + block_width, Y+ block_width));
        }

    //Funkcja do wyrozniania wierzcholka na gorze staku podczas animacji tworzenia labiryntu
    public void node_highlight(Graphics gph, int block_width, Color color)
        {
            int X = this.x_coord * block_width;
            int Y = this.y_coord * block_width;
            gph.FillRectangle(new SolidBrush(color), X, Y, block_width, block_width);
        }

    }
}
