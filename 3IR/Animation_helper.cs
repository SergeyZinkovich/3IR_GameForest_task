using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3IR
{
    static class Animation_helper
    {
        private const int animation_duration = 8;

        private static bool animation_set = false;

        private static List<List<Pair<int, int>>> init_position;

        private static List<List<Pair<int, int>>> destinations;

        private static int iteration;

        public static void Init_animation(List<List<Pair<int, int>>> a, List<List<Pair<int, int>>> b)
        {
            init_position = a;
            destinations = b;
            iteration = 0;
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
                    init_position[i].Add(new Pair<int, int>(0, 0));
                    destinations[i].Add(new Pair<int, int>(0, 0));
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
            animation_set = true;
        }

        public static Pair<int, int> Get_swap_coordinates(int i, int j)
        {
            if (!animation_set)
            {
                return new Pair<int, int>(0, 0);
            }
            Pair<int, int> ans = new Pair<int, int>(0, 0);
            ans.first = (destinations[i][j].first - init_position[i][j].first) * 64 / animation_duration * iteration;
            ans.second = (destinations[i][j].second - init_position[i][j].second) * 64 / animation_duration * iteration;
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
