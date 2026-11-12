using System;
using System.Collections.Generic;
using CoordinateUtilityServices;
using FluentAssertions;
using OpenCvSharp;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59ProductionVisualRegressionTests
{
    [Theory]
    [InlineData(0, true)]
    [InlineData(10, false)]
    [InlineData(11, false)]
    [InlineData(14, false)]
    public void ReviewedVisualCrop_ShouldAcceptOnlyWhiteExclamation(int index, bool expected)
    {
        byte[] png = Convert.FromBase64String(index switch
        {
            0 => BadgeExclamation,
            10 => BadgeNumeric,
            11 => DrumOne,
            14 => DrumTwo,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        });
        using Mat source = Cv2.ImDecode(png, ImreadModes.Color);
        var match = new NotificationTemplateMatch(0, 0, source.Width, source.Height, 0.99);

        NotificationExclamationVisualGate.IsExclamation(source, match).Should().Be(expected);
    }

    [Fact]
    public void IconOwner_ShouldRequireExistingOcrIdentity()
    {
        var badge = new RedDotEvidence(Rectangle.FromXYWH(90, 10, 20, 20), 0.98, []);
        var targets = new[]
        {
            new OcrGroundedTarget("a", "活動", Rectangle.FromXYWH(40, 170, 50, 24))
        };
        var icons = new[]
        {
            new NotificationIconTarget("a", Rectangle.FromXYWH(20, 20, 85, 100)),
            new NotificationIconTarget("missing", Rectangle.FromXYWH(20, 20, 85, 100))
        };
        var options = new NotificationAssociationOptions { EnableIconAboveLabel = false };

        var owners = NotificationAssociationGeometry.ResolveViaIcons(badge, targets, icons, options);

        owners.Should().ContainSingle().Which.Should().Be("a");
    }

    [Fact]
    public void IconOwner_ShouldFailClosedForCompetingIcons()
    {
        var badge = new RedDotEvidence(Rectangle.FromXYWH(90, 10, 20, 20), 0.98, []);
        var targets = new[]
        {
            new OcrGroundedTarget("a", "活動", Rectangle.FromXYWH(20, 180, 50, 24)),
            new OcrGroundedTarget("b", "任務", Rectangle.FromXYWH(100, 180, 50, 24))
        };
        var icons = new[]
        {
            new NotificationIconTarget("a", Rectangle.FromXYWH(20, 20, 85, 100)),
            new NotificationIconTarget("b", Rectangle.FromXYWH(25, 20, 85, 100))
        };
        var options = new NotificationAssociationOptions { EnableIconAboveLabel = false };

        var owners = NotificationAssociationGeometry.ResolveViaIcons(badge, targets, icons, options);

        owners.Should().BeEquivalentTo(new[] { "a", "b" });
    }

    [Fact]
    public void InvalidIconTargets_ShouldNotCreateAnOwner()
    {
        var badge = new RedDotEvidence(Rectangle.FromXYWH(90, 10, 20, 20), 0.98, []);
        var targets = new[]
        {
            new OcrGroundedTarget("a", "活動", Rectangle.FromXYWH(20, 180, 50, 24))
        };
        var icons = new[]
        {
            new NotificationIconTarget("a", Rectangle.FromXYWH(-10, -10, 0, 0))
        };
        var options = new NotificationAssociationOptions { EnableIconAboveLabel = false };

        NotificationAssociationGeometry.ResolveViaIcons(badge, targets, icons, options)
            .Should().BeEmpty();
    }

    private const string BadgeExclamation = "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";
    private const string BadgeNumeric = "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";
    private const string DrumOne = "iVBORw0KGgoAAAANSUhEUgAAABkAAAAaCAIAAADNH2CaAAAEUUlEQVR4nGWUPW9lVxWGn/Wudc699rU9djxxJiJkCqRA0gyIIIGgSQMSChIFdBQ0SBT8AAqqCAoqREWDaGjSIJEWiV9AgYRElAyBApQAQ2Ayju37cc5ei2LbGUZsHR1tHZ397PX1vvadlz5559H7zzx1e0+6bO32OO6frICQrcKBKZO9vYVrP8JN88WHy/A6EnD/Pxdv/Pm9F04Ovv/6Ly7m2X747DHU8e07Tw0DANx++nCQRmmQZAaMB/s8uSo2H+3feOc94Ls/+UGY7z93enJytC/XED4Osdwbw+WhcJlLLpMEGCD65vIadPvo6MXT/OXV5rWf/TrOjg9Xq8U4hknhivAhFK4YJA+F+ieqn80qgIyDmwh58eTWZ/716Hd/fdde/9y9g9VyMQ4dFOGrZcTgY0cOQQiPDgKuWeYdpQ/Wh2MAr/zqN9EyMwvooMUYEa7+DK6QQk3XBwHrrLwOa5p3jAF8/uNnMYRHaIwI92GIcDG4hmAMXISQermeWHmdM0MPmS89/2wc7C/3FuO4iMUY4ZKkITSEhcyF/oclycDAtJu2HeG3VlxNwKsv3A18AFlCFg6OCTNMMkkmM5mpZOlKJEMwHiwB5tY2u3NlbnbTwyl69GZghksud0nXCHOZyaRyMw8kgSAyaS3NSkqPHHJhHmaogwy5NIR7WJ8pSSYkXLgTQ59dQZA5VWtZLgu18kPrzTYzM0kYpl6RHin0q9zlUeGYycyAnB/3wY2my0eEzMKQTCKEPdErw5AZLusa6OVPWtJaVktAWBgnx4reqJCFIXvc/KyuFxJzkyLkjgkKVUJmZWs3g2dATNWQQ+4aTPOooZLrZKksRELm3JPKzISaLi8x2BtW8I+Hlwdmdb6JzJpbTZnumiGo/5tLsrCWmTe4SrZzL9Su/7GbgciWu5bMuYzyIntaJaq/i6LmLLtRTSWZ1Q8buwKo3QQeWTW1pqa55ZCZmTmlYs4SIIIirSiEZSUJVbRskMZmO7eqeTdvzQLIuc1VuOHGpBZZiRAtYcaVE0qyh0l1q5ipddnV+VWDeWr/tgxgnpq1rCFwx9uuiESkGkHPciJVzW5cDMFUTNT6YgPcOV09mCoebLbH47CrOi1s16Zd0+jZVZ2aZknyWa0aN+ZVBRfrsYuPAnbi9+8+iD+dX7x0fPT0ctzdeNKwnU2SK1zqnlPq2q8iKxMWTzb6nb+8/9uzw/jDw/O7q/3nV8t1yykTWK1nueRKl0KSGJRFQmXOkEm73H4EWl9NP3777dPTl+Ofm+3frtbPrfZCNoHMYtoppVSklJK0nUVBy2xFZWZN68d6/NGbb776jS/e+8or9unjo2f2Fl/+2Nnd1f4ojS4frlUleTcxUlRlS6oyi6zNZgY++4VP/PT+W4cnx1//9tdWR2fRqv74wYefunVwZ7kARhfbgiqjyEJFpdZZVUVWZVUWL3/1HlP+/P5b4xjf+t43N5t5d/73/wJ1NRfVrKT7hQAAAABJRU5ErkJggg==";
    private const string DrumTwo = "iVBORw0KGgoAAAANSUhEUgAAABkAAAAaCAIAAADNH2CaAAADnUlEQVR4nG2UMa8lSQ2FP5/y7fdmZjfZBZGQEm9GsBkiICBA/BN+ACm/i9+AIAKJCIkEzcKgfffebvuYoO/bnVlhldQld9XxsX1c8Yff/+6Pf/rzr1Jf5frpEq/2ZhtyxeWCHk7nok3b1bSBf/7bXz4/efj7h//+5f038Ztf/+Lnh3/7xTs+te+xcnHJuORUAWPmvrtNz9qfvjv/r9stf3n4a/6P6Sdf/sATlwRiGBFzAhgo+/1937vz6+cAqg8vKddnbza2PJ6eKldKkgT2YDMPUK8Me8r82HVU3Wtj5175+B3Bisi0pKUMsgcGDZIUxCIEYNOmmAVmpWAjpNzz/X0HfvT2aWVy0aRAiwAEIjjJrVhr84l1v2kYKBwjicuWYz94xQqWJBEKEUISS+SKteKS2hJLgGsqcWlGI5dtdxvIL542QCszpCCXYiUSARHESU+EOBWjZBWjmcEQxLmkBy8FEaxQBLH4TlOPasLAOp0mQiMhAwrNEjBD9tLT0zYjDQoYwl7LDhhwMAuYMpsxYLaM3T3KFoErry6P0zP2CPwxkbP9A/bYUYWEPyEL9DDGJkBBeuYHJwbi/IxBzHkD92s4ewaGAhvshIK03fYaMzrV+D34gI09RWjmvkeAhO0uuhnOtNdYdpqZmekh8eCxRow56zXCfrBsExHSjGmfYSAI6BPLMzYe2x7bimGGiI+ozeuGmAhmmLGNH7oZT/Sk7rrd6/JsY5cLgwPF63QJIWj7xD5LMPZQNu056pjYZ/LW/bzW9JxASVlYERN8RE2SH9RAOrtBmzLt6nI5r9XPa1XVebNJwtCPMYIREYzMYMOYeMU6q9J2U3bWuOyqlgIIKaocxBKYSi0GYjGGOaVnxmNm3GXabrdJ4NrO4wDyAhEzmmGlbGnVGgHReqR85m3A1Z6jfFQddncCt643hxRy4LN/U5BhjxWPLpjRY2NOhdfYR/V1r/JAvru+APXmXdbRboeY4eDz7LmscfaxP96lT8YMYN1r2hqD6+j827cvP3v39no/5pIXQlVyADdPdIcqlxR69BMGaoaIFKt7L8NsS0d1Xu1/3O5vM0xs0tN+nDG1oQopuKT0GOuCHm4zoVhrPVVXu3uuUy/3I785CvBdKAxW5PmY7bVdVsb6sB8rYgtWxDHUzNUjRS5HeTx79/V27O3867cvX33+2X9errK5lLfMXCl54rBZ5SEVaMHs7cNz61bEloqQIuz5cLsr4n+UdNCH2s1dEgAAAABJRU5ErkJggg==";
}
