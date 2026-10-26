using System.ComponentModel;
using CustomDataAnnotations.Maintenance;

namespace OmniAppium.ConfigUtilityServices.Models
{
    /// <summary>
    /// Represents a deprecated rectangle configuration model
    /// retained for backward compatibility.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(
    "Use CoordinateUtilityServices.Rectangle instead. " +
    "This type is retained for legacy configuration compatibility.")]
    [TechnicalDebt(
    CategoryType.ObsoleteApi,
    "Use CoordinateUtilityServices.Rectangle instead.")]
    public class Point
    {
        /// <summary>
        /// X座標，水平位置。
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y座標，垂直位置。
        /// </summary>
        public int Y { get; set; }
    }
}