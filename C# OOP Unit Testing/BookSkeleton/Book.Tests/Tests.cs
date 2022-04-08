namespace Book.Tests
{
    using System;

    using NUnit.Framework;

    public class Tests
    {
        private Book book;

        [SetUp]
        public void SetUp()
        {
            book = new Book("Guardians", "Dian Dwire");
        }

        [Test]
        public void ConstructorWorkCorrectly()
        {
            Assert.AreEqual("Guardians", book.BookName);
            Assert.AreEqual("Dian Dwire", book.Author);
        }

        [Test]
        public void PropertyNameThrowExceptionNullOrEmpty()
        {
            Assert.Throws <ArgumentException>(() => book = new Book(null, "Dian Dwire"));
            Assert.Throws<ArgumentException>(() => book = new Book(string.Empty, "Dian Dwire"));
        }

        [Test]
        public void PropertyAutorThrowExceprtionForNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => book = new Book("HarryPotter", null));
            Assert.Throws<ArgumentException>(() => book = new Book("HarryPotter", string.Empty));
        }

        [Test]
        public void AddFootnote_ShouldWorkProperly()
        {
            book.AddFootnote(2, "blabla");
            Assert.AreEqual(1, book.FootnoteCount);
        }

        [Test]
        public void AddFootnote_ShouldThrowExceptionWhenTryingToAddExistingFootNote()
        {
            book.AddFootnote(2, "something");
            Assert.Throws<InvalidOperationException>(() => book.AddFootnote(2, "something"));
        }

        [Test]
        public void FindFootnoteShouldThrowExceptionWhenTryingToReachNonexistingFootnote()
        {
            Assert.Throws<InvalidOperationException>(() => book.FindFootnote(6));
        }

        [Test]
        public void FindFootnoteShouldWorkCorrectly()
        {
            book.AddFootnote(2, "something");
            string expected = $"Footnote #2: something";
            Assert.AreEqual(expected, book.FindFootnote(2));
        }

        [Test]
        public void AlterFootnoteShouldThrowExceptionWhenTryingToAlterNonexistingFootnote()
        {
            Assert.Throws<InvalidOperationException>(() => this.book.AlterFootnote(4, "sometext"));
        }

        [Test]
        public void AlterFootnote_ShouldWorkProperly()
        {
            this.book.AddFootnote(4, "blabla");
            this.book.AlterFootnote(4, "kvakva");
            string expected = $"Footnote #4: kvakva";
            Assert.AreEqual(expected, this.book.FindFootnote(4));

        }
    }
}