using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Exploration;

public sealed class Issue59VisualGroundTruthRegressionTests
{
    private const string FixturePath = @"C:\fixtures\issue59-ground-truth.json";

    private static JsonDocument LoadFixture()
    {
        var fileSystem = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                [FixturePath] = new MockFileData(FixtureJson)
            });

        return JsonDocument.Parse(fileSystem.File.ReadAllText(FixturePath));
    }

    [Fact]
    public void ReviewedFixture_ShouldContainTwelveDistinctCandidates()
    {
        using var document = LoadFixture();
        var records = document.RootElement.GetProperty("records").EnumerateArray().ToArray();
        records.Should().HaveCount(12);
        records.Select(x => x.GetProperty("badgeIndex").GetInt32()).Should().OnlyHaveUniqueItems();
        records.Select(x => x.GetProperty("badgeIndex").GetInt32())
            .Should().BeEquivalentTo(new[] { 0, 2, 4, 5, 7, 8, 9, 10, 11, 12, 13, 14 });
    }

    [Fact]
    public void ReviewedFixture_ShouldKeepVisualCategoriesDistinct()
    {
        using var document = LoadFixture();
        var records = document.RootElement.GetProperty("records").EnumerateArray().ToArray();
        records.Count(x => x.GetProperty("candidateVisualType").GetString() == "Notification").Should().Be(7);
        records.Count(x => x.GetProperty("candidateVisualType").GetString() == "NumericBadge").Should().Be(1);
        records.Count(x => x.GetProperty("candidateVisualType").GetString() == "NonNotification").Should().Be(4);
        records.Where(x => new[] { 11, 12, 13, 14 }.Contains(x.GetProperty("badgeIndex").GetInt32()))
            .Should().OnlyContain(x => x.GetProperty("candidateVisualType").GetString() == "NonNotification");
    }

    [Fact]
    public void ReviewedFixture_ShouldNeverAuthorizeClicks()
    {
        using var document = LoadFixture();
        document.RootElement.GetProperty("records").EnumerateArray()
            .Should().OnlyContain(x => !x.GetProperty("allowAutomaticClick").GetBoolean());
    }

    [Fact]
    public void ReviewedFixture_ShouldKeepBoundsWithinLandscapeScreenshot()
    {
        using var document = LoadFixture();
        var root = document.RootElement;
        var width = root.GetProperty("observationResolution").GetProperty("width").GetInt32();
        var height = root.GetProperty("observationResolution").GetProperty("height").GetInt32();
        width.Should().Be(2340);
        height.Should().Be(1080);

        foreach (var record in root.GetProperty("records").EnumerateArray())
        {
            var bounds = record.GetProperty("iconBounds");
            if (bounds.ValueKind == JsonValueKind.Null)
                continue;

            var x = bounds.GetProperty("x").GetInt32();
            var y = bounds.GetProperty("y").GetInt32();
            var w = bounds.GetProperty("width").GetInt32();
            var h = bounds.GetProperty("height").GetInt32();
            x.Should().BeGreaterThanOrEqualTo(0);
            y.Should().BeGreaterThanOrEqualTo(0);
            w.Should().BeGreaterThan(0);
            h.Should().BeGreaterThan(0);
            (x + w).Should().BeLessThanOrEqualTo(width);
            (y + h).Should().BeLessThanOrEqualTo(height);
        }
    }

    private const string FixtureJson = """
{
  "observationResolution": {
    "width": 2340,
    "height": 1080
  },
  "source": "20261108-222803-fcd76eb8b3744fe493c6fb42f92ff23b.zip/original.png",
  "records": [
    {
      "badgeIndex": 0,
      "badgeCenter": {
        "x": 237.5,
        "y": 323.5
      },
      "candidateVisualType": "Notification",
      "ownerVisualName": "主城",
      "iconBounds": {
        "x": 125,
        "y": 370,
        "width": 125,
        "height": 155
      },
      "ocrLabelStatus": "MissingFromOcrTargets",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Red circle white exclamation on 主城 icon",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 2,
      "badgeCenter": {
        "x": 1923.5,
        "y": 100.5
      },
      "candidateVisualType": "Notification",
      "ownerVisualName": "強者之路",
      "iconBounds": {
        "x": 1850,
        "y": 78,
        "width": 113,
        "height": 115
      },
      "ocrLabelStatus": "MissingFromOcrTargets",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Top menu label visible but not in OCR targets",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 4,
      "badgeCenter": {
        "x": 459.5,
        "y": 850.5
      },
      "candidateVisualType": "Notification",
      "ownerVisualName": "天下",
      "iconBounds": {
        "x": 360,
        "y": 808,
        "width": 128,
        "height": 143
      },
      "ocrLabelStatus": "MissingFromOcrTargets",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Left bottom 天下 icon",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 5,
      "badgeCenter": {
        "x": 1673.5,
        "y": 231.5
      },
      "candidateVisualType": "Notification",
      "ownerVisualName": "探險征程",
      "iconBounds": {
        "x": 1592,
        "y": 211,
        "width": 124,
        "height": 124
      },
      "ocrLabelStatus": "MergedOrMissingFromOcrTargets",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Top menu label visible; OCR target not individually grounded",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 7,
      "badgeCenter": {
        "x": 1548.5,
        "y": 100.5
      },
      "candidateVisualType": "Notification",
      "ownerVisualName": "超值活動",
      "iconBounds": {
        "x": 1475,
        "y": 76,
        "width": 113,
        "height": 117
      },
      "ocrLabelStatus": "MissingFromOcrTargets",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Top menu label visible",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 8,
      "badgeCenter": {
        "x": 941.5,
        "y": 1005.5
      },
      "candidateVisualType": "Notification",
      "ownerVisualName": "好友/社交",
      "iconBounds": {
        "x": 894,
        "y": 978,
        "width": 86,
        "height": 79
      },
      "ocrLabelStatus": "NoReadableLabel",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Social icon with red exclamation, no text label",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 9,
      "badgeCenter": {
        "x": 233.5,
        "y": 51.5
      },
      "candidateVisualType": "Notification",
      "ownerVisualName": "玩家頭像/個人資訊",
      "iconBounds": {
        "x": 105,
        "y": 40,
        "width": 160,
        "height": 201
      },
      "ocrLabelStatus": "NoReliableIndividualLabel",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Red exclamation over top-left player title/profile",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 10,
      "badgeCenter": {
        "x": 830.5,
        "y": 1004.5
      },
      "candidateVisualType": "NumericBadge",
      "ownerVisualName": "郵件",
      "iconBounds": {
        "x": 760,
        "y": 984,
        "width": 95,
        "height": 72
      },
      "ocrLabelStatus": "NoReadableLabel",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Red circle with numeric 22, not white exclamation",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 11,
      "badgeCenter": {
        "x": 2055.5,
        "y": 894
      },
      "candidateVisualType": "NonNotification",
      "ownerVisualName": "群英會戰鼓",
      "iconBounds": {
        "x": 1960,
        "y": 831,
        "width": 205,
        "height": 180
      },
      "ocrLabelStatus": "NotApplicable",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Red drum head, no white exclamation",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 12,
      "badgeCenter": {
        "x": 546,
        "y": 1008.5
      },
      "candidateVisualType": "NonNotification",
      "ownerVisualName": null,
      "iconBounds": null,
      "ocrLabelStatus": "NotApplicable",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Background behind chat line; template false positive",
      "iconBoundsPrecision": "NotApplicable",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 13,
      "badgeCenter": {
        "x": 1200.5,
        "y": 1042.5
      },
      "candidateVisualType": "NonNotification",
      "ownerVisualName": null,
      "iconBounds": null,
      "ocrLabelStatus": "NotApplicable",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Bottom background; template false positive",
      "iconBoundsPrecision": "NotApplicable",
      "allowAutomaticClick": false
    },
    {
      "badgeIndex": 14,
      "badgeCenter": {
        "x": 2083.5,
        "y": 929
      },
      "candidateVisualType": "NonNotification",
      "ownerVisualName": "群英會戰鼓",
      "iconBounds": {
        "x": 1960,
        "y": 831,
        "width": 205,
        "height": 180
      },
      "ocrLabelStatus": "NotApplicable",
      "manualGroundTruthStatus": "ReviewedFromOriginalScreenshot",
      "reviewNote": "Second red drum head, no white exclamation",
      "iconBoundsPrecision": "ApproximateManualROI",
      "allowAutomaticClick": false
    }
  ]
}
""";
}
