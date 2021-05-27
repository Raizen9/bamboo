﻿// BAMWallet by Matthew Hellyer is licensed under CC BY-NC-ND 4.0. 
// To view a copy of this license, visit https://creativecommons.org/licenses/by-nc-nd/4.0

using System;
using MessagePack;

namespace BAMWallet.Model
{
    [MessagePackObject]
    public class WalletTransactionMessage
    {
        [Key(0)] public ulong Amount { get; set; }
        [Key(1)] public byte[] Blind { get; set; }
        [Key(2)] public string Memo { get; set; }
        [Key(3)] public DateTime Date { get; set; }
        [Key(4)] public Vout Output { get; set; }
    }
}
