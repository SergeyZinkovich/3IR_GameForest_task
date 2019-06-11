using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3IR
{
    class AnimationHelper
    {
        private const int animationDuration = 8;
        private bool animationSet = false;
        private List<List<Pair<int, int>>> initPosition;
        private List<List<Pair<int, int>>> destinations;
        private int iteration;
        int height, width;

        public AnimationHelper(int n, int m)
        {
            height = m;
            width = n;
        }

        public void InitGemsFallAnimation(List<List<Pair<int, int>>> a)
        {
            initPosition = a;
            destinations = new List<List<Pair<int, int>>>();
            for (int i = 0; i < height; i++)
            {
                destinations.Add(new List<Pair<int, int>>());
                for (int j = 0; j < width; j++)
                {
                    destinations[i].Add(new Pair<int, int>(i, j));
                }
            }
            iteration = 0;
        }

        public void AddMissingItemsToFallAnimation(List<List<int>> a)
        {
            for (int i = 0; i < width; i++)
            {
                int k = 0;
                for (int j = height - 1; j >= 0; j--)
                {
                    if (a[j][i] == -1)
                    {
                        k--;
                        initPosition[j][i].first = k;
                    }
                }
            }
        }

        public void InitSwapAnimation(int y1, int x1, int y2, int x2)
        {
            initPosition = new List<List<Pair<int, int>>>();
            destinations = new List<List<Pair<int, int>>>();
            for (int i = 0; i < height; i++)
            {
                initPosition.Add(new List<Pair<int, int>>());
                destinations.Add(new List<Pair<int, int>>());
                for (int j = 0; j < width; j++)
                {
                    initPosition[i].Add(new Pair<int, int>(i, j));
                    destinations[i].Add(new Pair<int, int>(i, j));
                }
            }
            initPosition[y1][x1].first = y1;
            initPosition[y1][x1].second = x1;
            initPosition[y2][x2].first = y2;
            initPosition[y2][x2].second = x2;
            destinations[y1][x1].first = y2;
            destinations[y1][x1].second = x2;
            destinations[y2][x2].first = y1;
            destinations[y2][x2].second = x1;
            iteration = 0;
        }

        public void InitReversSwapAnimation(int y1, int x1, int y2, int x2)
        {
            initPosition = new List<List<Pair<int, int>>>();
            destinations = new List<List<Pair<int, int>>>();
            for (int i = 0; i < height; i++)
            {
                initPosition.Add(new List<Pair<int, int>>());
                destinations.Add(new List<Pair<int, int>>());
                for (int j = 0; j < width; j++)
                {
                    initPosition[i].Add(new Pair<int, int>(i, j));
                    destinations[i].Add(new Pair<int, int>(i, j));
                }
            }
            initPosition[y1][x1].first = y2;
            initPosition[y1][x1].second = x2;
            initPosition[y2][x2].first = y1;
            initPosition[y2][x2].second = x1;
            destinations[y1][x1].first = y1;
            destinations[y1][x1].second = x1;
            destinations[y2][x2].first = y2;
            destinations[y2][x2].second = x2;
            iteration = 0;
        }

        public void StartAnimation()
        {
            animationSet = true;
        }

        public Pair<int, int> GetCoordinates(int i, int j)
        {
            if (!animationSet)
            {
                return new Pair<int, int>(64 * i, 64 * j);
            }
            Pair<int, int> ans = new Pair<int, int>(0, 0);
            ans.first = 64 * initPosition[i][j].first +
                (destinations[i][j].first - initPosition[i][j].first) * 64 / animationDuration * iteration;
            ans.second = 64 * initPosition[i][j].second +
                (destinations[i][j].second - initPosition[i][j].second) * 64 / animationDuration * iteration;
            return ans;
        }

        public void IterationInc()
        {
            if (animationSet)
            {
                iteration++;
                if (iteration > animationDuration)
                {
                    StopAnimation();
                }
            }
        }

        public void StopAnimation()
        {
            if (animationSet)
            {
                initPosition.Clear();
                destinations.Clear();
                iteration = 0;
                animationSet = false;
            }
        }

        public bool IsAnimationSet()
        {
            return animationSet;
        }
    }
}
