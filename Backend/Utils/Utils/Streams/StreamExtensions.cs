using System.Buffers;

namespace Utils.Streams;

public static class StreamExtensions
{
    /// <summary>
    /// It's like <see cref="Stream.CopyToAsync(Stream, CancellationToken)"/> but also counts the copied bytes.
    /// </summary>
    public static async Task<int> CopyToAndCountAsync(
        this Stream source, Stream destination,
        CancellationToken cancellationToken = default
    )
    {
        var buffer = ArrayPool<byte>.Shared.Rent(1024 * 8);
        try
        {
            var totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination
                    .WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead), cancellationToken)
                    .ConfigureAwait(false);
                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// Builds a read-only stream, that contains the <see cref="str"/>
    /// </summary>
    public static Stream GetReadOnlyStream(this string str)
    {
        // TODO: Try find a final buffer length for MemoryStream before new instance creation
        // to avoid memory reallocation.
        
        // TODO: Find a way without copying the string.
        // Something with Span and Memory.
        
        // I took the code from this answer:
        // https://stackoverflow.com/a/1879470
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(str);
        writer.Flush();
        stream.Position = 0;

        // Creating new instance is needed to make stream read-only.
        // Old stream isn't disposed, because it is not necessary:
        // https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream?view=net-6.0#remarks
        return new MemoryStream(stream.GetBuffer(), false);
    }
}