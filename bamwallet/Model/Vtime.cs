// CypherNetwork BAMWallet by Matthew Hellyer is licensed under CC BY-NC-ND 4.0.
// To view a copy of this license, visit https://creativecommons.org/licenses/by-nc-nd/4.0

using MessagePack;

namespace BAMWallet.Model
{
    [MessagePackObject]
    public record Vtime
    {
        [Key(0)] public long W { get; set; }
        [Key(1)] public byte[] M { get; set; }
        [Key(2)] public byte[] N { get; set; }
        [Key(3)] public int I { get; set; }
        [Key(4)] public byte[] S { get; set; }
        [Key(5)] public long L { get; set; }
        [Key(6)] public int T { get; set; }
        [Key(7)] public int K { get; set; }
    }
}