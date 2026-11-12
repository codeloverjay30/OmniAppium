using OpenCvSharp;
using OCRUtilityServices.Models;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OmniAppium.EngineUtilityServices.Models.Observation;
using OmniAppium.EngineUtilityServices.Services.Exploration;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.Observation;
using Microsoft.Extensions.Logging;

namespace OmniAppiumDemo.SmokeTests.Issue59;

/// <summary>
/// Runs a read-only red-dot exploration smoke test against a real Android screen.
/// </summary>
public sealed class Issue59RedDotSmokeTestRunner
{
    private readonly IAndroidScreenObservationService _observationService;
    private readonly IRedDotObservationIntegrationService _integrationService;

    private readonly ILogger<Issue59RedDotSmokeTestRunner> _logger;
    private readonly NotificationAssociationOptions _associationOptions;
    private readonly Issue59ReadOnlyOcrDiagnosticService? _readOnlyDiagnostics;
    private readonly IAndroidScreenOcrService? _regionalOcr;

    /// <summary>
    /// Initializes the smoke test runner with existing observation services.
    /// </summary>
    public Issue59RedDotSmokeTestRunner(
        IAndroidScreenObservationService observationService,
        IRedDotObservationIntegrationService integrationService,
        ILogger<Issue59RedDotSmokeTestRunner> logger,
        NotificationAssociationOptions? associationOptions = null,
        Issue59ReadOnlyOcrDiagnosticService? readOnlyDiagnostics = null,
        IAndroidScreenOcrService? regionalOcr = null)
    {
        _observationService = observationService
            ?? throw new ArgumentNullException(nameof(observationService));

        _integrationService = integrationService
            ?? throw new ArgumentNullException(nameof(integrationService));

        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        _associationOptions = associationOptions ?? new NotificationAssociationOptions();
        _associationOptions.Validate();
        _readOnlyDiagnostics = readOnlyDiagnostics;
        _regionalOcr = regionalOcr;
    }

    /// <summary>
    /// Detects independently visible icon circles from the baseline screenshot without modifying OCR evidence.
    /// </summary>
    private IReadOnlyList<NotificationIconTarget> DetectIconsReadOnly(
        IAndroidScreenObservation observation,
        IReadOnlyList<OcrGroundedTarget> targets)
    {
        try
        {
            var icons = new SameFrameNotificationIconDetector().Detect(observation.ImageBytes, targets);
            _logger.LogInformation(
                "Issue 59 same-frame icon targets: {Count}", icons.Count);
            foreach (NotificationIconTarget icon in icons)
                _logger.LogInformation(
                    "Issue 59 icon target: targetId={TargetId}, x={X}, y={Y}, width={Width}, height={Height}",
                    icon.TargetId, icon.IconBounds.TopLeft.X, icon.IconBounds.TopLeft.Y,
                    icon.IconBounds.Width, icon.IconBounds.Height);
            return icons;
        }
        catch (Exception ex) when (ex is not OutOfMemoryException)
        {
            _logger.LogWarning(ex,
                "Issue 59 same-frame icon detection unavailable; retaining OCR-only fail-closed association.");
            return Array.Empty<NotificationIconTarget>();
        }
    }

