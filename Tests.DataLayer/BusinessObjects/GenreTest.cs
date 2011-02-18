using BusinessObjects;
using NUnit.Framework;

namespace Tests.DataLayer.BusinessObjects
{
    [TestFixture]
    public class GenreTest
    {
        [Test]
        public void Clone_Successful()
        {
            Genre genre = CreateDefaultGenre();
            Genre genre2 = genre.Clone();

            Assert.AreNotSame(genre, genre2);

            Assert.AreEqual(genre.ID, genre2.ID);
            Assert.AreEqual(genre.Name, genre2.Name);
            Assert.AreEqual(genre.PrivateMarks, genre2.PrivateMarks);
            Assert.AreEqual(genre.Description, genre2.Description);
            Assert.AreEqual(genre.Comments, genre2.Comments);
        }

        [Test]
        public void Equals_Reflexive_Success()
        {
            Genre genre = CreateDefaultGenre();

            Assert.True(genre.Equals(genre));
        }

        [Test]
        public void Equals_Simmetric_Success()
        {
            Genre g1 = CreateDefaultGenre();

            Genre g2 = CreateDefaultGenre();

            Assert.AreNotSame(g1, g2);

            Assert.True(g1.Equals(g2));
            Assert.True(g2.Equals(g1));
        }

        [Test]
        public void Equals_Transitive_Success()
        {
            Genre g1 = CreateDefaultGenre();
            Genre g2 = CreateDefaultGenre();
            Genre g3 = CreateDefaultGenre();

            Assert.AreNotSame(g1, g2);
            Assert.AreNotSame(g2, g3);

            Assert.True(g1.Equals(g2));
            Assert.True(g2.Equals(g3));
            Assert.True(g1.Equals(g3));
        }

        [Test]
        public void Equals_DifferentObjects_Fail()
        {
            Genre g1 = CreateDefaultGenre();
            Genre g2 = CreateDefaultGenre();

            Assert.AreNotSame(g1,g2);
            Assert.True(g1.Equals(g2));

            g2.ID = g2.ID + g2.ID;
            Assert.False(g1.Equals(g2));

            g2 = CreateDefaultGenre();
            g2.Name = g2.Name + g2.Name;
            Assert.False(g1.Equals(g2));

            g2 = CreateDefaultGenre();
            g2.PrivateMarks = g2.PrivateMarks + g2.PrivateMarks;
            Assert.False(g1.Equals(g2));

            g2 = CreateDefaultGenre();
            g2.Description = g2.Description + g2.Description;
            Assert.False(g1.Equals(g2));

            g2 = CreateDefaultGenre();
            g2.Comments = g2.Comments + g2.Comments;
            Assert.False(g1.Equals(g2));
        }

        [Test]
        public void OperatorEquality_SameObjects_Success()
        {
            Genre g1 = CreateDefaultGenre();
            Genre g2 = CreateDefaultGenre();

            Assert.AreNotSame(g1, g2);
            Assert.True(g1.Equals(g2));

            Assert.True(g1 == g2);
        }

        [Test]
        public void OperatorEquality_DifferentObjects_Fail()
        {
            Genre g1 = CreateDefaultGenre();
            Genre g2 = CreateDefaultGenre();

            g2.ID = g2.ID + g2.ID;
            Assert.False(g1.Equals(g2));

            g2 = CreateDefaultGenre();
            g2.Name = g2.Name + g2.Name;
            Assert.False(g1 == g2);

            g2 = CreateDefaultGenre();
            g2.PrivateMarks = g2.PrivateMarks + g2.PrivateMarks;
            Assert.False(g1 == g2);

            g2 = CreateDefaultGenre();
            g2.Description = g2.Description + g2.Description;
            Assert.False(g1 == g2);


            g2 = CreateDefaultGenre();
            g2.Comments = g2.Comments + g2.Comments;
            Assert.False(g1 == g2);
        }

        [Test]
        public void GetHashCode_SameObjects_Success()
        {
            Genre g1 = CreateDefaultGenre();
            Genre g2 = CreateDefaultGenre();

            Assert.AreNotSame(g1, g2);
            Assert.AreEqual(g1, g2);

            Assert.AreEqual(g1.GetHashCode(), g2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentObjects_Fail()
        {
            Genre g1 = CreateDefaultGenre();
            Genre g2 = CreateDefaultGenre();

            g2.ID = g2.ID + g2.ID;
            Assert.False(g1.Equals(g2));

            g2 = CreateDefaultGenre();
            g2.Name = g2.Name + g2.Name;
            Assert.AreNotEqual(g1.GetHashCode(), g2.GetHashCode());

            g2 = CreateDefaultGenre();
            g2.PrivateMarks = g2.PrivateMarks + g2.PrivateMarks;
            Assert.AreNotEqual(g1.GetHashCode(), g2.GetHashCode());

            g2 = CreateDefaultGenre();
            g2.Description = g2.Description + g2.Description;
            Assert.AreNotEqual(g1.GetHashCode(), g2.GetHashCode());


            g2 = CreateDefaultGenre();
            g2.Comments = g2.Comments + g2.Comments;
            Assert.AreNotEqual(g1.GetHashCode(), g2.GetHashCode());
        }

        private Genre CreateDefaultGenre()
        {
            Genre genre = new Genre()
            {
                ID = 1,
                Name = "Genre1",
                PrivateMarks = "None",
                Description = "None",
                Comments = "Unknown"
            };

            return genre;
        }
    }
}
