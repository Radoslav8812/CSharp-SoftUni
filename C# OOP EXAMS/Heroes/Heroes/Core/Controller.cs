using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Heroes.Core.Contracts;
using Heroes.Models.Contracts;
using Heroes.Models.Heroes;
using Heroes.Models.Map;
using Heroes.Models.Weapons;
using Heroes.Repositories;

namespace Heroes.Core
{
	public class Controller : IController
	{
        private HeroRepository heroRepository;
        private WeaponRepository weaponRepository;
        private readonly IMap map;

		public Controller()
		{
            heroRepository = new HeroRepository();
            weaponRepository = new WeaponRepository();
            map = new Map();
		}

        public string AddWeaponToHero(string weaponName, string heroName)
        {
            var hero = heroRepository.FindByName(heroName);
            var weapon = weaponRepository.FindByName(weaponName);

            if (hero == null)
            {
                throw new InvalidOperationException($"Hero {heroName} does not exist.");
            }
            if (weapon == null)
            {
                throw new InvalidOperationException($"Weapon {weaponName} does not exist.");
            }

            if (hero.Weapon != null)
            {
                throw new InvalidOperationException($"Hero {heroName} is well-armed.");
            }

            hero.AddWeapon(weapon);
            string result = $"Hero {heroName} can participate in battle using a {weapon.GetType().Name.ToLower()}.";
            return result;
        }

        public string CreateHero(string type, string name, int health, int armour)
        {
            IHero hero;

            if (type == "Barbarian")
            {
                hero = new Barbarian(name, health, armour);
            }
            else if (type == "Knight")
            {
                hero = new Knight(name, health, armour);
            }
            else
            {
                throw new InvalidOperationException("Invalid hero type.");
            }

            if (heroRepository.Models.Any(x => x.Name == name))
            {
                throw new InvalidOperationException($"The hero {name} already exists.");
            }

            heroRepository.Add(hero);
            string result = string.Empty;

            if (hero.GetType().Name == "Barbarian")
            {
                result = $"Successfully added Barbarian { name } to the collection.";
            }
            else if (hero.GetType().Name == "Knight")
            {
                result = $"Successfully added Sir { name } to the collection.";
            }

            return result;
        }

        public string CreateWeapon(string type, string name, int durability)
        {
            IWeapon weapon;

            if (type == "Claymore")
            {
                weapon = new Claymore(name, durability);
            }
            else if (type == "Mace")
            {
                weapon = new Mace(name, durability);
            }
            else
            {
                throw new InvalidOperationException("Invalid weapon type.");
            }

            if (weaponRepository.Models.Any(x => x.Name == name))
            {
                throw new InvalidOperationException($"The weapon { name } already exists.");
            }

            weaponRepository.Add(weapon);
            return $"A {weapon.GetType().Name} {name} is added to the collection.";
        }

        public string HeroReport()
        {
            var sb = new StringBuilder();
            var orderedHeroes = heroRepository.Models.OrderBy(x => x.GetType().Name).ThenByDescending(x => x.Health).ThenBy(x => x.Name).ToList();

            foreach (var hero in orderedHeroes)
            {
                sb.AppendLine($"{hero.GetType().Name}: {hero.Name}");
                sb.AppendLine($"--Health: {hero.Health}");
                sb.AppendLine($"--Armour: {hero.Armour}");
                string weaponYesOrNo = hero.Weapon != null ? $"{hero.Weapon.Name}" : "Unarmed";
                sb.AppendLine($"--Weapon: {weaponYesOrNo}");
            }

            return sb.ToString().TrimEnd();
        }

        public string StartBattle()
        {
            var players = new List<IHero>(heroRepository.Models.Where(x => x.Weapon != null));

            return map.Fight(players);
        }
    }
}

                                              