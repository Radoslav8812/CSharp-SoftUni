using System;
using System.Collections.Generic;
using System.Linq;
using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Peripherals;

namespace OnlineShop.Models.Products.Computers
{
	public abstract class Computer : Product, IComputer
	{

        private List<IComponent> components;
        private List<IPeripheral> peripherals;

        public Computer(int id, string manufacturer, string model, decimal price, double overallPerformance) : base(id, manufacturer, model, price, overallPerformance)
        {
            components = new List<IComponent>();
            peripherals = new List<IPeripheral>();
        }

        public override double OverallPerformance => components.Any() ? base.OverallPerformance + components.Average(x => x.OverallPerformance) : base.OverallPerformance;

        public IReadOnlyCollection<IComponent> Components
        {
            get
            {
                return components.AsReadOnly();
            }
        }

        public override decimal Price => base.Price + components.Sum(x => x.Price) + peripherals.Sum(x => x.Price);

        public IReadOnlyCollection<IPeripheral> Peripherals
        {
            get
            {
                return peripherals.AsReadOnly();
            }
        }

        public void AddComponent(IComponent component)
        {
            var currentCompoment = components.FirstOrDefault(x => x.GetType().Name == component.GetType().Name);

            if (!components.Contains(currentCompoment))
            {
                throw new ArgumentException($"Component {component.GetType().Name} already exists in {GetType().Name} with Id {Id}.");
            }

            components.Add(component);
        }

        public void AddPeripheral(IPeripheral peripheral)
        {
            var currentPeripheral = peripherals.FirstOrDefault(x => x.GetType().Name == peripheral.GetType().Name);

            if (currentPeripheral != null)
            {
                throw new ArgumentException($"Peripheral {peripheral.GetType().Name} already exists in {GetType().Name} with Id {Id}.");
            }

            peripherals.Add(peripheral);
        }

        public IComponent RemoveComponent(string componentType)
        {
            var currentComponent = components.FirstOrDefault(x => x.GetType().Name == componentType);

            if (Components.Count == 0 || currentComponent == null)
            {
                throw new ArgumentException($"Component {componentType} does not exist in {GetType().Name} with Id {Id}.");
            }

            components.Remove(currentComponent);
            return currentComponent;
        }

        public IPeripheral RemovePeripheral(string peripheralType)
        {
            var currentPeripheral = peripherals.FirstOrDefault(x => x.GetType().Name == peripheralType);

            if (peripherals.Count == 0 || currentPeripheral == null)
            {
                throw new ArgumentException($"Peripheral {peripheralType} does not exist in {GetType().Name} with Id {Id}.");
            }

            peripherals.Remove(currentPeripheral);
            return currentPeripheral;
        }
    }
}

