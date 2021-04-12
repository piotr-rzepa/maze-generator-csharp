using Priority_Queue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maze_generator
{
    delegate void pathfinding_algorithm();
    public partial class Form1 : Form
    {
        Graphics gph;
        List<Node> grid = new List<Node>(); //Lista przechowujaca komorki
        int block_width = 80;   //rozmiar pojedynczej komorki gridu(siatki)
        Node current_node;
        Stack<Node> stack = new Stack<Node>(); // Stos do przechowywania blokow podczas backtrackingu
        int cols;
        int rows;
        Random rdm = new Random(); //Tylko raz inicjujemy i wysylamy jako parametr, poniewaz dzieki temu unikniemy powtarzajaych sie rezultatow (dlugie korytarze)
        int label_counter = 0;
        List<Node> path = new List<Node>(); //Optymalna sciezka wyznaczona przez algorytm A*
        int timer2_counter = 0; //Do wyswietlania sciezki 
        int tickBar_counter = 500; //Do manipulowania szybkoscia wyswietlania sie animacji labiryntu oraz algorytmow
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            draw_maze();
        }
        /**********************************************************************SETUP LABIRYNTU**********************************************************************/

        //Przygotowanie labiryntu (Animacja lub stala generacja)
        public void draw_maze()
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gph = Graphics.FromImage(pictureBox1.Image);

            cols = pictureBox1.Width / block_width; 
            rows = pictureBox1.Height / block_width;
            

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    //Dodajemy kazdy z blokow do naszej list(grid)
                    Node block = new Node(j, i);
                    grid.Add(block);
                }
            }
            //Wybieramy miejsce startowe i ustawiamy je jako odwiedzone
            current_node = grid.ElementAt(0);
            current_node.set_visited = true;
        }
        private void btnAnimacja_Click(object sender, EventArgs e)
        {
            clear_maze();
            timer1.Interval = tickBar_counter;
            timer1.Start();
        }

        //TWORZENIE LABIRYNTU -> ANIMACJA
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (label_counter)
            {
                case 0:
                    label1.Text = "Tworzenie labiryntu.";
                    label_counter++;
                    break;
                case 1:
                    label1.Text = "Tworzenie labiryntu..";
                    label_counter++;
                    break;
                case 2:
                    label1.Text = "Tworzenie labiryntu...";
                    label_counter = 0;
                    break;
                default: break;

            }
            timer1.Interval = tickBar_counter;

            //Sprawdzamy, czy aktualna komorka posiada jakichs sasiadow
            Node next_cell = current_node.check_neighb(ref grid, cols, rows, rdm);

            //Jezeli komorka nie ma sasiadow oraz stos jest pusty -> konczymy
            if (next_cell == null && stack.Count == 0) { label1.Text = "Ukonczono tworzenie labiryntu"; timer1.Stop(); }

            current_node.node_highlight(gph, block_width, Color.FromArgb(128, 0, 0, 255));    //Podswietlamy(niebieski) aktualna komorke dla lepszego podgladu animacji
            pictureBox1.Refresh();
            current_node.draw(gph, block_width, Color.Black);             //Rysujemy aktualna komorke

            //Linia odpowiedzialna za znikniecie trackera komorki(niebieski blok) po skonczeniu labiryntu
            if (current_node == grid.ElementAt(0)) { current_node.draw(gph, block_width,Color.Black); pictureBox1.Refresh(); } 
            if (next_cell != null)
                {
                    next_cell.set_visited = true;                      //Oznaczamy sasiednia komorke jako odwiedzona
                 
                    stack.Push(current_node);                          //Dodajemy biezaca komorke na stos     

                    remove_walls(current_node, next_cell);             //Usuwamy sciany pomiedzy biezaca oraz wybranym sasiadem

                    current_node = next_cell;                          //Sasiednia komorka staje sie biezaca
            }
                else if (stack.Count > 0) current_node = stack.Pop();  //Jezeli nie ma sasiadow, zdejmujemy komorke ze stosu i staje sie ona biezaca
        }

        //TWORZENIE LABIRYNTU -> STALA GENERACJA
        private void btnConstant_Click(object sender, EventArgs e)
        {
            clear_maze(); //Czysci i ponownie inicjalizuje nasz grid, dzieki czemu otrzymujemy inny labirynt

            //Petla w ktorej odwiedzamy bloki, az nie trafimy na brak sasiadow
            while (current_node != null)
            {
                Node next_cell = current_node.check_neighb(ref grid, cols, rows, rdm);
                if (next_cell != null)
                {

                    next_cell.set_visited = true;         

                    stack.Push(current_node);                                

                    remove_walls(current_node, next_cell);  

                    current_node = next_cell;              
                }
                else if (stack.Count > 0)
                {
                    Node temp = stack.Pop();               
                    current_node = temp;
                }
                else break;
            }

            for(int i = 0; i < grid.Count; ++i)
            {
                grid.ElementAt(i).draw(gph, block_width,Color.Black); //Rysujemy kazdy z blokow

            }
            pictureBox1.Refresh();
            label1.Text = "Ukonczono tworzenie labiryntu";
        }

        /**********************************************************************ALGORYTMY SCIEZKI**********************************************************************/

        public void dijkstra()
        {
            //Pocztek i koniec
            Node start = grid[0];
            Node end = grid[(cols - 1) + ((rows - 1) * cols)];

            //Tablica ktora sprawdza, czy wierzcholek zostal odwiwedzony
            bool[] visited = Enumerable.Repeat(false, grid.Count).ToArray();

            //Tablica zwierajaca odleglosc do kazdego z wierzcholkow
            //Na poczatku wszystkie maja odleglosc = inf
            double[] distance = Enumerable.Repeat(double.PositiveInfinity, grid.Count).ToArray();

            //Kolejka priorytetowa przechowujaca nieodwiedzone wierzcholki
            SimplePriorityQueue<Node> unvisited = new SimplePriorityQueue<Node>();

            //Tablica indeksow danego wierzcholka
            Node[] nodeindex = new Node[grid.Count];
            for (int i = 0; i < nodeindex.Length; ++i) nodeindex[i] = null;

            //Tablica wierzcholkow dzieki ktorej odtworzona zostanie sciezka 
            Node[] previous = new Node[grid.Count];
            for (int i = 0; i < previous.Length; ++i) previous[i] = null;

            distance[0] = 0;
            nodeindex[0] = start;
            unvisited.Enqueue(start, 0);

            int count = 0;

            while (unvisited.Count > 0)
            {
                ++count;
                Node temp = unvisited.Dequeue();
                int tempos_index = temp.X + (temp.Y * cols);
                if (distance[tempos_index] == double.PositiveInfinity) break;
                if (temp.X == end.X && temp.Y == end.Y)
                {
                    break;
                }
                for (int i = 0; i < temp.neighbors.Count; ++i)
                {
                    if (valid_neighboor(temp, temp.neighbors[i]))
                    {
                        int vposindex = temp.neighbors[i].X + (temp.neighbors[i].Y * cols);
                        if (!visited[vposindex])
                        {
                            double d = Math.Abs(temp.X - temp.neighbors[i].X) + Math.Abs(temp.Y - temp.neighbors[i].Y);
                            double newdistance = distance[tempos_index] + d;
                            if (newdistance < distance[vposindex])
                            {
                                Node vnode = nodeindex[vposindex];
                                if (vnode == null)
                                {
                                    vnode = temp.neighbors[i];
                                    unvisited.Enqueue(vnode, (float)newdistance);
                                    nodeindex[vposindex] = vnode;
                                    distance[vposindex] = newdistance;
                                    previous[vposindex] = temp;
                                }
                                else
                                {
                                    vnode = temp.neighbors[i];
                                    unvisited.UpdatePriority(vnode, (float)newdistance);
                                    distance[vposindex] = newdistance;
                                    vnode.parent = temp;
                                    previous[vposindex] = temp;
                                }
                            }

                        }
                    }
                }
                visited[tempos_index] = true;
            }
            Node curr = end;
            //Odtwarzamy znaleziona sciezke, kolorujac poszczegolne bloki
            while (curr != null)
            {
                path.Add(curr);
                curr = previous[curr.X + curr.Y * cols];
            }
            path.Reverse();
        }
        public void A_star()
        {
            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>();
            Node start = grid[0];
            Node end = grid[(cols - 1) + ((rows - 1) * cols)];  //Ze wzoru i + (j * kolumny)
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                int lowest_fid = 0;
                //for (int i = 0; i < openSet.Count; ++i) openSet.ElementAt(i).node_highlight(gph, block_width, Color.FromArgb(128, 0, 255, 0));
                //for (int i = 0; i < closedSet.Count; ++i) closedSet.ElementAt(i).node_highlight(gph, block_width, Color.FromArgb(128, 255, 0 , 0));
                pictureBox1.Refresh();

                //Szukamy wierzcholka z najnizsza wartoscia f
                //Na starcie przyjmujemy, ze najnizsza wartosc f ma wierzcholek o indeksie 0
                for (int i = 0; i < openSet.Count; ++i)
                {
                    if (openSet.ElementAt(i).f < openSet.ElementAt(lowest_fid).f)
                        lowest_fid = i;
                }

                Node current_Astar = openSet.ElementAt(lowest_fid);
                if (current_Astar == end) break;   //Jezeli aktualny wierzcholek = koncowi, konczymy algorytm

                openSet.Remove(current_Astar);
                closedSet.Add(current_Astar); //Usuwamy obliczony wierzcholek i dodajemy go do listy wierzcholkow obliczonych

                //Petla, w ktorej odwiedzamy kazdego z sasiadow aktualnego wierzcholka
                for (int i = 0; i < current_Astar.neighbors.Count; ++i)
                {
                    Node neighboor = current_Astar.neighbors.ElementAt(i);
                    if (!closedSet.Contains(neighboor) && valid_neighboor(current_Astar, neighboor)) //Jezeli sasiad nie znajduje sie w liscie obliczonych wierzcholkow
                    {
                        double tempG_value = current_Astar.g + 1; //Do tymczasowej wartosci G dodajemy g aktualnego wierzcholka + koszt przejscia do sasiada

                        bool newPath = false;
                        if (openSet.Contains(neighboor))
                        {
                            if (tempG_value < neighboor.g)
                            {
                                neighboor.g = tempG_value; //Jezeli wartosc G jest nizsza od G sasiada, to nadajemu mu taka wartosc
                                newPath = true;
                            }
                        }
                        else
                        {
                            neighboor.g = tempG_value; //W przeciwnym wypadku rowniez nadajemy wartosc oraz dodajemy go do listy nieobliczonych wierzcholkow
                            openSet.Add(neighboor);
                            newPath = true;
                        }

                        if (newPath)
                        {
                            neighboor.h = heuristic(neighboor, end);
                            neighboor.f = neighboor.g + neighboor.h;
                            neighboor.parent = current_Astar; //Zapamietujemy rodzica aktualnego wierzcholka, w celu odtworzenia sciezki
                        }

                    }
                }

            }
            Node temp = end;
            path.Add(temp);
            //Odtwarzamy znaleziona sciezke, kolorujac poszczegolne bloki
            while (temp.parent != null)
            {
                path.Add(temp.parent);
                temp = temp.parent;
            }
            path.Reverse(); //Poniewaz sciezka jest tworzona od konca
            /*
            for (int i = 0; i < path.Count; ++i)
                path.ElementAt(i).node_highlight(gph, block_width, Color.FromArgb(128, 0, 0, 255));
            pictureBox1.Refresh();
            */
        }
        public void dfs()
        {
            Node start = grid[0];
            Node end = grid[(cols - 1) + ((rows - 1) * cols)]; 

            //Stos do przechowywania wierzcholkow
            Stack<Node> st = new Stack<Node>();

            bool[] visited = Enumerable.Repeat(false, grid.Count).ToArray();
            Node[] previous = new Node[grid.Count];
            for (int i = 0; i < previous.Length; ++i) previous[i] = null;

            st.Push(start);

            //Dopoki stos nie jest pusty
            while(st.Count > 0)
            {
                //Sciagamy i usuwamy komorke z wierzchu stosu
                Node current = st.Pop();

                //Jezeli komorka rowna sie koncowi, konczymy dzialanie algorytmu
                if (current == end) break;

                //Oznaczamy aktualny wierzcholek jako odwiedzony
                visited[current.X + (current.Y * cols)] = true;

                //Dla kazdego z sasiadow current
                for(int i = 0; i < current.neighbors.Count; ++i)
                {
                    //Jezeli nie ma sciany miedzy wierzcholkami i sasiad nie byl odwiedzony
                    if (valid_neighboor(current, current.neighbors[i]))
                    {
                        int neigh_pos = current.neighbors[i].X + (current.neighbors[i].Y * cols);
                        if (!visited[neigh_pos])
                        {
                            st.Push(current.neighbors[i]); //Dodajemy sasiada do stosu
                            previous[neigh_pos] = current; //Oznaczamy jego rodzica jako current
                        }
                    }
                }
            }  
            Node curr = end;
            //Odtwarzamy znaleziona sciezke, kolorujac poszczegolne bloki
            while (curr != null)
            {
                path.Add(curr);
                curr = previous[curr.X + (curr.Y * cols)];
            }
            path.Reverse();
        }

        public void bfs()
        {
            //Pocztek i koniec
            Node start = grid[0];
            Node end = grid[(cols - 1) + ((rows - 1) * cols)];

            //Kolejka FIFO do wierzcholkow nieodwiedzonych
            Queue<Node> unvisited = new Queue<Node>();
            bool[] visited = Enumerable.Repeat(false, grid.Count).ToArray();
            Node[] previous = new Node[grid.Count];
            for (int i = 0; i < previous.Length; ++i) previous[i] = null;

            visited[start.X + start.Y * cols] = true;
            unvisited.Enqueue(start);

            while(unvisited.Count > 0)
            {
                //Zdejmujemy pierwszy element dodany do kolejki
                Node current = unvisited.Dequeue();
                if (current == end) break;

                //Przeszukiwanie odbywa sie odwrotnie niz dfs -> przeszukujemy wierzcholki na danej "glebokosci", przed przejsciem do nastepnego poziomu
                for (int i = 0; i < current.neighbors.Count; ++i)
                {
                    if (valid_neighboor(current, current.neighbors[i]))
                    {
                        int index = current.neighbors[i].X + current.neighbors[i].Y * cols;
                        if (!visited[index])
                        {
                            visited[index] = true;
                            previous[index] = current;
                            unvisited.Enqueue(current.neighbors[i]);
                        }
                    }
                }
            }

            Node curr = end;
            //Odtwarzamy znaleziona sciezke, kolorujac poszczegolne bloki
            while (curr != null)
            {
                path.Add(curr);
                curr = previous[curr.X + (curr.Y * cols)];
            }
            path.Reverse();
        }

        //WYWOLYWANIE POSZCZEGOLNEGO ALGORYTMU
        private void button1_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            switch (index)
            {
                case 0:
                    path.Clear();
                    pathfinding_algorithm dij = new pathfinding_algorithm(dijkstra);
                    timer2_counter = 0;
                    dij();
                    timerDijkstra.Interval = tickBar_counter;
                    timerDijkstra.Start();
                    break;
                case 1:
                    path.Clear();
                    pathfinding_algorithm Astar = new pathfinding_algorithm(A_star);
                    timer2_counter = 0;
                    Astar();
                    timer2.Interval = tickBar_counter;
                    timer2.Start();
                    break;
                case 2:
                    pathfinding_algorithm deepfs = new pathfinding_algorithm(dfs);
                    path.Clear();
                    timer2_counter = 0;
                    deepfs();
                    timerDFS.Interval = tickBar_counter;
                    timerDFS.Start();
                    break;
                case 3:
                    pathfinding_algorithm breadthfs = new pathfinding_algorithm(bfs);
                    path.Clear();
                    timer2_counter = 0;
                    breadthfs();
                    timerBFS.Interval = tickBar_counter;
                    timerBFS.Start();
                    break;
            }
        }

        //TIMER A*
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = tickBar_counter;
            if (timer2_counter < path.Count)
            {
                path.ElementAt(timer2_counter).node_highlight(gph, block_width, Color.FromArgb(128, 0, 255, 255));
                timer2_counter++;
                pictureBox1.Refresh();
            }
            else timer2.Stop();
        }
        //TIMER DIJKSTRA
        private void timerDijkstra_Tick(object sender, EventArgs e)
        {
            timerDijkstra.Interval = tickBar_counter;
            if (timer2_counter < path.Count)
            {
                path.ElementAt(timer2_counter).node_highlight(gph, block_width, Color.FromArgb(128, 255, 0, 255));
                timer2_counter++;
                pictureBox1.Refresh();
            }
            else timerDijkstra.Stop();

        }

        //TIMER DFS
        private void timerDFS_Tick(object sender, EventArgs e)
        {
            timerDFS.Interval = tickBar_counter;
            if (timer2_counter < path.Count)
            {
                path.ElementAt(timer2_counter).node_highlight(gph, block_width, Color.FromArgb(64, 255, 255, 0));
                timer2_counter++;
                pictureBox1.Refresh();
            }
            else timerDFS.Stop();
        }

        //TIMER BFS
        private void timerBFS_Tick(object sender, EventArgs e)
        {
            timerBFS.Interval = tickBar_counter;
            if (timer2_counter < path.Count)
            {
                path.ElementAt(timer2_counter).node_highlight(gph, block_width, Color.FromArgb(128, 0, 0, 0));
                timer2_counter++;
                pictureBox1.Refresh();
            }
            else timerBFS.Stop();
        }

        /**********************************************************************FUNKCJE POMOCNICZE**********************************************************************/

        //Funkcja sluzaca do sprawdzenia czy istnieje przejscie do sasiada(brak sciany)
        public bool valid_neighboor(Node current, Node neighboor)
        {
            bool answ = false;
            int x = current.X - neighboor.X;
            int y = current.Y - neighboor.Y;

            switch (x)
            {
                case 0: break;
                case 1:
                    if (!current.walls[2] && !neighboor.walls[3]) answ = true;
                    break;
                case -1:
                    if (!current.walls[3] && !neighboor.walls[2]) answ = true;
                    break;
            }
            switch (y)
            {
                case 0: break;
                case 1:
                    if (!current.walls[0] && !neighboor.walls[1]) answ = true;
                    break;
                case -1:
                    if (!current.walls[1] && !neighboor.walls[0]) answ = true;
                    break;
            }
            return answ;
        }

        //Usuwanie scian miedzy sasiadujacymi wierzcholkami
        public void remove_walls(Node current_node, Node next_node)
        {
            //Obliczamy roznice indeksow aktualnej oraz kolejnej komorki
            int x = current_node.X - next_node.X;
            //Na podstawie roznicy likwidujemy odpowiednie sciany w sasiednich komorkach
            switch (x)
            {
                case 1:
                    current_node.delete_wall(2);
                    next_node.delete_wall(3);
                    break;
                case -1:
                    current_node.delete_wall(3);
                    next_node.delete_wall(2);
                    break;
                default: break;
            }
            int y = current_node.Y - next_node.Y;
            switch (y)
            {
                case 1:
                    current_node.delete_wall(0);
                    next_node.delete_wall(1);
                    break;
                case -1:
                    current_node.delete_wall(1);
                    next_node.delete_wall(0);
                    break;
                default: break;
            }
        }

        //CZYSZCZENIE STOSU, POSZCZEGOLNYCH LIST
        public void clear_maze()
        {
            if (grid.Count > 0)
            {
                grid.Clear();
                label1.Text = string.Empty;
                draw_maze();
            }
            if (path.Count > 0)
                path.Clear();
        }

        //OBLICZANIE ODLEGLOSCI DO KONCOWEGO WIERZCHOLKA
        public double heuristic(Node neighboor, Node end)
        {
            //Dystans liczony za pomoca metody Manhattan
            return Math.Abs(neighboor.X - end.X) + Math.Abs(neighboor.Y - end.Y);
            //odleglosc euklidesowa
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tickBar_counter = trackBar1.Value;
            labelPredkosc.Text = "Szybkość animacji: " + trackBar1.Value.ToString() + " ms";
        }

        private void trackBarMaze_Scroll(object sender, EventArgs e)
        {
            block_width = trackBarMaze.Value;
            labelRozmiar.Text = "Rozmiar labiryntu: " + (800 / trackBarMaze.Value).ToString() + "x" + (800 / trackBarMaze.Value).ToString() + " bloków";
        }
    }
}
