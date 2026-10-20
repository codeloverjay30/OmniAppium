using OCRUtilityServices.Models;
using OCRUtilityServices.Services;
using CoordinateUtilityServices;
using FluentAssertions;
using Moq;
using OmniAppium.EngineUtilityServices.Services.Click;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class OcrGroundedClickServiceAtomicActionTests
{
    [Fact]
    public void Click_WhenTargetIsValid_ShouldAcquireFreshnessLeaseBeforeExecutingDeviceClick()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);
        Mock<IOcrGroundedActionLease> lease = new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot = CreateSnapshot();
        List<string> operations = [];

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Callback(() => operations.Add("Acquire"))
            .Returns(lease.Object);

        clickService
            .Setup(service => service.ClickAbsolute(20, 30))
            .Callback(() => operations.Add("Click"));

        lease
            .Setup(currentLease => currentLease.Dispose())
            .Callback(() => operations.Add("Dispose"));

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        sut.Click(snapshot, "ocr-0");

        operations.Should().Equal(
            "Acquire",
            "Click",
            "Dispose");

        freshnessGuard.Verify(
            guard => guard.AcquireCurrent(snapshot),
            Times.Once);

        clickService.Verify(
            service => service.ClickAbsolute(20, 30),
            Times.Once);

        lease.Verify(
            currentLease => currentLease.Dispose(),
            Times.Once);
    }

    [Fact]
    public void Click_WhenDeviceClickIsExecuting_ShouldKeepFreshnessLeaseActive()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);
        Mock<IOcrGroundedActionLease> lease = new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot = CreateSnapshot();

        bool leaseIsActive = false;

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Returns(() =>
            {
                leaseIsActive = true;
                return lease.Object;
            });

        clickService
            .Setup(service => service.ClickAbsolute(20, 30))
            .Callback(() =>
            {
                leaseIsActive.Should().BeTrue(
                    "the freshness authorization must remain active " +
                    "while the device click is executing");
            });

        lease
            .Setup(currentLease => currentLease.Dispose())
            .Callback(() => leaseIsActive = false);

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        sut.Click(snapshot, "ocr-0");

        leaseIsActive.Should().BeFalse(
            "the freshness authorization must be released after the click");
    }

    [Fact]
    public void Click_WhenDeviceClickCompletes_ShouldDisposeFreshnessLeaseExactlyOnce()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);
        Mock<IOcrGroundedActionLease> lease = new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot = CreateSnapshot();

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Returns(lease.Object);

        clickService
            .Setup(service => service.ClickAbsolute(20, 30));

        lease
            .Setup(currentLease => currentLease.Dispose());

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        sut.Click(snapshot, "ocr-0");

        lease.Verify(
            currentLease => currentLease.Dispose(),
            Times.Once);
    }

    [Fact]
    public void Click_WhenAcquireCurrentRejectsSnapshot_ShouldNotExecuteDeviceClick()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot = CreateSnapshot();

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Throws(
                new StaleOcrGroundedSnapshotException(
                    "snapshot-1",
                    "snapshot-2"));

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "ocr-0");

        act.Should()
            .Throw<StaleOcrGroundedSnapshotException>()
            .WithMessage(
                "*snapshot-1*stale*snapshot-2*");

        clickService.Verify(
            service => service.ClickAbsolute(
                It.IsAny<double>(),
                It.IsAny<double>()),
            Times.Never);
    }

    [Fact]
    public void Click_WhenDeviceClickThrows_ShouldDisposeFreshnessLeaseExactlyOnce()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);
        Mock<IOcrGroundedActionLease> lease = new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot = CreateSnapshot();

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Returns(lease.Object);

        clickService
            .Setup(service => service.ClickAbsolute(20, 30))
            .Throws(
                new InvalidOperationException(
                    "Device click failed."));

        lease
            .Setup(currentLease => currentLease.Dispose());

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "ocr-0");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*Device click failed*");

        lease.Verify(
            currentLease => currentLease.Dispose(),
            Times.Once);
    }

    [Fact]
    public void Click_WhenDeviceClickThrows_ShouldPropagateOriginalException()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);
        Mock<IOcrGroundedActionLease> lease = new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot = CreateSnapshot();

        InvalidOperationException expectedException =
            new("Device click failed.");

        freshnessGuard
            .Setup(guard => guard.AcquireCurrent(snapshot))
            .Returns(lease.Object);

        clickService
            .Setup(service => service.ClickAbsolute(20, 30))
            .Throws(expectedException);

        lease
            .Setup(currentLease => currentLease.Dispose());

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "ocr-0");

        InvalidOperationException actualException =
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*Device click failed*")
                .Which;

        actualException.Should().BeSameAs(expectedException);
    }

    [Fact]
    public void Click_WhenTargetDoesNotExist_ShouldNotAcquireFreshnessLease()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot = CreateSnapshot();

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "missing-target");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*OCR-grounded target 'missing-target' was not found*");

        freshnessGuard.Verify(
            guard => guard.AcquireCurrent(
                It.IsAny<OcrGroundedSnapshot>()),
            Times.Never);

        clickService.Verify(
            service => service.ClickAbsolute(
                It.IsAny<double>(),
                It.IsAny<double>()),
            Times.Never);
    }

    [Fact]
    public void Click_WhenTargetIsAmbiguous_ShouldNotAcquireFreshnessLease()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot =
            new(
                "snapshot-1",
                [
                    CreateTarget("ocr-0"),
                    CreateTarget("ocr-0")
                ]);

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "ocr-0");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*OCR-grounded target 'ocr-0' is ambiguous*");

        freshnessGuard.Verify(
            guard => guard.AcquireCurrent(
                It.IsAny<OcrGroundedSnapshot>()),
            Times.Never);

        clickService.Verify(
            service => service.ClickAbsolute(
                It.IsAny<double>(),
                It.IsAny<double>()),
            Times.Never);
    }

    [Fact]
    public void Click_WhenTargetBoundsHaveZeroArea_ShouldNotAcquireFreshnessLease()
    {
        Mock<IClickService> clickService = new(MockBehavior.Strict);
        Mock<IOcrGroundedSnapshotFreshnessGuard> freshnessGuard =
            new(MockBehavior.Strict);

        OcrGroundedSnapshot snapshot =
            new(
                "snapshot-1",
                [
                    new OcrGroundedTarget(
                        "ocr-0",
                        "Target",
                        Rectangle.FromXYWH(
                            10,
                            20,
                            0,
                            20))
                ]);

        OcrGroundedClickService sut =
            CreateAuthorizedService(clickService.Object, freshnessGuard.Object);

        Action act = () =>
            sut.Click(snapshot, "ocr-0");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "*OCR-grounded target 'ocr-0' has zero-area bounds*");

        freshnessGuard.Verify(
            guard => guard.AcquireCurrent(
                It.IsAny<OcrGroundedSnapshot>()),
            Times.Never);

        clickService.Verify(
            service => service.ClickAbsolute(
                It.IsAny<double>(),
                It.IsAny<double>()),
            Times.Never);
    }

    private static OcrGroundedClickService CreateAuthorizedService(
        IClickService clickService,
        IOcrGroundedSnapshotFreshnessGuard freshnessGuard)
    {
        Mock<IAtomicGroundedClickEvidenceProvider> evidenceProvider = new(MockBehavior.Strict);
        Mock<IGroundedClickAuthorizationService> authorizationService = new(MockBehavior.Strict);

        evidenceProvider
            .Setup(provider => provider.Consume(
                It.IsAny<OcrGroundedSnapshot>(),
                It.IsAny<OcrGroundedTarget>()))
            .Returns(new ConsumedGroundedClickEvidence(null!, 0.99f));

        authorizationService
            .Setup(service => service.Authorize(
                It.IsAny<string>(),
                It.IsAny<float>(),
                It.IsAny<CoordinateUtilityServices.Rectangle>(),
                It.IsAny<ReviewedGroundTruth>()))
            .Returns((string _, float _, CoordinateUtilityServices.Rectangle bounds, ReviewedGroundTruth _) =>
                (bounds.Center.X, bounds.Center.Y));

        return new OcrGroundedClickService(
            clickService,
            freshnessGuard,
            authorizationService.Object,
            evidenceProvider.Object);
    }

    private static OcrGroundedSnapshot CreateSnapshot()
    {
        return new OcrGroundedSnapshot(
            "snapshot-1",
            [
                CreateTarget("ocr-0")
            ]);
    }

    private static OcrGroundedTarget CreateTarget(
        string targetId)
    {
        return new OcrGroundedTarget(
            targetId,
            "Target",
            Rectangle.FromXYWH(
                10,
                20,
                20,
                20));
    }
}