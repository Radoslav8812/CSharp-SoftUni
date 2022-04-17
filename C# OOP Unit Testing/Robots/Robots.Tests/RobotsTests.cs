namespace Robots.Tests
{
    using System;
    using NUnit.Framework;

    public class RobotsTests
    {
        [Test]
        public void CtorValidation()
        {
            var robot = new Robot("Robo", 100);
            var robotManager = new RobotManager(10);

            Assert.AreEqual("Robo", robot.Name);
            Assert.AreEqual(100, robot.MaximumBattery);
        }

        [Test]
        public void CapacityThrowException()
        {
            Assert.Throws<ArgumentException>(() => new RobotManager(-10));
        }

        [Test]
        public void CapacityValidation()
        {
            var robotManager = new RobotManager(10);

            Assert.AreEqual(10, robotManager.Capacity);
        }

        [Test]
        public void CountValidation()
        {
            var robotManager = new RobotManager(10);

            robotManager.Add(new Robot("1", 1));
            robotManager.Add(new Robot("2", 2));
            robotManager.Add(new Robot("3", 3));

            Assert.AreEqual(3, robotManager.Count);
        }

        [Test]
        public void MethodAddThrowExceptionExistName()
        {
            var robotManager = new RobotManager(10);
            robotManager.Add(new Robot("1", 1));

            Assert.Throws<InvalidOperationException>(() => robotManager.Add(new Robot("1", 2)));
        }

        [Test]
        public void MethodAddThrowExceptionCapacity()
        {
            var robotManager = new RobotManager(1);
            robotManager.Add(new Robot("1", 1));

            Assert.Throws<InvalidOperationException>(() => robotManager.Add(new Robot("2", 2)));
        }

        [Test]
        public void MethodAddValidation()
        {
            var robotManager = new RobotManager(1);
            robotManager.Add(new Robot("1", 1));

            Assert.AreEqual(1, robotManager.Count);
        }

        [Test]
        public void MethodRemoveInvalidOperationException()
        {
            var robotManager = new RobotManager(1);
            Assert.Throws<InvalidOperationException>(() => robotManager.Remove(null));
            Assert.Throws<InvalidOperationException>(() => robotManager.Remove(string.Empty));
            Assert.Throws<InvalidOperationException>(() => robotManager.Remove("Spiro"));
        }

        [Test]
        public void MethodRemoveValidation()
        {
            var robotManager = new RobotManager(1);
            robotManager.Add(new Robot("1", 1));
            robotManager.Remove("1");

            Assert.AreEqual(0, robotManager.Count);
        }

        [Test]
        public void MethodWorkThrowExceptionForNull()
        {
            var robotManager = new RobotManager(1);
            Assert.Throws<InvalidOperationException>(() => robotManager.Work(null, null, 100));
            Assert.Throws<InvalidOperationException>(() => robotManager.Work(string.Empty, string.Empty, 100));
            Assert.Throws<InvalidOperationException>(() => robotManager.Work("Spiro", "any", 100));
        }

        [Test]
        public void MethodWorkThrowExceptionForBattery()
        {
            var robotManager = new RobotManager(10);
            robotManager.Add(new Robot("1", 50));
            robotManager.Add(new Robot("2", 25));


            Assert.Throws<InvalidOperationException>(() => robotManager.Work("1", "bla", 100));
            Assert.Throws<InvalidOperationException>(() => robotManager.Work("2", "kva", 51));
        }

        [Test]
        public void MethodWorkValidation()
        {
            var robotManager = new RobotManager(10);
            var robot = new Robot("1", 50);
            robotManager.Add(robot);
            robotManager.Work("1", "a", 40);

            Assert.AreEqual(10, robot.Battery);
        }

        [Test]
        public void MethodChargeThrowExceptionForNullName()
        {
            var robotManager = new RobotManager(10);

            robotManager.Add(new Robot("1", 50));
            robotManager.Add(new Robot("2", 25));


            Assert.Throws<InvalidOperationException>(() => robotManager.Charge("33"));
        }

        [Test]
        public void ChargeValidation()
        {
            var robotManager = new RobotManager(10);
            var robot = new Robot("1", 50);
            robotManager.Add(robot);
            robotManager.Work("1", "a", 40);
            robotManager.Charge("1");

            Assert.AreEqual(robot.Battery, robot.MaximumBattery);
        }
    }
}
