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

        public Engine(int n, int m, int gemesCount)
        {
            GenerateMap(n, m, gemesCount);
            while (Annihilate())
            {
                RegenerateAnnihilatedItems();
            }
            score = 0;
        }

        public List<List<int>> GetMap()
        {
            return map;
        }

        public int GetScore()
        {
            return score;
        }

        public void GenerateMap(int n, int m, int gemesCount)
        {
            Random rnd = new Random();
            map = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                map.Add(new List<int>());
                for (int j = 0; j < m; j++)
                {
                    map[i].Add(rnd.Next() % gemesCount);
                }
            }
        }

        public void RegenerateAnnihilatedItems()
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
            List<Pair<int, int>> deleteMarks = new List<Pair<int, int>>();
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
                            deleteMarks.Add(new Pair<int, int>(i, j));
                            deleteMarks.Add(new Pair<int, int>(i, j - 1));
                            deleteMarks.Add(new Pair<int, int>(i, j - 2));
                        }
                        else
                        {
                            deleteMarks.Add(new Pair<int, int>(i, j));
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
                            deleteMarks.Add(new Pair<int, int>(j, i));
                            deleteMarks.Add(new Pair<int, int>(j - 1, i));
                            deleteMarks.Add(new Pair<int, int>(j - 2, i));
                        }
                        else
                        {
                            deleteMarks.Add(new Pair<int, int>(j, i));
                        }
                    }
                    else
                    {
                        in_seq = false;
                    }
                }
            }
            score += 10 * deleteMarks.Count;
            foreach (Pair<int, int> pair in deleteMarks)
            {
                map[pair.first][pair.second] = -1;
            }

            return deleteMarks.Count != 0;
        }

        public List<List<Pair<int, int>>> DropElements()
        {
            List<List<Pair<int, int>>> previousPosition = new List<List<Pair<int, int>>>();
            for (int i = 0; i < map.Count; i++)
            {
                previousPosition.Add(new List<Pair<int, int>>());
                for (int j = 0; j < map[0].Count; j++)
                {
                    previousPosition[i].Add(new Pair<int, int>(i, j));
                }
            }
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
                        previousPosition[k][i].first = j;
                    }
                }
            }
            return previousPosition;
        }

        public void SwapItems(int y1, int x1, int y2, int x2)
        {
            int c = map[y1][x1];
            map[y1][x1] = map[y2][x2];
            map[y2][x2] = c;
        }

    }

}
