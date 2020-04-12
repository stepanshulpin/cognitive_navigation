using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.NeuralNetwork {
    public delegate double ActivationFunction(double x);

    public class NeuralNetworkLayer {
        public uint InputSize {
            get {
                return this.neuronsCount;
            }
        }

        public uint OutputSize {
            get {
                return this.outputSize;
            }
        }

        public double[] Weights {
            get {
                return this.weights;
            }
        }

        public NeuralNetworkLayer(uint neuronsCount, uint outputSize, ActivationFunction activation) {
            this.neuronsCount = neuronsCount;
            this.outputSize = outputSize;
            this.weights = new double[neuronsCount * outputSize];
        }

        public double[] ProcessInput(double[] input) {
            double[] output = new double[outputSize];
            int currentWeightIndex = 0;
            for (int j = 0; j < this.outputSize; ++j) {
                for (int i = 0; i < this.neuronsCount; ++i) {
                    output[j] += input[i] * this.weights[currentWeightIndex++];
                }
            }
            if (this.activation != null) {
                for (int i = 0; i < this.outputSize; ++i) {
                    output[i] = this.activation(output[i]);
                }
            }
            return output;
        }

        public NeuralNetworkLayer Clone() {
            NeuralNetworkLayer clone = new NeuralNetworkLayer(this.neuronsCount, this.outputSize, this.activation);
            Array.Copy(this.weights, clone.weights, this.weights.Length);
            return clone;
        }

        public void RandomizeWeights(double min, double max) {
            double range = Math.Abs(max - min);
            for (int i = 0; i < this.weights.Length; ++i) {
                this.weights[i] = min + random.NextDouble() * range;
            }
        }

        private uint neuronsCount;

        private uint outputSize;

        private double[] weights;

        private ActivationFunction activation;

        private static Random random = new Random();
    }
}
