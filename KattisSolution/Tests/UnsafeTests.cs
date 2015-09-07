using NUnit.Framework;

namespace KattisSolution.Tests
{
    [TestFixture]
    public class UnsafeTests
    {
        [Test]
        public unsafe void CreateArray_Should_FillInArray()
        {
            // Arrange
            int[,] expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] data = new int[2, 3];

            // Act
            fixed (int* intPtr = data)
            {
                for (int i = 0; i < data.GetLength(0) * data.GetLength(1); i++)
                {
                    intPtr[i] = i + 1;
                }
            }

            // Assert
            CollectionAssert.AreEqual(expected, data);
        }

        [Test]
        public unsafe void ReadArray_Should_ReadArrayUsingPointerArithmetic()
        {
            // Arrange
            int[,] data = { { 1, 2, 3 }, { 4, 5, 6 } };
            int[] expected = { 1, 2, 3, 4, 5, 6 };

            // Act
            fixed (int* intPtrFixed = data)
            {
                int* intPtr = intPtrFixed;

                for (int i = 0; i < data.GetLength(0) * data.GetLength(1); i++)
                {
                    int value = *intPtr;
                    intPtr++;

                    // Assert
                    Assert.That(expected[i], Is.EqualTo(value), "Should equal for {0} element", i);
                }
            }
        }
    }
}
