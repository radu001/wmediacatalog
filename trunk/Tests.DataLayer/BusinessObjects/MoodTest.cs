using BusinessObjects;
using NUnit.Framework;

namespace MediaCatalog.Tests
{
    [TestFixture]
    public class MoodTest
    {
        [Test]
        public void Clone_Successful()
        {
            Mood mood = CreateDefaultMood();
            Mood mood2 = mood.Clone();

            Assert.AreNotSame(mood, mood2);

            Assert.AreEqual(mood.ID, mood2.ID);
            Assert.AreEqual(mood.Name, mood2.Name);
            Assert.AreEqual(mood.PrivateMarks, mood2.PrivateMarks);
            Assert.AreEqual(mood.Description, mood2.Description);
            Assert.AreEqual(mood.Comments, mood2.Comments);
        }

        [Test]
        public void Equals_Reflexive_Success()
        {
            Mood mood = CreateDefaultMood();

            Assert.True(mood.Equals(mood));
        }

        [Test]
        public void Equals_Simmetric_Success()
        {
            Mood m1 = CreateDefaultMood();

            Mood m2 = CreateDefaultMood();

            Assert.AreNotSame(m1, m2);

            Assert.True(m1.Equals(m2));
            Assert.True(m2.Equals(m1));
        }

        [Test]
        public void Equals_Transitive_Success()
        {
            Mood m1 = CreateDefaultMood();
            Mood m2 = CreateDefaultMood();
            Mood m3 = CreateDefaultMood();

            Assert.AreNotSame(m1, m2);
            Assert.AreNotSame(m2, m3);

            Assert.True(m1.Equals(m2));
            Assert.True(m2.Equals(m3));
            Assert.True(m1.Equals(m3));
        }

        [Test]
        public void Equals_DifferentObjects_Fail()
        {
            Mood m1 = CreateDefaultMood();
            Mood m2 = CreateDefaultMood();

            Assert.AreNotSame(m1, m2);
            Assert.True(m1.Equals(m2));

            m2.ID = m2.ID + m2.ID;
            Assert.False(m1.Equals(m2));

            m2 = CreateDefaultMood();
            m2.Name = m2.Name + m2.Name;
            Assert.False(m1.Equals(m2));

            m2 = CreateDefaultMood();
            m2.PrivateMarks = m2.PrivateMarks + m2.PrivateMarks;
            Assert.False(m1.Equals(m2));

            m2 = CreateDefaultMood();
            m2.Description = m2.Description + m2.Description;
            Assert.False(m1.Equals(m2));

            m2 = CreateDefaultMood();
            m2.Comments = m2.Comments + m2.Comments;
            Assert.False(m1.Equals(m2));
        }

        [Test]
        public void OperatorEquality_SameObjects_Success()
        {
            Mood m1 = CreateDefaultMood();
            Mood m2 = CreateDefaultMood();

            Assert.AreNotSame(m1, m2);
            Assert.True(m1.Equals(m2));

            Assert.True(m1 == m2);
        }

        [Test]
        public void OperatorEquality_DifferentObjects_Fail()
        {
            Mood m1 = CreateDefaultMood();
            Mood m2 = CreateDefaultMood();

            m2.ID = m2.ID + m2.ID;
            Assert.False(m1.Equals(m2));

            m2 = CreateDefaultMood();
            m2.Name = m2.Name + m2.Name;
            Assert.False(m1 == m2);

            m2 = CreateDefaultMood();
            m2.PrivateMarks = m2.PrivateMarks + m2.PrivateMarks;
            Assert.False(m1 == m2);

            m2 = CreateDefaultMood();
            m2.Description = m2.Description + m2.Description;
            Assert.False(m1 == m2);


            m2 = CreateDefaultMood();
            m2.Comments = m2.Comments + m2.Comments;
            Assert.False(m1 == m2);
        }

        [Test]
        public void GetHashCode_SameObjects_Success()
        {
            Mood m1 = CreateDefaultMood();
            Mood m2 = CreateDefaultMood();

            Assert.AreNotSame(m1, m2);
            Assert.AreEqual(m1, m2);

            Assert.AreEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentObjects_Fail()
        {
            Mood m1 = CreateDefaultMood();
            Mood m2 = CreateDefaultMood();

            m2.ID = m2.ID + m2.ID;
            Assert.False(m1.Equals(m2));

            m2 = CreateDefaultMood();
            m2.Name = m2.Name + m2.Name;
            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());

            m2 = CreateDefaultMood();
            m2.PrivateMarks = m2.PrivateMarks + m2.PrivateMarks;
            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());

            m2 = CreateDefaultMood();
            m2.Description = m2.Description + m2.Description;
            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());


            m2 = CreateDefaultMood();
            m2.Comments = m2.Comments + m2.Comments;
            Assert.AreNotEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        private Mood CreateDefaultMood()
        {
            Mood genre = new Mood()
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
