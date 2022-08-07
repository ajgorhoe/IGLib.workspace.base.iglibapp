using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Services.Maps;
using System.Security.Cryptography;

namespace IG.Crypto
{


    public interface IHashCalculator
    {

    }

    public class HashConst
    {

        public static string MD5Hash { get; } = HashAlgorithmName.MD5.Name;

        public static string SHA1Hash { get; } = HashAlgorithmName.SHA1.Name;

        public static string SHA256Hash { get; } = HashAlgorithmName.SHA256.Name;

        public static string SHA384Hash { get; } = HashAlgorithmName.SHA384.Name;

        public static string SHA512Hash { get; } = HashAlgorithmName.SHA512.Name;

        public static string HashToHexString(Byte[] bytes, bool capitalize=false)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes), "Cannot convert null byte array to hexadecimal string.");
            string ret = BitConverter.ToString(bytes).Replace("-", "");
            if (capitalize)
                return ret.ToUpper();
            else return ret.ToLower();
        }

    }

    public class HashResults: HashConst
    {
        
        private IDictionary<string, string> Hashes { get; } = new Dictionary<string, string>();

        public void AddHash(string hashName, string hashValue)
        {
            if (string.IsNullOrEmpty(hashName))
                throw new ArgumentException("Name of the hash function cannot be null or empty string.");
            if (string.IsNullOrEmpty(hashValue))
                throw new ArgumentException("Hash function value cannot be null or empty string.");

        }

    }

    public class HashCalcultorBase : HashConst, IHashCalculator
    {

        private Dictionary<string, HashAlgorithm> HashAlgorithms = new Dictionary<string, HashAlgorithm>();

        public virtual void SetHashAlgorithm(string hashName, HashAlgorithm algorithm, bool allowResetting = false)
        {
            if (allowResetting && HashAlgorithms.ContainsKey(hashName))
                throw new ArgumentException("Modifying hash algorithm is not allowed.",nameof(hashName));
            HashAlgorithms[hashName] = algorithm;
        }

        public static void InitStandardHashAlgorithms(HashCalcultorBase hashCalculator)
        {
            hashCalculator.SetHashAlgorithm(MD5Hash, MD5.Create());
            hashCalculator.SetHashAlgorithm(SHA1Hash, System.Security.Cryptography.SHA1.Create());
            hashCalculator.SetHashAlgorithm(SHA256Hash, System.Security.Cryptography.SHA256.Create());
            hashCalculator.SetHashAlgorithm(SHA384Hash, System.Security.Cryptography.SHA384.Create());
            hashCalculator.SetHashAlgorithm(SHA512Hash, System.Security.Cryptography.SHA512.Create());
        }


        public virtual HashAlgorithm GetHashAlgorithm(string hashName)
        {
            if (!HashAlgorithms.ContainsKey(hashName))
                throw new ArgumentException($"Do not have hash computation algorithm corresponding to hash name {hashName}", hashName);
            return HashAlgorithms[hashName];
        }
        
        

        public virtual byte[] CalculateHash(string hashName, Byte[] buffer)
        {
            return GetHashAlgorithm(hashName).ComputeHash(buffer);
        }

        public virtual string CalculateHashString(string hashName, Byte[] buffer)
        {
            return HashToHexString(CalculateHash(hashName, buffer));
        }



    }






}