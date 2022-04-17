using System;
using NUnit.Framework;

public class HeroRepositoryTests
{
    [Test]
    public void CtorValidation()
    {
        var hero = new Hero("Spiro", 100);

        Assert.AreEqual("Spiro", hero.Name);
        Assert.AreEqual(100, hero.Level);
        
    }

    [Test]
    public void PropertyCreateThrowExceptionNullEmpty()
    {
        var repo = new HeroRepository();

        Assert.Throws<ArgumentNullException>(() => repo.Create(null));
    }

    [Test]
    public void PropertyCreateThrowExceptionExist()
    {
        var repo = new HeroRepository();
        repo.Create(new Hero("Spiro", 100));

        Assert.Throws<InvalidOperationException>(() => repo.Create(new Hero("Spiro", 100)));
    }

    [Test]
    public void PropertyCreateValidation()
    {
        var repo = new HeroRepository();
        repo.Create(new Hero("Spiro", 100));

        Assert.AreEqual(1, repo.Heroes.Count);
    }

    [Test]
    public void PropertyRemoveThrowExceptionNullWhiteSpace()
    {
        var repo = new HeroRepository();

        Assert.Throws<ArgumentNullException>(() => repo.Remove(null));
        Assert.Throws<ArgumentNullException>(() => repo.Remove(string.Empty));
    }

    [Test]
    public void PropertyRemoveValidation()
    {
        var repo = new HeroRepository();
        repo.Create(new Hero("Spiro", 100));
        repo.Create(new Hero("Kiro", 100));

        var result = repo.Remove("Spiro");

        Assert.AreEqual(true, result);
    }

    [Test]
    public void GetHighestLevelValidation()
    {
        var repo = new HeroRepository();
        repo.Create(new Hero("Spiro", 80));
        repo.Create(new Hero("Miro", 90));
        repo.Create(new Hero("Kiro", 100));

        var result = repo.GetHeroWithHighestLevel();

        Assert.AreEqual(100, result.Level);
    }

    [Test]
    public void GetHeroValidation()
    {
        var repo = new HeroRepository();
        repo.Create(new Hero("Spiro", 80));

        var result = repo.GetHero("Spiro");

        Assert.AreEqual("Spiro", result.Name);
    }
}