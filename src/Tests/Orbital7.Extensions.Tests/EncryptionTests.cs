using Orbital7.Extensions.Encryption;

namespace Orbital7.Extensions.Tests;

public class EncryptionTests
{
    [Fact]
    public void ComputeHmacSha256Hash()
    {
        // NOTE: This test really doesn't do anything other than verify
        // in conjunction with EncryptionHelper.ComputeHmacSha256Hash
        // that .NET9+'s Convert.ToHexStringLower() == Convert.ToHexString().ToLower().

        var salt = GuidFactory.NextShortString();
        var input1 = GuidFactory.NextShortString();
        var input2 = GuidFactory.NextShortString().ToUpper();
        var input3 = GuidFactory.NextShortString().ToLower();

        var outputBase64 = EncryptionHelper.ComputeHmacSha256Hash(input1, salt, OutputEncodingFormat.Base64);

        var outputHex1 = EncryptionHelper.ComputeHmacSha256Hash(input1, salt, OutputEncodingFormat.Hex);
        var outputHexLower1 = EncryptionHelper.ComputeHmacSha256Hash(input1, salt, OutputEncodingFormat.HexLower);

        var outputHex2 = EncryptionHelper.ComputeHmacSha256Hash(input2, salt, OutputEncodingFormat.Hex);
        var outputHexLower2 = EncryptionHelper.ComputeHmacSha256Hash(input2, salt, OutputEncodingFormat.HexLower);

        var outputHex3 = EncryptionHelper.ComputeHmacSha256Hash(input3, salt, OutputEncodingFormat.Hex);
        var outputHexLower3 = EncryptionHelper.ComputeHmacSha256Hash(input3, salt, OutputEncodingFormat.HexLower);

        Assert.Equal(outputHexLower1, outputHex1.ToLower());
        Assert.Equal(outputHexLower2, outputHex2.ToLower());
        Assert.Equal(outputHexLower3, outputHex3.ToLower());
    }
}
