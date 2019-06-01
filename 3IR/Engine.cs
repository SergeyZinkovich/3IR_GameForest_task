using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3IR
{
    public class Engine
    {
        List<List<int>> map;
        int score = 0;

        public Engine(int n, int m)
        {
            Generate_map(n, m);
            while (Annihilate())
            {
                Regenerate_annihilated_items();
            }
            score = 0;
        }

        public List<List<int>> Get_map()
        {
            return map;
        }

        public void Generate_map(int n, int m)
        {
            Random rnd = new Random();
            map = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                map.Add(new List<int>());
                for (int j = 0; j < m; j++)
                {
                    map[i].Add(rnd.Next() % 5);
                }
            }
        }

        public void Regenerate_annihilated_items()
        {
            Random rnd = new Random();
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[0].Count; j++)
                {
                    if (map[i][j] == -1)
                    {
                        map[i][j] = rnd.Next() % 5;
                    }
                }
            }
        }

        public bool Annihilate()
        {
            List<Pair<int, int>> delete_marks = new List<Pair<int, int>>();
            bool in_seq = false;
            for (int i = 0; i < map.Count(); i++)
            {
                for (int j = 2; j < map[0].Count(); j++)
                {
                    if ((map[i][j] == map[i][j - 1]) && (map[i][j] == map[i][j - 2]))
                    {
                        if (!in_seq)
                        {
                            in_seq = true;
                            delete_marks.Add(new Pair<int, int>(i, j));
                            delete_marks.Add(new Pair<int, int>(i, j - 1));
                            delete_marks.Add(new Pair<int, int>(i, j - 2));
                        }
                        else
                        {
                            delete_marks.Add(new Pair<int, int>(i, j));
                        }
                    }
                    else
                    {
                        in_seq = false;
                    }
                }
            }

            in_seq = false;
            for (int i = 0; i < map.Count(); i++)
            {
                for (int j = 2; j < map[0].Count(); j++)
                {
                    if ((map[j][i] == map[j - 1][i]) && (map[j][i] == map[j - 2][i]))
                    {
                        if (!in_seq)
                        {
                            in_seq = true;
                            delete_marks.Add(new Pair<int, int>(j, i));
                            delete_marks.Add(new Pair<int, int>(j - 1, i));
                            delete_marks.Add(new Pair<int, int>(j - 2, i));
                        }
                        else
                        {
                            delete_marks.Add(new Pair<int, int>(i, j));
                        }
                    }
                    else
                    {
                        in_seq = false;
                    }
                }
            }

            foreach (Pair<int, int> pair in delete_marks)
            {
                map[pair.first][pair.second] = -1;
            }

            return delete_marks.Count != 0;
        }

        public void Drop_elements()
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = map[0].Count - 1; j >= 0; j--)
                {
                    if (map[j][i] != -1)
                    {
                        int k = j;
                        while ((k + 1 < map.Count) && (map[k + 1][i] == -1))
                        {
                            int c = map[k][i];
                            map[k][i] = map[k + 1][i];
                            map[k + 1][i] = c;
                            k++;
                        }
                    }
                }
            }
        }

        public bool Turn(int y1, int x1, int y2, int x2)
        {
            int c = map[y1][x1];
            map[y1][x1] = map[y2][x2];
            map[y2][x2] = c;
            if (!Annihilate())
            {
                c = map[y1][x1];
                map[y1][x1] = map[y2][x2];
                map[y2][x2] = c;
                return false;
            }
            return true;
        }

    }

}
