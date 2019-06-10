using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3IR
{
    static class Animation_helper
    {
        private const int animation_duration = 4;

        private static bool animation_set = false;

        private static List<List<Pair<int, int>>> init_position;

        private static List<List<Pair<int, int>>> destinations;

        private static int iteration;

        public static void Init_gems_fall_animation(List<List<Pair<int, int>>> a)
        {
            init_position = a;
            destinations = new List<List<Pair<int, int>>>();
            for (int i = 0; i < 8; i++)  //TODO: вынести отовсюду размер поля
            {
                destinations.Add(new List<Pair<int, int>>());
                for (int j = 0; j < 8; j++)
                {
                    destinations[i].Add(new Pair<int, int>(i, j));
                }
            }
            iteration = 0;
        }

        public static void Add_missing_items_to_fall_animation(List<List<int>> a)
        {
            for (int i = 0; i < 8; i++) //TODO: вынести отовсюду размер поля
            {
                int k = 0;
                for (int j = 8 - 1; j >= 0; j--)
                {
                    if (a[j][i] == -1)
                    {
                        k--;
                        init_position[j][i].first = k;
                    }
                }
            }
        }

        public static void Init_swap_animation(int y1, int x1, int y2, int x2)
        {
            init_position = new List<List<Pair<int, int>>>();
            destinations = new List<List<Pair<int, int>>>();
            for (int i = 0; i < 8; i++)  //TODO: вынести отовсюду размер поля
            {
                init_position.Add(new List<Pair<int, int>>());
                destinations.Add(new List<Pair<int, int>>());
                for (int j = 0; j < 8; j++)
                {
                    init_position[i].Add(new Pair<int, int>(i, j));
                    destinations[i].Add(new Pair<int, int>(i, j));
                }
            }
            init_position[y1][x1].first = y1;
            init_position[y1][x1].second = x1;
            init_position[y2][x2].first = y2;
            init_position[y2][x2].second = x2;
            destinations[y1][x1].first = y2;
            destinations[y1][x1].second = x2;
            destinations[y2][x2].first = y1;
            destinations[y2][x2].second = x1;
            iteration = 0;
        }

        public static void Init_revers_swap_animation(int y1, int x1, int y2, int x2)
        {
            init_position = new List<List<Pair<int, int>>>();
            destinations = new List<List<Pair<int, int>>>();
            for (int i = 0; i < 8; i++)  //TODO: вынести отовсюду размер поля
            {
                init_position.Add(new List<Pair<int, int>>());
                destinations.Add(new List<Pair<int, int>>());
                for (int j = 0; j < 8; j++)
                {
                    init_position[i].Add(new Pair<int, int>(i, j));
                    destinations[i].Add(new Pair<int, int>(i, j));
                }
            }
            init_position[y1][x1].first = y2;
            init_position[y1][x1].second = x2;
            init_position[y2][x2].first = y1;
            init_position[y2][x2].second = x1;
            destinations[y1][x1].first = y1;
            destinations[y1][x1].second = x1;
            destinations[y2][x2].first = y2;
            destinations[y2][x2].second = x2;
            iteration = 0;
        }

        public static void Start_animation()
        {
            animation_set = true;
        }

        public static Pair<int, int> Get_coordinates(int i, int j)
        {
            if (!animation_set)
            {
                return new Pair<int, int>(64 * i, 64 * j);
            }
            Pair<int, int> ans = new Pair<int, int>(0, 0);
            ans.first = 64 * init_position[i][j].first + //TODO:вынести размер 
                (destinations[i][j].first - init_position[i][j].first) * 64 / animation_duration * iteration;
            ans.second = 64 * init_position[i][j].second +
                (destinations[i][j].second - init_position[i][j].second) * 64 / animation_duration * iteration;
            return ans;
        }

        public static void Iteration_inc()
        {
            if (animation_set)
            {
                iteration++;
                if (iteration > animation_duration)
                {
                    Stop_animation();
                }
            }
        }

        public static void Stop_animation()
        {
            if (animation_set)
            {
                init_position.Clear();
                destinations.Clear();
                iteration = 0;
                animation_set = false;
            }
        }

        public static bool is_Animation_set()
        {
            return animation_set;
        }
    }
}
