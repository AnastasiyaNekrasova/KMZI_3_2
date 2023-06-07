using System;
using System.Numerics;

namespace lab8
{
    class BBSGenerator
    {
        private BigInteger p;
        private BigInteger q;
        private BigInteger n;
        private BigInteger seed;

        public BBSGenerator(BigInteger p, BigInteger q, BigInteger seed)
        {
            this.p = p;
            this.q = q;
            this.n = p * q;
            this.seed = seed;
        }

        public int GetNext()
        {
            BigInteger x = (seed * seed) % n;
            seed = x;
            string binaryNumber = Convert.ToString((int)(x % 2));
            for (int i = 1; i < 8; i++)
            {
                x = (x * x) % n;
                binaryNumber += Convert.ToString((int)(x % 2));
            }
            int decimalNumber = Convert.ToInt32(binaryNumber);
            int sequenceNumber = decimalNumber % 256;
            return sequenceNumber;
        }

    }

    static class BigIntegerExtensions
    {
        public static BigInteger Random(BigInteger n)
        {
            Random random = new Random();
            byte[] bytes = n.ToByteArray();
            random.NextBytes(bytes);
            return new BigInteger(bytes);
        }
    }

}

class BBSGenerator
{
    private BigInteger p;
    private BigInteger q;
    private BigInteger n;
    private BigInteger seed;

    public BBSGenerator(BigInteger p, BigInteger q, BigInteger seed)
    {
        this.p = p;
        this.q = q;
        this.n = p * q;
        this.seed = seed;
    }

    public int GetNext()
    {
        BigInteger x = (seed * seed) % n;
        seed = x;
        string binaryNumber = Convert.ToString((int)(x % 2));
        for (int i = 1; i < 8; i++)
        {
            x = (x * x) % n;
            binaryNumber += Convert.ToString((int)(x % 2));
        }
        int decimalNumber = Convert.ToInt32(binaryNumber);
        int sequenceNumber = decimalNumber % 256;
        return sequenceNumber;
    }

}

static class BigIntegerExtensions
{
    public static BigInteger Random(BigInteger n)
    {
        Random random = new Random();
        byte[] bytes = n.ToByteArray();
        random.NextBytes(bytes);
        return new BigInteger(bytes);
    }
}

