/// <summary>
/// Represents the OCR marker evaluation result for a single screen snapshot.
/// </summary>
/// <param name="MatchedTexts">
/// The expected OCR markers found in the current snapshot.
/// </param>
/// <param name="MissingTexts">
/// The expected OCR markers not found in the current snapshot.
/// </param>
public sealed record OcrVerificationAttemptResult(
    IReadOnlyList<string> MatchedTexts,
    IReadOnlyList<string> MissingTexts)
{
    /// <summary>
    /// Gets a value indicating whether all expected OCR markers were found.
/// </summary>
    public bool AllTextsFound =>
        MissingTexts.Count == 0;
}