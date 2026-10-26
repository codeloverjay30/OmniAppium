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
    public class Rectangle
    {
        /// <summary>
        /// 矩形區域的左上角座標，指定了矩形的起始位置。
        /// </summary>
        public Point LeftTop { get; set; }
        /// <summary>
        /// 矩形區域的寬度，從左到右的距離。
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 矩形區域的高度，從上到下的距離。
        /// </summary>
        public int Height { get; set; }
    }
}