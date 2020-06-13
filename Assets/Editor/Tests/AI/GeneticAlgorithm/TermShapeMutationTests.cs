using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.GeneticAlgorithm;
using NUnit.Framework;
using AI.FuzzyLogic.Terms;
using UnityEngine;

namespace Tests.AI.GeneticAlgorithmTest
{
    class TermShapeMutationTests
    {

        private static double eps = 0.001;

        [Test]
        public void TrapezoidalToTriangularTest1()
        {
            Term trapezoidal = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezoidal.SetValues(new double[] { 1, 5, 7, 8 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(trapezoidal.GetGenericParameters());
            Assert.IsTrue(Math.Abs(0.15 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(5.85 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(9.15 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void TrapezoidalToTriangularTest2()
        {
            Term trapezoidal = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezoidal.SetValues(new double[] { -7, -3, 2, 5 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(trapezoidal.GetGenericParameters());
            Assert.IsTrue(Math.Abs(-9.45 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(-0.55 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(7.55 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void TriangularToTrapezoidalTest1()
        {
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.SetValues(new double[] { 3, 6, 8 });
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.Update(triangular.GetGenericParameters());
            Assert.IsTrue(Math.Abs(3.3 - trapezodial.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(5.7 - trapezodial.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(6.2 - trapezodial.GetValues()[2]) < eps);
            Assert.IsTrue(Math.Abs(7.8 - trapezodial.GetValues()[3]) < eps);
        }

        [Test]
        public void TriangularToTrapezoidalTest2()
        {
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.SetValues(new double[] { -4, 3, 5 });
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.Update(triangular.GetGenericParameters());
            Assert.IsTrue(Math.Abs(-3.3 - trapezodial.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(2.3 - trapezodial.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(3.2 - trapezodial.GetValues()[2]) < eps);
            Assert.IsTrue(Math.Abs(4.8 - trapezodial.GetValues()[3]) < eps);
        }

        [Test]
        public void TriangularToGaussTest1()
        {
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.SetValues(new double[] { -4, 3, 5 });
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.Update(triangular.GetGenericParameters());
            Assert.IsTrue(Math.Abs(1.062 - gaussian.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(2.75 - gaussian.GetValues()[1]) < eps);
        }

        [Test]
        public void TriangularToGaussTest2()
        {
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.SetValues(new double[] { 3, 6, 8 });
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.Update(triangular.GetGenericParameters());
            Assert.IsTrue(Math.Abs(0.8918 - gaussian.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(5.9500 - gaussian.GetValues()[1]) < eps);
        }

        [Test]
        public void GaussToTriangularTest1()
        {
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.SetValues(new double[] { 2, 5 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(gaussian.GetGenericParameters());
            Assert.IsTrue(Math.Abs(0.2904 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(5 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(9.71 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void GaussToTriangularTest2()
        {
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.SetValues(new double[] { -2, 4 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(gaussian.GetGenericParameters());
            Assert.IsTrue(Math.Abs(-0.7096 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(4 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(8.71 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void GaussToTrapezoidalTest1()
        {
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.SetValues(new double[] { 2, 5 });
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.Update(gaussian.GetGenericParameters());
            Assert.IsTrue(Math.Abs(1.208 - trapezodial.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(4.082 - trapezodial.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(5.918 - trapezodial.GetValues()[2]) < eps);
            Assert.IsTrue(Math.Abs(8.792 - trapezodial.GetValues()[3]) < eps);
        }

        [Test]
        public void TrapezoidalToGaussTest1()
        {
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.SetValues(new double[] { 1, 5, 7, 8 });
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.Update(trapezodial.GetGenericParameters());
            Assert.IsTrue(Math.Abs(1.401 - gaussian.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(5.85 - gaussian.GetValues()[1]) < eps);
        }

        [Test]
        public void SigmoidalToTriangularTest1()
        {
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.SetValues(new double[] { 2, 4 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(sigmoidal.GetGenericParameters());
            Assert.IsTrue(Math.Abs(2.352 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(5.648 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(11.14 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void SigmoidalToGaussTest1()
        {
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.SetValues(new double[] { 2, 4 });
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.Update(sigmoidal.GetGenericParameters());
            Assert.IsTrue(Math.Abs(2.333 - gaussian.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(5.648 - gaussian.GetValues()[1]) < eps);
        }

        [Test]
        public void TrapezoidalToSigmoidalTest1()
        {
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.SetValues(new double[] { 1, 5, 7, 8 });
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.Update(trapezodial.GetGenericParameters());
            Assert.IsTrue(Math.Abs(1.373 - sigmoidal.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - sigmoidal.GetValues()[1]) < eps);
        }

        [Test]
        public void TriangularToSigmoidalTest1()
        {
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.SetValues(new double[] { -8, 0, 5 });
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.Update(triangular.GetGenericParameters());
            Assert.IsTrue(Math.Abs(0.6866 - sigmoidal.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(-4 - sigmoidal.GetValues()[1]) < eps);
        }

        [Test]
        public void TriangularToBellTest1()
        {
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.SetValues(new double[] { -8, 0, 5 });
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.Update(triangular.GetGenericParameters());
            Assert.IsTrue(Math.Abs(3.85 - bell.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(2.406 - bell.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(-0.15 - bell.GetValues()[2]) < eps);
        }

        [Test]
        public void SigmoidalToBellTest1()
        {
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.SetValues(new double[] { -2, 4 });
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.Update(sigmoidal.GetGenericParameters());
            Assert.IsTrue(Math.Abs(1.099 - bell.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(2 - bell.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(2.901 - bell.GetValues()[2]) < eps);
        }

        [Test]
        public void BellToTriangularTest1()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 2, 4, 6 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(bell.GetGenericParameters());
            Assert.IsTrue(Math.Abs(2 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(6 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(10 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void BellToGaussTest1()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 2, 4, 6 });
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.Update(bell.GetGenericParameters());
            Assert.IsTrue(Math.Abs(1.699 - gaussian.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(6 - gaussian.GetValues()[1]) < eps);
        }

        [Test]
        public void BellToSShapeTest1()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 2, 4, 6 });
            Term sshape = TermHelper.instantiate(TermType.SShape, "test");
            sshape.Update(bell.GetGenericParameters());
            Assert.IsTrue(Math.Abs(3.52 - sshape.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(4.48 - sshape.GetValues()[1]) < eps);
        }

        [Test]
        public void GaussToSShapeTest1()
        {
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.SetValues(new double[] { 2, 5 });
            Term sshape = TermHelper.instantiate(TermType.SShape, "test");
            sshape.Update(gaussian.GetGenericParameters());
            Assert.IsTrue(Math.Abs(1.208 - sshape.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(4.082 - sshape.GetValues()[1]) < eps);
        }

        [Test]
        public void SShapeToTriangularTest1()
        {
            Term sshape = TermHelper.instantiate(TermType.SShape, "test");
            sshape.SetValues(new double[] { 1, 8 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(sshape.GetGenericParameters());
            Assert.IsTrue(Math.Abs(-5.65 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(7.65 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(22.35 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void SShapeToSigmoidalTest1()
        {
            Term sshape = TermHelper.instantiate(TermType.SShape, "test");
            sshape.SetValues(new double[] { 1, 8 });
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.Update(sshape.GetGenericParameters());
            Assert.IsTrue(Math.Abs(0.3488 - sigmoidal.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(1 - sigmoidal.GetValues()[1]) < eps);
        }

        [Test]
        public void BellToZShapeTest1()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 2, 4, 6 });
            Term zshape = TermHelper.instantiate(TermType.ZShape, "test");
            zshape.Update(bell.GetGenericParameters());
            Assert.IsTrue(Math.Abs(7.52 - zshape.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(8.48 - zshape.GetValues()[1]) < eps);
        }

        [Test]
        public void GaussToZShapeTest1()
        {
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.SetValues(new double[] { 2, 5 });
            Term zshape = TermHelper.instantiate(TermType.ZShape, "test");
            zshape.Update(gaussian.GetGenericParameters());
            Assert.IsTrue(Math.Abs(5.918 - zshape.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(8.792 - zshape.GetValues()[1]) < eps);
        }

        [Test]
        public void ZShapeToTriangularTest1()
        {
            Term zshape = TermHelper.instantiate(TermType.ZShape, "test");
            zshape.SetValues(new double[] { 3, 7 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(zshape.GetGenericParameters());
            Assert.IsTrue(Math.Abs(-1 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(7 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(15 - triangular.GetValues()[2]) < eps);
        }

        [Test]
        public void ZShapeToSigmoidalTest1()
        {
            Term zshape = TermHelper.instantiate(TermType.ZShape, "test");
            zshape.SetValues(new double[] { 3, 7 });
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.Update(zshape.GetGenericParameters());
            Assert.IsTrue(Math.Abs(0.5493 - sigmoidal.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - sigmoidal.GetValues()[1]) < eps);
        }

        [Test]
        public void setGetValuesTriangular()
        {
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.SetValues(new double[] { 1, 3, 7 });
            Assert.IsTrue(Math.Abs(1 - triangular.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - triangular.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(7 - triangular.GetValues()[2]) < eps);
        }
        [Test]
        public void setGetValuesTrapezoidal()
        {
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.SetValues(new double[] { 1, 3, 7, 10 });
            Assert.IsTrue(Math.Abs(1 - trapezodial.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - trapezodial.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(7 - trapezodial.GetValues()[2]) < eps);
            Assert.IsTrue(Math.Abs(10 - trapezodial.GetValues()[3]) < eps);
        }

        [Test]
        public void setGetValuesGaussian()
        {
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.SetValues(new double[] { 2, 6 });
            Assert.IsTrue(Math.Abs(2 - gaussian.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(6 - gaussian.GetValues()[1]) < eps);
        }

        [Test]
        public void setGetValuesBell()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 1, 3, 7 });
            Assert.IsTrue(Math.Abs(1 - bell.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - bell.GetValues()[1]) < eps);
            Assert.IsTrue(Math.Abs(7 - bell.GetValues()[2]) < eps);
        }

        [Test]
        public void setGetValuesSigmoidal()
        {
            Term sigmoidal = TermHelper.instantiate(TermType.Sigmoidal, "test");
            sigmoidal.SetValues(new double[] { 1, 3 });
            Assert.IsTrue(Math.Abs(1 - sigmoidal.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - sigmoidal.GetValues()[1]) < eps);
        }

        [Test]
        public void setGetValuesSShape()
        {
            Term sshape = TermHelper.instantiate(TermType.SShape, "test");
            sshape.SetValues(new double[] { 1, 3 });
            Assert.IsTrue(Math.Abs(1 - sshape.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - sshape.GetValues()[1]) < eps);
        }
        [Test]
        public void setGetValuesZShape()
        {
            Term zshape = TermHelper.instantiate(TermType.ZShape, "test");
            zshape.SetValues(new double[] { 1, 3 });
            Assert.IsTrue(Math.Abs(1 - zshape.GetValues()[0]) < eps);
            Assert.IsTrue(Math.Abs(3 - zshape.GetValues()[1]) < eps);
        }

        [Test]
        public void testIsKeepTolerance()
        {
            Term gaussian = TermHelper.instantiate(TermType.Gaussian, "test");
            gaussian.SetValues(new double[] { 2, 5 });
            Term zshape = TermHelper.instantiate(TermType.ZShape, "test");
            zshape.Update(gaussian.GetGenericParameters());
            Assert.IsTrue(TermHelper.isKeepTolerance(gaussian, zshape, 0, 10));
        }

        [Test]
        public void testIsKeepTolerance2()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 2, 4, 6 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(bell.GetGenericParameters());
            Assert.IsFalse(TermHelper.isKeepTolerance(bell, triangular, 0, 10));
        }

        [Test]
        public void testIsKeepTolerance3()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 2, 4, 6 });
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.Update(bell.GetGenericParameters());
            Assert.IsTrue(TermHelper.isKeepTolerance(bell, trapezodial, 0, 10));
        }
        [Test]
        public void testIsKeepTolerance4()
        {
            Term trapezoidal = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezoidal.SetValues(new double[] { 1, 5, 7, 8 });
            Term triangular = TermHelper.instantiate(TermType.Triangular, "test");
            triangular.Update(trapezoidal.GetGenericParameters());
            Assert.IsTrue(TermHelper.isKeepTolerance(trapezoidal, triangular, 0, 10));
        }

        [Test]
        public void bellToTrap()
        {
            Term bell = TermHelper.instantiate(TermType.Bell, "test");
            bell.SetValues(new double[] { 2, 4, 6 });
            Term trapezodial = TermHelper.instantiate(TermType.Trapezodial, "test");
            trapezodial.Update(bell.GetGenericParameters());
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                builder.Append(trapezodial.GetValues()[i]);
                builder.Append(" ");
            }
            Debug.Log(builder.ToString());
        }

        [Test]
        public void getDirection()
        {
            GetMovementDirection();
        }

        [Test]
        public void deltaTime()
        {
            Vector2 target = new Vector2(10, -10);
            Vector2 right = new Vector2(1, 0);
            float angle = Vector2.SignedAngle(right, target / target.magnitude);
            Debug.Log(angle);
        }

        [Test]
        public void testSin()
        {
            double sin = Math.Sin(30 * Math.PI / 180);
            Debug.Log(sin);
        }

        [Test]
        public void testArctg()
        {
            double sin = Math.Atan(1) * 180 / Math.PI;
            Debug.Log(sin);
        }

        [Test]
        public void rotateTest()
        {

            Vector2 direction = Quaternion.Euler(0, 0, 45f) * new Vector2(4.9f, 0.7f);
            Debug.Log(direction.ToString());

        }

        private Vector3 GetMovementDirection()
        {
            Vector3 desiredDirection = new Vector3(10,0,0) - new Vector3(40, 0, 0);
            Debug.Log(desiredDirection);
            float rotationAngle = -40;
            desiredDirection.y = 0.0f;
            Quaternion quaternion = Quaternion.AngleAxis(rotationAngle, Vector3.up);
            Debug.Log(quaternion);
            desiredDirection = quaternion * desiredDirection;
            Debug.Log(desiredDirection);
            return desiredDirection;
        }


    }
}
