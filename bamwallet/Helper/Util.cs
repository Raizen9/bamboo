﻿// CypherNetwork BAMWallet by Matthew Hellyer is licensed under CC BY-NC-ND 4.0.
// To view a copy of this license, visit https://creativecommons.org/licenses/by-nc-nd/4.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using LiteDB;
using BAMWallet.Extensions;
using Blake3;

namespace BAMWallet.Helper
{
    public static class Util
    {
        public static string AppDomainDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static byte[] RandomDealerIdentity()
        {
            return Hasher.Hash(Guid.NewGuid().ToByteArray()).HexToByte();
        }

        public static string WalletPath(string id)
        {
            var wallets = Path.Combine(Path.GetDirectoryName(AppDomainDirectory()), "wallets");
            var wallet = Path.Combine(wallets, $"{id}.db");

            if (Directory.Exists(wallets)) return wallet;
            try
            {
                Directory.CreateDirectory(wallets);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return wallet;
        }

        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static LiteRepository LiteRepositoryFactory(string walletName, SecureString passphrase)
        {
            var connectionString = new ConnectionString
            {
                Filename = WalletPath(walletName),
                Password = passphrase.FromSecureString(),
                Connection = ConnectionType.Shared
            };


            var x = new LiteRepository(connectionString);
            return x;
        }

        public static LiteRepository LiteRepositoryAppSettingsFactory()
        {
            var connectionString = new ConnectionString
            {
                Filename = WalletPath("appsettings"),
                Connection = ConnectionType.Shared
            };

            var x = new LiteRepository(connectionString);
            return x;
        }

        public static ulong Sum(IEnumerable<ulong> source)
        {
            return source.Aggregate(0UL, (current, number) => current + number);
        }

        public static byte[] StreamToArray(Stream input)
        {
            using var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        public static BigInteger Mod(BigInteger a, BigInteger n)
        {
            var result = a % n;
            if (result < 0 && n > 0 || result > 0 && n < 0)
            {
                result += n;
            }

            return result;
        }

        public static DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetAdjustedTime()
        {
            return GetUtcNow().Add(TimeSpan.Zero);
        }

        public static long GetAdjustedTimeAsUnixTimestamp()
        {
            return new DateTimeOffset(GetAdjustedTime()).ToUnixTimeSeconds();
        }

        static DateTimeOffset unixRef = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static uint DateTimeToUnixTime(DateTimeOffset dt)
        {
            return (uint)DateTimeToUnixTimeLong(dt);
        }

        internal static ulong DateTimeToUnixTimeLong(DateTimeOffset dt)
        {
            dt = dt.ToUniversalTime();
            if (dt < unixRef)
                throw new ArgumentOutOfRangeException("The supplied datetime can't be expressed in unix timestamp");
            var result = (dt - unixRef).TotalSeconds;
            if (result > UInt32.MaxValue)
                throw new ArgumentOutOfRangeException("The supplied datetime can't be expressed in unix timestamp");
            return (ulong)result;
        }

        public static DateTimeOffset UnixTimeToDateTime(uint timestamp)
        {
            var span = TimeSpan.FromSeconds(timestamp);
            return unixRef + span;
        }

        public static DateTimeOffset UnixTimeToDateTime(ulong timestamp)
        {
            var span = TimeSpan.FromSeconds(timestamp);
            return unixRef + span;
        }

        public static DateTimeOffset UnixTimeToDateTime(long timestamp)
        {
            var span = TimeSpan.FromSeconds(timestamp);
            return unixRef + span;
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            var ret = new byte[arrays.Sum(x => x.Length)];
            var offset = 0;
            foreach (var data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }

            return ret;
        }

        public static OSPlatform GetOperatingSystemPlatform()
        {
            foreach (var platform in new[] { OSPlatform.Linux, OSPlatform.FreeBSD, OSPlatform.OSX, OSPlatform.Windows })
                if (RuntimeInformation.IsOSPlatform(platform))
                    return platform;

            throw new NotSupportedException();
        }
    }
}
