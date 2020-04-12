using System;
using System.Collections.Generic;

namespace Utils {
    public class Random {
        public Random() {
            this.randoms = new Dictionary<Type, Delegate>();
            this.AddDefault();
            this.parametrizedRandoms = new Dictionary<Type, Delegate>();
            this.AddDefaultParametrized();
        }

        public void Add<T>(Func<T> randomizer) {
            this.randoms.Add(typeof(T), randomizer);
        }

        public void AddParametrized<T>(Func<T, T, T> randomizer) {
            this.parametrizedRandoms.Add(typeof(T), randomizer);
        }

        public T Randomize<T>() {
            return ((Func<T>)this.randoms[typeof(T)])();
        }

        public T Randomize<T>(T min, T max) {
            return ((Func<T, T, T>)this.parametrizedRandoms[typeof(T)])(min, max);
        }

        private void AddDefault() {
            this.Add<int>(() => random.Next());
            this.Add<float>(() => (float)random.NextDouble());
            this.Add<double>(() => random.NextDouble());
        }

        private void AddDefaultParametrized() {
            this.AddParametrized<int>((int min, int max) => random.Next(min, max));
            this.AddParametrized<float>((float min, float max) => (float)(random.NextDouble() * (max - min) + min));
            this.AddParametrized<double>((double min, double max) => random.NextDouble() * (max - min) + min);
        }

        private Dictionary<Type, Delegate> randoms;

        private Dictionary<Type, Delegate> parametrizedRandoms;

        private static System.Random random = new System.Random();
    }
}

