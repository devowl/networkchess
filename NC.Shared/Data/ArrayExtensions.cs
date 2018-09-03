namespace NC.Shared.Data
{
    /// <summary>
    /// Array extensions.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Convert multi-dimensional array to jagged.
        /// </summary>
        /// <typeparam name="T">Array type.</typeparam>
        /// <param name="twoDimensionalArray">Two-dimensional array.</param>
        /// <returns>Jagged array.</returns>
        public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }

        /// <summary>
        /// Convert jagged to multi-dimensional.
        /// </summary>
        /// <typeparam name="T">Array type.</typeparam>
        /// <param name="jaggedArray">Jagged array.</param>
        /// <returns>Multi-dimensional array.</returns> 
        public static T[,] ToMultiDimensionalArray<T>(this T[][] jaggedArray)
        {
            T[,] result = new T[jaggedArray.Length, jaggedArray[0].Length];

            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int k = 0; k < jaggedArray[0].Length; k++)
                {
                    result[i, k] = jaggedArray[i][k];
                }
            }

            return result;
        }
    }
}