namespace OmniAppium.EngineUtilityServices.Services.Exploration;

/// <summary>
/// Classifies OCR text as a possible primary navigation label or non-owning secondary information.
/// </summary>
public static class NotificationOcrLabelClassifier
{
    /// <summary>
    /// Returns whether OCR text may independently identify a notification-owning UI feature.
    /// Unknown text remains eligible; this method is not a semantic authorization decision.
    /// </summary>
    public static bool IsPrimaryLabelCandidate(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        ReadOnlySpan<char> value = text.AsSpan().Trim();
        if (value.Length > 64)
            return false;

        // Remove OCR-introduced whitespace without altering the original OCR evidence.
        Span<char> compact = stackalloc char[value.Length];
        int count = 0;
        foreach (char c in value)
        {
            if (!char.IsWhiteSpace(c))
                compact[count++] = c;
        }

        ReadOnlySpan<char> normalized = compact[..count];
        if (normalized.IsEmpty)
            return false;

        // Timers, countdowns and numerical badges are secondary evidence, never owners.
        if (IsClock(normalized) || IsNumberOnly(normalized) ||
            normalized.StartsWith("剩餘時間".AsSpan(), StringComparison.Ordinal) ||
            normalized.StartsWith("倒數".AsSpan(), StringComparison.Ordinal) ||
            normalized.StartsWith("剩餘".AsSpan(), StringComparison.Ordinal))
            return false;

        return true;
    }

    /// <summary>
    /// Identifies a clock-like value with two or three numeric components separated by colons.
    /// </summary>
    private static bool IsClock(ReadOnlySpan<char> value)
    {
        int segments = 1;
        int digits = 0;
        foreach (char c in value)
        {
            if (c is ':' or '：')
            {
                if (digits is < 1 or > 3)
                    return false;
                segments++;
                digits = 0;
                continue;
            }
            if (!char.IsAsciiDigit(c))
                return false;
            digits++;
            if (digits > 3)
                return false;
        }
        return segments is 2 or 3 && digits is >= 1 and <= 3;
    }

    /// <summary>
    /// Identifies OCR values containing only digits and common numeric separators.
    /// </summary>
    private static bool IsNumberOnly(ReadOnlySpan<char> value)
    {
        bool hasDigit = false;
        foreach (char c in value)
        {
            if (char.IsAsciiDigit(c))
            {
                hasDigit = true;
                continue;
            }
            if (c is not ('.' or ',' or '，' or '+' or '-' or '%' or '％'))
                return false;
        }
        return hasDigit;
    }
}
