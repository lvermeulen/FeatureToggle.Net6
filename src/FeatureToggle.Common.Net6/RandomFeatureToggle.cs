using System;

namespace FeatureToggle
{
    public class RandomFeatureToggle : IFeatureToggle
    {        
        public bool FeatureEnabled => RandomGenerator.Next() % 2 == 0;


        // Based on: http://blogs.msdn.com/b/pfxteam/archive/2009/02/19/9434171.aspx
        private static class RandomGenerator
        {
            private static readonly Random NonThreadLocalInstance = new Random();

            [ThreadStatic]
            private static Random s_threadLocalInstance;

            public static int Next()
            {
                var rnd = s_threadLocalInstance;

                if (rnd != null)
                {
                    return rnd.Next();
                }

                int seed;

                lock (NonThreadLocalInstance)
                {
                    seed = NonThreadLocalInstance.Next();
                }

                s_threadLocalInstance = rnd = new Random(seed);

                return rnd.Next();
            }
        }
    }
}