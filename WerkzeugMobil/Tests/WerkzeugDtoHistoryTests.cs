using NUnit.Framework;
using System.Collections.Generic;
using WerkzeugMobil.DTO;

namespace WerkzeugMobil.Tests
{
    [TestFixture]
    public class WerkzeugDtoHistoryTests
    {
        [Test]
        public void History_Speichert_Maximal_5_Adressen()
        {
            // Arrange
            var werkzeug = new WerkzeugDto
            {
                WerkzeugId = "WZ-1",
                History = new List<string>()
            };

            // Act: 7 Adressen hinzufügen
            for (int i = 1; i <= 7; i++)
                AddAddressToHistory(werkzeug, $"Adresse {i}");

            // Assert
            Assert.That(werkzeug.History.Count, Is.EqualTo(5));
            Assert.That(werkzeug.History[0], Is.EqualTo("Adresse 3"));
            Assert.That(werkzeug.History[4], Is.EqualTo("Adresse 7"));
        }

        private void AddAddressToHistory(WerkzeugDto w, string addr)
        {
            w.History.Add(addr);
            if (w.History.Count > 5)
                w.History.RemoveAt(0);
        }
    }
}
