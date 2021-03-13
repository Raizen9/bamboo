﻿// BAMWallet by Matthew Hellyer is licensed under CC BY-NC-ND 4.0. 
// To view a copy of this license, visit https://creativecommons.org/licenses/by-nc-nd/4.0

using FlatSharp.Attributes;

namespace BAMWallet.Model
{
    [FlatBufferTable]
    public class PayloadFlatBuffer : object
    {
        [FlatBufferItem(1)]
        public virtual ulong Node { get; set; }
        [FlatBufferItem(2)]
        public virtual byte[] Payload { get; set; }
        [FlatBufferItem(3)]
        public virtual byte[] PublicKey { get; set; }
        [FlatBufferItem(4)]
        public virtual byte[] Signature { get; set; }
        [FlatBufferItem(5)]
        public virtual string Message { get; set; }
        [FlatBufferItem(6)]
        public virtual bool Error { get; set; }
    }
}