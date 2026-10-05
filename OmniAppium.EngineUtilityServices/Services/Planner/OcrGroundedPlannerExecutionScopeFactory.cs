using AiUtility.ToolKits.Abstractions;
using OmniAppium.EngineUtilityServices.Services.Grounding;
using OmniAppium.EngineUtilityServices.Services.OCR;
using OCRUtilityServices.Models;
using System.Globalization;

namespace OmniAppium.EngineUtilityServices.Services.Planner;

/// <summary>
/// Creates planner execution scopes by converting a fresh Android OCR result
/// into execution-owned grounded state.
/// </summary>
public sealed class OcrGroundedPlannerExecutionScopeFactory
    : IOcrGroundedPlannerExecutionScopeFactory
{
    private readonly IAndroidScreenOcrService _androidScreenOcrService;
    private readonly IOcrGroundedSnapshotFactory _snapshotFactory;
    private readonly IOcrGroundedSnapshotFreshnessGuard _freshnessGuard;
    private readonly IAiToolExecutionStateAccessor<
        OcrGroundedPlannerExecutionState> _executionStateAccessor;

    private long _snapshotSequence;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="OcrGroundedPlannerExecutionScopeFactory"/> class.
    /// </summary>
    /// <param name="androidScreenOcrService">
    /// Captures and recognizes the current Android screen.
    /// </param>
    /// <param name="snapshotFactory">
    /// Converts OCR results into grounded snapshots.
    /// </param>
    /// <param name="freshnessGuard">
    /// Tracks the snapshot representing the latest observable screen.
    /// </param>
    /// <param name="executionStateAccessor">
    /// Stores planner state in the current logical asynchronous flow.
    /// </param>
    public OcrGroundedPlannerExecutionScopeFactory(
        IAndroidScreenOcrService androidScreenOcrService,
        IOcrGroundedSnapshotFactory snapshotFactory,
        IOcrGroundedSnapshotFreshnessGuard freshnessGuard,
        IAiToolExecutionStateAccessor<
            OcrGroundedPlannerExecutionState> executionStateAccessor)
    {
        ArgumentNullException.ThrowIfNull(androidScreenOcrService);
        ArgumentNullException.ThrowIfNull(snapshotFactory);
        ArgumentNullException.ThrowIfNull(freshnessGuard);
        ArgumentNullException.ThrowIfNull(executionStateAccessor);

        _androidScreenOcrService = androidScreenOcrService;
        _snapshotFactory = snapshotFactory;
        _freshnessGuard = freshnessGuard;
        _executionStateAccessor = executionStateAccessor;
    }

    /// <inheritdoc/>
    public async Task<IOcrGroundedPlannerExecutionScope> CreateAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        OcrResult ocrResult =
            await _androidScreenOcrService
                .RecognizeCurrentScreenAsync(cancellationToken)
                .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        string snapshotId =
            CreateSnapshotId();

        OcrGroundedSnapshot snapshot =
            _snapshotFactory.Create(
                snapshotId,
                ocrResult);

        /*
         * Freshness is device/screen-global, while the execution-state accessor
         * is logical-async-flow-local. Marking a newer screen current therefore
         * intentionally makes older snapshots stale.
         */
        _freshnessGuard.MarkCurrent(
            snapshot.SnapshotId);

        OcrGroundedPlannerExecutionState executionState =
            new(snapshot);

        IDisposable? executionStateLease = null;

        try
        {
            executionStateLease =
                _executionStateAccessor.Push(
                    executionState);

            return new OcrGroundedPlannerExecutionScope(
                snapshot,
                executionStateLease);
        }
        catch
        {
            executionStateLease?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Creates a concurrency-safe process-local snapshot identifier.
    /// </summary>
    /// <returns>
    /// A unique snapshot identifier for this factory instance.
    /// </returns>
    private string CreateSnapshotId()
    {
        long sequence =
            Interlocked.Increment(
                ref _snapshotSequence);

        return string.Create(
            CultureInfo.InvariantCulture,
            $"ocr-snapshot-{sequence:D8}");
    }
}
