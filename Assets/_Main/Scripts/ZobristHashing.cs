

using Random = System.Random;

namespace Chessticle
{
    // Used to hash board positions in order to detect position repetitions
    public class ZobristHashing
    {
        static ZobristHashing()
        {
            const int seed = 18761234; // any seed would do
            var random = new Random(seed);

            const int squareCount = 64; 
            // one random 64bit hash for each combination of a board value (piece, color, virginity)
            // and a position on the board (0 .. 63)
            s_Hashes = new ulong[Chessboard.MaxBoardValue + 1, squareCount];

            for (int i = 0; i < Chessboard.MaxBoardValue; i++)
            {
                for (int j = 0; j < squareCount; j++)
                {
                    s_Hashes[i, j] = (ulong) random.Next() | ((ulong) random.Next() << 32);
                }
            }

            s_WhitePlayersHash = (ulong) random.Next() | ((ulong) random.Next() << 32);
        }

        public static ulong HashPosition(byte[] squares0X88, Color currentPlayer)
        {
            ulong result = 0;
            
            // combine the hashes of all the squares using bitwise XOR
            const int squareCount = 64;
            for (int squareIdx = 0; squareIdx < squareCount; squareIdx++)
            {
                var idx0X88 = squareIdx + (squareIdx & ~7);
                int boardValue = squares0X88[idx0X88];
                bool isEmptySquare = boardValue == 0;
                if (isEmptySquare) continue;

                result ^= s_Hashes[boardValue, squareIdx];
            }

            if (currentPlayer == Color.White)
            {
                result ^= s_WhitePlayersHash;
            }

            return result;
        }

        static readonly ulong s_WhitePlayersHash;
        static readonly ulong[,] s_Hashes;
    }
}