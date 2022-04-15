using System;
using NUnit.Framework;

namespace SmartphoneShop.Tests
{
    [TestFixture]
    public class SmartphoneShopTests
    {
        [Test]
        public void ValidatioForCtor()
        {
            var phone = new Smartphone("IPhone", 13);
            var capacity = 1;
            var market = new Shop(capacity);
            market.Add(phone);

            Assert.AreEqual(capacity, market.Capacity);
        }

        [Test]
        public void CapacityThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Shop(-1));
        }

        [Test]
        public void CountCorrectly()
        {
            var phone = new Smartphone("IPhone", 13);
            var shop = new Shop(1);
            shop.Add(phone);

            Assert.AreEqual(1, shop.Capacity);
        }

        [Test]
        public void AddPhoneThrowExceptionForSameName()
        {
            var phone = new Smartphone("IPhone", 13);
            var shop = new Shop(2);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.Add(new Smartphone("IPhone", 13)));
        }

        [Test]
        public void AddPhoneThrowExceptionForCapacity()
        {
            var phone = new Smartphone("IPhone", 13);
            var shop = new Shop(1);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.Add(new Smartphone("Galaxy Note", 10)));
        }

        [Test]
        public void AddPhoneCorrectly()
        {
            var phone = new Smartphone("IPhone", 13);
            var phone1 = new Smartphone("galaxy", 22);
            var shop = new Shop(2);
            shop.Add(phone);
            shop.Add(phone1);

            Assert.AreEqual(2, shop.Count);
        }

        [Test]
        public void RemoveThrowExceptionForNull()
        {
            var phone = new Smartphone("IPhone", 13);
            var shop = new Shop(10);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.Remove(null));           
        }

        [Test]
        public void RemoveThrowExceptionForInvalidName()
        {
            var phone = new Smartphone("IPhone", 13);
            var shop = new Shop(10);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.Remove("Galaxy Phone"));
            Assert.Throws<InvalidOperationException>(() => shop.Remove(string.Empty));
            Assert.Throws<InvalidOperationException>(() => shop.Remove(null));
        }

        [Test]
        public void TestPhoneThrowExceptionForNotExist()
        {
            var phone = new Smartphone("Galaxy", 100);
            var shop = new Shop(10);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.TestPhone("IPhone", 50));
            Assert.Throws<InvalidOperationException>(() => shop.TestPhone(null, 50));
            Assert.Throws<InvalidOperationException>(() => shop.TestPhone(string.Empty, 50));
        }

        [Test]
        public void TestPhoneThrowExceptionForLowBattery()
        {
            var phone = new Smartphone("Galaxy", 50);
            var shop = new Shop(10);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.TestPhone("Galaxy", 100));
        }

        [Test]
        public void TestPhoneWorkCorrectly()
        {
            var phone = new Smartphone("Galaxy", 100);
            //phone.CurrentBateryCharge = 50;
            var shop = new Shop(10);
            shop.Add(phone);
            shop.TestPhone("Galaxy", 50);

            Assert.AreEqual(50, phone.CurrentBateryCharge);
            
        }

        [Test]
        public void ChargePhoneThrowExceptionForNull()
        {
            var phone = new Smartphone("Galaxy", 100);
            phone.CurrentBateryCharge = 50;
            var shop = new Shop(10);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.ChargePhone(null));
            Assert.Throws<InvalidOperationException>(() => shop.ChargePhone("IPhone"));
            Assert.Throws<InvalidOperationException>(() => shop.ChargePhone(string.Empty));
        }

        [Test]
        public void ChargePhone()
        {
            var phone = new Smartphone("Galaxy", 100);
            var shop = new Shop(10);
            shop.Add(phone);

            shop.TestPhone("Galaxy", 50);
            phone.CurrentBateryCharge = 50;
            shop.ChargePhone("Galaxy");

            Assert.AreEqual(100, phone.CurrentBateryCharge);
            Assert.AreEqual(100, phone.MaximumBatteryCharge);
        }
    }
}