using CoordinateRectangle = CoordinateUtilityServices.Rectangle;
using LegacyRectangle = OmniAppium.ConfigUtilityServices.Models.Rectangle;

namespace OmniAppium.ConfigUtilityServices.Extensions;

/// <summary>
/// Provides validated conversions from legacy configuration
/// rectangles to coordinate-domain rectangles.
/// </summary>
public static class CoordinateRectangleMigrationExtensions
{
    /// <summary>
    /// Converts a legacy configuration rectangle into a validated
    /// coordinate-domain rectangle.
    /// </summary>
    /// <param name="source">The legacy rectangle.</param>
    /// <returns>The equivalent coordinate-domain rectangle.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the rectangle or its origin is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the width or height is negative.
    /// </exception>
    public static CoordinateRectangle ToCoordinateRectangle(
        this LegacyRectangle source)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (source.LeftTop is null)
        {
            throw new ArgumentException(
                "The rectangle origin cannot be null.",
                nameof(source));
        }

        if (source.Width < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(source),
                source.Width,
                "Rectangle width cannot be negative.");
        }

        if (source.Height < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(source),
                source.Height,
                "Rectangle height cannot be negative.");
        }

        return CoordinateRectangle.FromXYWH(
            source.LeftTop.X,
            source.LeftTop.Y,
            source.Width,
            source.Height);
    }
}