    /// <summary>
    /// Inspects the limited-gift text area using the baseline frame only.
    /// Results are untrusted diagnostics and never enter ownership analysis.
    /// </summary>
    private async Task DiagnoseLimitedGiftRegionAsync(
        IAndroidScreenObservation observation,
        CancellationToken cancellationToken)
    {
        if (_regionalOcr is null)
        {
            _logger.LogWarning(
                "Issue 59 LOCAL RE-OCR UNTRUSTED skipped: OCR adapter unavailable.");
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            using Mat image = Cv2.ImDecode(
                observation.ImageBytes.ToArray(), ImreadModes.Color);
            if (image.Empty())
                throw new InvalidOperationException(
                    "Issue 59 LOCAL RE-OCR screenshot could not be decoded.");

            // Diagnostic coordinates from the reviewed 2340x1080 baseline only.
            // Never silently reinterpret them on another screen size.
            if (image.Width != 2340 || image.Height != 1080)
            {
                _logger.LogWarning(
                    "Issue 59 LOCAL RE-OCR UNTRUSTED skipped: unexpected image size {Width}x{Height}.",
                    image.Width, image.Height);
                return;
            }

            var region = CoordinateUtilityServices.Rectangle.FromXYWH(
                2030, 272, 250, 85);
            OcrResult result = await _regionalOcr.RecognizeRegionAsync(
                observation.ImageBytes, region, cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation(
                "Issue 59 LOCAL RE-OCR UNTRUSTED: lines={Count}, x={X}, y={Y}, width={Width}, height={Height}",
                result.Lines.Count, region.TopLeft.X, region.TopLeft.Y,
                region.Width, region.Height);

            foreach (OcrTextLine line in result.Lines)
            {
                if (line is null)
                    continue;
                _logger.LogInformation(
                    "Issue 59 LOCAL RE-OCR UNTRUSTED target: text={Text}, x={X}, y={Y}, width={Width}, height={Height}",
                    line.Text, line.Bounds.TopLeft.X, line.Bounds.TopLeft.Y,
                    line.Bounds.Width, line.Bounds.Height);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex) when (ex is not OutOfMemoryException)
        {
            _logger.LogWarning(ex,
                "Issue 59 LOCAL RE-OCR UNTRUSTED failed; ownership evidence unchanged.");
        }
    }

    /// <summary>
    /// Captures one Android observation and analyzes its red-dot evidence.
    /// </summary>
    /// <param name="targetFactory">
    /// Creates OCR targets from the exact observation captured by this run.
    /// Pass null to validate screenshot decoding and red-dot detection only.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the operation before analysis when requested.
    /// </param>
    /// <returns>The read-only red-dot association result.</returns>
    public async Task<RedDotAssociationResult> RunAsync(
        Func<IAndroidScreenObservation, IReadOnlyList<OcrGroundedTarget>>?
            targetFactory = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IAndroidScreenObservation observation =
            await ObserveOrDiagnoseAsync("baseline", cancellationToken)
                .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        // Read-only diagnostic: never add regional OCR lines to targetFactory output.
        await DiagnoseLimitedGiftRegionAsync(observation, cancellationToken)
            .ConfigureAwait(false);

        bool isTargetFactoryConfigured = targetFactory is not null;

        IReadOnlyList<OcrGroundedTarget> targets;

        if (!isTargetFactoryConfigured)
        {
            targets = Array.Empty<OcrGroundedTarget>();

            _logger.LogWarning(
                "Issue 59 OCR target source: NOT CONFIGURED. " +
                "Red-dot association cannot be evaluated.");
        }
        else
        {
            targets = targetFactory!(observation)
                ?? throw new InvalidOperationException(
                    "Issue 59 OCR target factory returned null.");

            if (targets.Count == 0)
            {
                _logger.LogWarning(
                    "Issue 59 OCR target source: CONFIGURED, " +
                    "but returned zero targets.");
            }
            else
            {
                _logger.LogInformation(
                    "Issue 59 OCR target source: CONFIGURED.");
            }
        }

        _logger.LogInformation(
            "Issue 59 OCR targets: {OcrTargetCount}",
            targets.Count);

        string observationId = Guid.NewGuid().ToString("N");

        // Capture independent follow-up observations; never reuse the baseline.
        var subsequentObservations = new List<IAndroidScreenObservation>(2);
        for (int index = 0; index < 2; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            subsequentObservations.Add(await ObserveOrDiagnoseAsync(
                $"follow-up-{index + 1}", cancellationToken).ConfigureAwait(false));
        }

        // Load the reviewed badge before invoking the production integration boundary.
        string templatePath = Path.Combine(AppContext.BaseDirectory,
            "Templates", "Notifications", "red-exclamation.png");
        if (!File.Exists(templatePath))
            throw new FileNotFoundException(
                "Issue 59 notification template is missing.", templatePath);
        byte[] templateBytes = await File.ReadAllBytesAsync(templatePath, cancellationToken)
            .ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();

        var input = new RedDotObservationInput(
            observationId,
            observation,
            targets)
        {
            IconTargets = DetectIconsReadOnly(observation, targets),
            NotificationTemplate = templateBytes,
            TemplateOptions = new NotificationTemplateMatchOptions(RequireExclamationGlyph: true),
            SubsequentObservations = subsequentObservations,
            FilterOptions = new NotificationEvidenceFilterOptions()
        };

        cancellationToken.ThrowIfCancellationRequested();

        RedDotAssociationResult result =
            _integrationService.Analyze(input);

        if (!string.Equals(
                result.ObservationId,
                observationId,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Red-dot smoke test observation identity mismatch.");
        }

        if (result.Nodes.Any(node =>
                !string.Equals(
                    node.ObservationId,
                    observationId,
                    StringComparison.Ordinal)))
        {
            throw new InvalidOperationException(
                "Red-dot smoke test contains nodes from another observation.");
        }

        // Report final verified-badge ownership after temporal and geometric association.
        int zeroOwners = result.Evidence.Count(b => b.CandidateNodeIds.Count == 0);
        int uniqueOwners = result.Evidence.Count(b => b.CandidateNodeIds.Distinct(StringComparer.Ordinal).Count() == 1);
        int competingOwners = result.Evidence.Count(b => b.CandidateNodeIds.Distinct(StringComparer.Ordinal).Count() > 1);
        _logger.LogInformation(
            "Issue 59 association diagnostics: noOwner={NoOwner}, uniqueOwner={UniqueOwner}, competingOwners={CompetingOwners}",
            zeroOwners, uniqueOwners, competingOwners);
        foreach (RedDotEvidence badge in result.Evidence)
        {
            _logger.LogInformation(
                "Issue 59 badge geometry: x={X}, y={Y}, width={Width}, height={Height}, candidateNodeIds={CandidateNodeIds}",
                badge.Bounds.TopLeft.X, badge.Bounds.TopLeft.Y,
                badge.Bounds.Width, badge.Bounds.Height,
                string.Join(",", badge.CandidateNodeIds));
        }

        // Diagnostics are observational only: they cannot change candidate IDs or authorize clicks.
        using (Mat diagnosticImage = Cv2.ImDecode(
            observation.ImageBytes.ToArray(), ImreadModes.Color))
        {
            if (diagnosticImage.Empty())
                throw new InvalidOperationException(
                    "Issue 59 association diagnostics could not decode the baseline screenshot.");

            int outOfFrameTargets = targets.Count(t =>
                t.Bounds.TopLeft.X < 0 || t.Bounds.TopLeft.Y < 0 ||
                t.Bounds.Width <= 0 || t.Bounds.Height <= 0 ||
                t.Bounds.TopLeft.X + t.Bounds.Width > diagnosticImage.Width ||
                t.Bounds.TopLeft.Y + t.Bounds.Height > diagnosticImage.Height);

            _logger.LogInformation(
                "Issue 59 coordinate diagnostics: imageWidth={ImageWidth}, imageHeight={ImageHeight}, outOfFrameTargets={OutOfFrameTargets}, totalTargets={TotalTargets}",
                diagnosticImage.Width, diagnosticImage.Height, outOfFrameTargets, targets.Count);

            // Diagnostic only: never feed OCR segmentation hypotheses back into ownership.
            try
            {
                var segmentationFindings = NotificationOcrSegmentationDiagnostics.Inspect(
                    result.Evidence, targets);
                _logger.LogInformation(
                    "Issue 59 OCR segmentation review: nearbyEvidence={Count}",
                    segmentationFindings.Count);
                foreach (var finding in segmentationFindings)
                {
                    _logger.LogInformation(
                        "Issue 59 OCR segmentation evidence: badgeIndex={BadgeIndex}, targetId={TargetId}, text={Text}, primaryCandidate={PrimaryCandidate}, multipleTokens={MultipleTokens}, distancePx={DistancePx:F1}",
                        finding.BadgeIndex, finding.TargetId, finding.Text,
                        finding.IsPrimaryLabelCandidate,
                        finding.ContainsMultipleWhitespaceSeparatedTokens,
                        finding.DistancePx);
                }
            }
            catch (Exception ex) when (ex is not OutOfMemoryException)
            {
                _logger.LogWarning(ex,
                    "Issue 59 OCR segmentation diagnostics unavailable; association remains unchanged.");
            }

            var badgeDiagnostics = NotificationAssociationDiagnostics.Inspect(
                result.Evidence, targets, _associationOptions,
                diagnosticImage.Width, diagnosticImage.Height);
            try
            {
                IReadOnlyList<NotificationUnownedBadgeDiagnostic> unownedDiagnostics =
                    NotificationUnownedBadgeDiagnostics.Inspect(
                        result.Evidence, targets, _associationOptions,
                        diagnosticImage.Width, diagnosticImage.Height);

                _logger.LogInformation(
                    "Issue 59 read-only unowned badge diagnostics: count={Count}",
                    unownedDiagnostics.Count);
                foreach (NotificationUnownedBadgeDiagnostic unowned in unownedDiagnostics)
                {
                    _logger.LogInformation(
                        "Issue 59 unowned badge: index={BadgeIndex}, center=({CenterX:F1},{CenterY:F1}), region={Region}, hint={Hint}, nearestOcrId={NearestOcrId}, nearestOcrText={NearestOcrText}, nearestReason={NearestReason}, dx={DeltaX}, dy={DeltaY}, distance={Distance}",
                        unowned.BadgeIndex, unowned.CenterX, unowned.CenterY,
                        unowned.Region, unowned.Hint, unowned.NearestTargetId,
                        unowned.NearestTargetText, unowned.NearestRejectionReason,
                        unowned.DeltaX, unowned.DeltaY, unowned.Distance);

                    _logger.LogInformation(
                        "Issue 59 unowned badge review: index={BadgeIndex}, visualType={VisualType}, layout={SuggestedLayout}, groundTruth={GroundTruthStatus}, manualReview={RequiresManualReview}, nearestWidthRatio={WidthRatio}, possibleMergedOcr={PossibleMergedOcr}, nearbyCount={NearbyCount}",
                        unowned.BadgeIndex, unowned.CandidateVisualType,
                        unowned.SuggestedLayout, unowned.GroundTruthStatus,
                        unowned.RequiresManualReview, unowned.OcrTargetWidthRatio,
                        unowned.PossibleMergedOcr, unowned.NearbyTargets.Count);
                    foreach (NotificationNearbyOcrDiagnostic nearby in unowned.NearbyTargets)
                    {
                        _logger.LogInformation(
                            "Issue 59 unowned badge nearby OCR: index={BadgeIndex}, targetId={TargetId}, text={Text}, bounds=({X},{Y},{Width},{Height}), dx={DeltaX}, dy={DeltaY}, distance={Distance}, reason={Reason}, widthRatio={WidthRatio}, possibleMerged={PossibleMerged}",
                            unowned.BadgeIndex, nearby.TargetId, nearby.Text,
                            nearby.X, nearby.Y, nearby.Width, nearby.Height,
                            nearby.DeltaX, nearby.DeltaY, nearby.Distance,
                            nearby.RejectionReason, nearby.OcrTargetWidthRatio,
                            nearby.PossibleMergedOcr);
                    }
                }

            }
            catch (Exception diagnosticException)
            {
                // Diagnostics must never block observation or grant click authorization.
                _logger.LogWarning(diagnosticException,
                    "Issue 59 read-only unowned badge diagnostics failed.");
            }

            foreach (NotificationBadgeDiagnostic badge in badgeDiagnostics)
            {
                _logger.LogInformation(
                    "Issue 59 badge center: x={CenterX:F1}, y={CenterY:F1}, insideImage={InsideImage}, candidateNodeIds={CandidateNodeIds}",
                    badge.CenterX, badge.CenterY, badge.IsInsideImage,
                    string.Join(",", badge.CandidateNodeIds));
                foreach (NotificationTargetDiagnostic candidate in badge.NearestTargets)
                {
                    _logger.LogInformation(
                        "Issue 59 nearby OCR: id={TargetId}, text={Text}, bounds=({X},{Y},{Width},{Height}), dx={DeltaX:F1}, dy={DeltaY:F1}, allowedH={AllowedH:F1}, allowedV={AllowedV:F1}, distance={Distance:F1}, accepted={Accepted}, reason={Reason}",
                        candidate.TargetId, candidate.Text, candidate.X, candidate.Y,
                        candidate.Width, candidate.Height, candidate.DeltaX,
                        candidate.DeltaY, candidate.AllowedHorizontal,
                        candidate.AllowedVertical, candidate.Distance,
                        candidate.Accepted, candidate.Reason);
                }
            }
        }

        int associatedNodeCount = result.Nodes.Count;

        int unambiguousRedDotNodeCount = result.Nodes.Count(
            node => node.HasRedDot &&
                    !node.IsRedDotAssociationAmbiguous);

        _logger.LogInformation(
            "Issue 59 associated nodes: {AssociatedNodeCount}",
            associatedNodeCount);

        _logger.LogInformation(
            "Issue 59 unambiguous red-dot nodes: {UnambiguousRedDotNodeCount}",
            unambiguousRedDotNodeCount);

        if (targets.Count == 0)
        {
            _logger.LogWarning(
                "Issue 59 association status: NOT EVALUATED. " +
                "Reason={Reason}",
                isTargetFactoryConfigured
                    ? "OCR_TARGET_FACTORY_RETURNED_ZERO_TARGETS"
                    : "OCR_TARGET_FACTORY_NOT_CONFIGURED");
        }
        else if (associatedNodeCount == 0)
        {
            _logger.LogWarning(
                "Issue 59 association status: NO ASSOCIATED NODES. " +
                "OCR targets={OcrTargetCount}, observation={ObservationId}",
                targets.Count,
                result.ObservationId);
        }
        else if (unambiguousRedDotNodeCount == 0)
        {
            _logger.LogWarning(
                "Issue 59 association status: NO UNAMBIGUOUS RED-DOT NODES. " +
                "Associated nodes={AssociatedNodeCount}, observation={ObservationId}",
                associatedNodeCount,
                result.ObservationId);
        }
        else
        {
            _logger.LogInformation(
                "Issue 59 association status: UNAMBIGUOUS RED-DOT NODES FOUND. " +
                "Count={UnambiguousRedDotNodeCount}, observation={ObservationId}",
                unambiguousRedDotNodeCount,
                result.ObservationId);
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (result.FilteringStatistics is { } statistics)
        {
            _logger.LogInformation(
                $"Issue 59 filtering: observation={result.ObservationId}, " +
                $"raw={statistics.RawCandidates}, filtered={statistics.FilteredCandidates}, " +
                $"rejected={statistics.RejectedCandidates}, " +
                $"followUpFrames={statistics.SubsequentFrameCount}, " +
                $"associatedNodes={statistics.AssociatedNodes}");
        }
        _logger.LogInformation(
            "Issue 59 verified template matches: {Count}",
            result.VerifiedTemplateMatches.Count);
        SaveAnnotatedScreenshot(observation, result, result.VerifiedTemplateMatches);

        return result;
    }

    /// <summary>
    /// Preserves the original observation failure while collecting independent
    /// untrusted OCR evidence for troubleshooting.
    /// </summary>
    private async Task<IAndroidScreenObservation> ObserveOrDiagnoseAsync(
        string phase,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _observationService.ObserveAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (TimeoutException observationFailure)
        {
            _logger.LogWarning(observationFailure,
                "Issue 59 observation failed; collecting untrusted OCR diagnostics for phase={Phase}",
                phase);
            try
            {
                if (_readOnlyDiagnostics is not null)
                    await _readOnlyDiagnostics.CaptureAndLogAsync(
                        phase, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception diagnosticFailure)
            {
                _logger.LogWarning(diagnosticFailure,
                    "Issue 59 untrusted OCR diagnostics failed; phase={Phase}", phase);
            }

            // Never use diagnostic data to continue association or click authorization.
            throw;
        }
    }

    /// <summary>
    /// Saves the analyzed screenshot and its numbered red-dot evidence overlay.
    /// </summary>
    /// <param name="observation">The exact observation used for red-dot analysis.</param>
    /// <param name="result">The evidence produced from the same observation.</param>
    private void SaveAnnotatedScreenshot(
        IAndroidScreenObservation observation,
        RedDotAssociationResult result,
        IReadOnlyList<NotificationTemplateMatch> matches)
    {
        ArgumentNullException.ThrowIfNull(matches);
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(result);

        // Decode the same encoded screenshot; never capture a second frame.
        using Mat image = Cv2.ImDecode(
            observation.ImageBytes.ToArray(),
            ImreadModes.Color);

        if (image.Empty())
        {
            throw new InvalidOperationException(
                "Issue 59 annotation failed: screenshot could not be decoded.");
        }

        using Mat annotated = image.Clone();

        for (int index = 0; index < result.Evidence.Count; index++)
        {
            RedDotEvidence evidence = result.Evidence[index];
            var bounds = evidence.Bounds;

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                throw new InvalidOperationException(
                    "Issue 59 annotation failed: invalid evidence bounds.");
            }

            // Round outward so fractional bounds remain fully visible.
            int left = (int)Math.Floor(
                Math.Clamp(bounds.TopLeft.X, 0d, (double)annotated.Width));
            int top = (int)Math.Floor(
                Math.Clamp(bounds.TopLeft.Y, 0d, (double)annotated.Height));
            int right = (int)Math.Ceiling(
                Math.Clamp(bounds.TopLeft.X + bounds.Width, 0d, (double)annotated.Width));
            int bottom = (int)Math.Ceiling(
                Math.Clamp(bounds.TopLeft.Y + bounds.Height, 0d, (double)annotated.Height));

            if (right <= left || bottom <= top)
            {
                throw new InvalidOperationException(
                    "Issue 59 annotation failed: evidence bounds are outside the screenshot.");
            }

            var rectangle = new OpenCvSharp.Rect(
                left, top, right - left, bottom - top);

            Cv2.Rectangle(
                annotated,
                rectangle,
                new Scalar(0, 255, 255),
                thickness: 2);

            string label = $"{index + 1}: {evidence.Confidence:F3}";
            int labelY = top >= 22
                ? top - 6
                : Math.Min(annotated.Height - 4, bottom + 18);

            Cv2.PutText(
                annotated,
                label,
                new OpenCvSharp.Point(left, labelY),
                HersheyFonts.HersheySimplex,
                0.48,
                new Scalar(0, 0, 0),
                thickness: 3);

            Cv2.PutText(
                annotated,
                label,
                new OpenCvSharp.Point(left, labelY),
                HersheyFonts.HersheySimplex,
                0.48,
                new Scalar(0, 255, 255),
                thickness: 1);
        }


        string directory = Path.Combine(
            AppContext.BaseDirectory,
            "Screenshots",
            "Issue59",
            $"{DateTimeOffset.Now:yyyyMMdd-HHmmss}-{result.ObservationId}");

        Directory.CreateDirectory(directory);

        string originalPath = Path.Combine(directory, "original.png");
        string annotatedPath = Path.Combine(directory, "validated-notifications.png");

        if (!Cv2.ImWrite(originalPath, image) ||
            !Cv2.ImWrite(annotatedPath, annotated))
        {
            throw new IOException(
                "Issue 59 annotation failed: screenshot output could not be written.");
        }

        _logger.LogInformation($"Issue 59 original screenshot: {originalPath}");
        _logger.LogInformation($"Issue 59 validated screenshot: {annotatedPath}");
    }
}