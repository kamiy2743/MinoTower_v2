using Unity.Mathematics;

namespace Domain
{
    public sealed class MinoFactory
    {
        Random _random;

        const int MinBlockCount = 3;
        const int MaxBlockCount = 10;
        
        public MinoFactory(int seed)
        {
            _random = new Random((uint)seed);
        }

        public Mino CreateRandom()
        {
            var blockCount = _random.NextInt(MinBlockCount, MaxBlockCount);
            return new Mino(blockCount, _random);
        }
    }
}