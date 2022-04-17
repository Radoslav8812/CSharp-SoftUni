using System;
using NUnit.Framework;

namespace Gyms.Tests
{
    public class GymsTests
    {
        [Test]
        public void ValidationCtor()
        {
            var gym = new Gym("Pulse", 100);
            var athlete = new Athlete("Rado");

            Assert.AreEqual("Pulse", gym.Name);
            Assert.AreEqual(100, gym.Capacity);
            Assert.AreEqual("Rado", athlete.FullName);
        }

        [Test]
        public void PropertyNameThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new Gym(null, 100));
            Assert.Throws<ArgumentNullException>(() => new Gym(String.Empty, 100));
        }

        [Test]
        public void PropertyNameValidation()
        {
            var gym = new Gym("Pulse", 100);

            Assert.AreEqual("Pulse", gym.Name);
        }

        [Test]
        public void PropertyCapacityThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Gym("Athletic", -100));
        }

        [Test]
        public void PropertyCapacityValidation()
        {
            var gym = new Gym("Pulse", 100);

            Assert.AreEqual(100, gym.Capacity);
        }

        [Test]
        public void CountValidation()
        {
            var gym = new Gym("Pulse", 100);
            var athlete = new Athlete("Rado");
            var athlete1 = new Athlete("Kiro");
            var athlete2 = new Athlete("spiro");

            gym.AddAthlete(athlete);
            gym.AddAthlete(athlete1);
            gym.AddAthlete(athlete2);

            Assert.AreEqual(3, gym.Count);
        }

        [Test]
        public void MethodAddAthleteThrowExceptionForCapacity()
        {
            var gym = new Gym("Pulse", 3);
            var athlete = new Athlete("Rado");
            var athlete1 = new Athlete("Kiro");
            var athlete2 = new Athlete("spiro");

            gym.AddAthlete(athlete);
            gym.AddAthlete(athlete1);
            gym.AddAthlete(athlete2);

            Assert.Throws<InvalidOperationException>(() => gym.AddAthlete(new Athlete("Prahan")));
        }

        [Test]
        public void MethodRemoveAthleteThrowException()
        {
            var gym = new Gym("Pulse", 100);

            Assert.Throws<InvalidOperationException>(() => gym.RemoveAthlete(null));
        }

        [Test]
        public void MethodRemoveAthleteValidation()
        {
            var gym = new Gym("Pulse", 100);

            gym.AddAthlete(new Athlete("Kiro"));
            gym.AddAthlete(new Athlete("Spiro"));

            gym.RemoveAthlete("Spiro");

            Assert.AreEqual(1, gym.Count);
        }

        [Test]
        public void MethodInjureAthleteThrowException()
        {
            var gym = new Gym("Pulse", 100);

            gym.AddAthlete(new Athlete("Kiro"));
            gym.AddAthlete(new Athlete("Spiro"));

            Assert.Throws<InvalidOperationException>(() => gym.InjureAthlete(null));
            Assert.Throws<InvalidOperationException>(() => gym.InjureAthlete(String.Empty));
            Assert.Throws<InvalidOperationException>(() => gym.InjureAthlete("Brashlqn"));
        }

        [Test]
        public void MethodInjureAthleteValidation()
        {
            var gym = new Gym("Pulse", 100);

            gym.AddAthlete(new Athlete("Kiro"));
            gym.AddAthlete(new Athlete("Spiro"));

            var injured = gym.InjureAthlete("Spiro");

            Assert.AreEqual(injured.FullName, "Spiro");
        }

        [Test]
        public void MethodReportValidation()
        {
            var gym = new Gym("west", 1);
            gym.AddAthlete(new Athlete("Spiro"));
            var expectedMsg = $"Active athletes at west: Spiro";
            Assert.AreEqual(expectedMsg, gym.Report());
        }
    }
}
