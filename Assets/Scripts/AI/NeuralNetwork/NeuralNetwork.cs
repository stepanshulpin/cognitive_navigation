using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.NeuralNetwork {
    public class NeuralNetwork {
        public NeuralNetworkLayer[] Layers {
            get {
                return this.layers;
            }
        }

        public uint[] Topology {
            get {
                return this.topology;
            }
        }

        public int TotalWeightsCount {
            get {
                return this.weightsCount;
            }
        }

        public NeuralNetwork(uint[] topology) {
            this.topology = topology;
            this.layers = new NeuralNetworkLayer[topology.Length - 1];
            for (int layer = 0; layer < topology.Length; ++layer) {
                this.layers[layer] = new NeuralNetworkLayer(topology[layer], topology[layer + 1], (double x) => {
                    if (x > 10.0) {
                        return 1.0;
                    }
                    if (x < -10.0) {
                        return 0.0;
                    }
                    return 1.0 / (1.0 + Math.Exp(-x));
                });
            }
        }

        public double[] ProcessInput(double[] input) {
            double[] output = input;
            for (int i = 0; i < this.layers.Length; i++) {
                output = this.layers[i].ProcessInput(output);
            }
            return output;
        }

        public double[] ProcessInput(float[] input) {
            double[] output = new double[input.Length];
            return this.ProcessInput(output);
        }

        public void InitializeWeights(float[] weights) {
            int weightIndex = 0;
            for (int layer = 0; layer < this.layers.Length; ++layer) {
                for (int weight = 0; weight < this.layers[layer].Weights.Length; ++weight) {
                    this.layers[layer].Weights[weight] = weights[weightIndex];
                    weightIndex += 1;
                }
            }
        }

        public void RandomizeWeights(double min, double max) {
            for (int i = 0; i < this.layers.Length; i++) {
                this.layers[i].RandomizeWeights(min, max);
            }
        }

        private uint[] topology;

        private NeuralNetworkLayer[] layers;

        private int weightsCount;
    }
}
