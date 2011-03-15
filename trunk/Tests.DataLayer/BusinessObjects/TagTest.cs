
using System;
using BusinessObjects;
using NUnit.Framework;
namespace MediaCatalog.Tests
{
    [TestFixture]
    public class TagTest
    {
        [Test]
        public void Clone_Successful()
        {
            Tag target = CreateDefaultTag();

            Tag actual = target.Clone();

            Assert.AreNotSame(target, actual);

            Assert.AreEqual(target.ID, actual.ID);
            Assert.AreEqual(target.Name, actual.Name);
            Assert.AreEqual(target.PrivateMarks, actual.PrivateMarks);
            Assert.AreEqual(target.Description, actual.Description);
            Assert.AreEqual(target.CreateDate, actual.CreateDate);
            Assert.AreEqual(target.Comments, actual.Comments);
        }

        [Test]
        public void Equals_Reflexive_Success()
        {
            Tag t1 = CreateDefaultTag();

            Assert.True(t1.Equals(t1));
        }

        [Test]
        public void Equals_Simmetric_Success()
        {
            Tag t1 = CreateDefaultTag();
            Tag t2 = CreateDefaultTag();

            Assert.AreNotSame(t1, t2);

            Assert.True(t1.Equals(t2));
            Assert.True(t2.Equals(t1));
        }

        [Test]
        public void Equals_Transitive_Success()
        {
            Tag t1 = CreateDefaultTag();
            Tag t2 = CreateDefaultTag();
            Tag t3 = CreateDefaultTag();

            Assert.AreNotSame(t1, t2);
            Assert.AreNotSame(t2, t3);

            Assert.True(t1.Equals(t2));
            Assert.True(t2.Equals(t3));
            Assert.True(t1.Equals(t3));
        }

        [Test]
        public void Equals_DifferentObjects_Fail()
        {
            Tag t1 = CreateDefaultTag();
            Tag t2 = CreateDefaultTag();

            Assert.AreNotSame(t1, t2);
            Assert.True(t1.Equals(t2));

            t2.Name = t2.Name + t2.Name;
            Assert.False(t1.Equals(t2));

            t2 = CreateDefaultTag();
            t2.PrivateMarks = t2.PrivateMarks + t2.PrivateMarks;
            Assert.False(t1.Equals(t2));

            t2 = CreateDefaultTag();
            t2.Description = t2.Description + t2.Description;
            Assert.False(t1.Equals(t2));

            t2 = CreateDefaultTag();
            t2.CreateDate = t2.CreateDate.AddDays(20);
            Assert.False(t1.Equals(t2));

            t2 = CreateDefaultTag();
            t2.Comments = t2.Comments + t2.Comments;
            Assert.False(t1.Equals(t2));
        }

        [Test]
        public void OperatorEquality_SameObjects_Success()
        {
            Tag t1 = CreateDefaultTag();
            Tag t2 = CreateDefaultTag();

            Assert.AreNotSame(t1, t2);
            Assert.True(t1.Equals(t2));

            Assert.True(t1 == t2);
        }

        [Test]
        public void OperatorEquality_DifferentObjects_Fail()
        {
            Tag t1 = CreateDefaultTag();
            Tag t2 = CreateDefaultTag();

            t2.Name = t2.Name + t2.Name;
            Assert.False(t1 == t2);

            t2 = CreateDefaultTag();
            t2.PrivateMarks = t2.PrivateMarks + t2.PrivateMarks;
            Assert.False(t1 == t2);

            t2 = CreateDefaultTag();
            t2.Description = t2.Description + t2.Description;
            Assert.False(t1 == t2);

            t2 = CreateDefaultTag();
            t2.CreateDate = t2.CreateDate.AddDays(20);
            Assert.False(t1 == t2);

            t2 = CreateDefaultTag();
            t2.Comments = t2.Comments + t2.Comments;
            Assert.False(t1 == t2);
        }

        [Test]
        public void GetHashCode_SameObjects_Success()
        {
            Tag t1 = CreateDefaultTag();
            Tag t2 = CreateDefaultTag();

            Assert.AreNotSame(t1, t2);
            Assert.AreEqual(t1, t2);

            Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentObjects_Fail()
        {
            Tag t1 = CreateDefaultTag();
            Tag t2 = CreateDefaultTag();

            t2.Name = t2.Name + t2.Name;
            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());

            t2 = CreateDefaultTag();
            t2.PrivateMarks = t2.PrivateMarks + t2.PrivateMarks;
            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());

            t2 = CreateDefaultTag();
            t2.Description = t2.Description + t2.Description;
            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());

            t2 = CreateDefaultTag();
            t2.CreateDate = t2.CreateDate.AddDays(20);
            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());

            t2 = CreateDefaultTag();
            t2.Comments = t2.Comments + t2.Comments;
            Assert.AreNotEqual(t1.GetHashCode(), t2.GetHashCode());
        }

        private Tag CreateDefaultTag()
        {
            Tag t1 = new Tag();
            t1.Name = "Tag1";
            t1.PrivateMarks = "None";
            t1.Description = "Unknown";
            t1.CreateDate = new DateTime(2010, 05, 05);
            t1.Comments = "No comments";

            return t1;
        }
    }
}
