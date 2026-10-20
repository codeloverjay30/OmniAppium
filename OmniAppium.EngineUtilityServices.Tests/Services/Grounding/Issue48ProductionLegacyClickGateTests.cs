using FluentAssertions;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppium.EngineUtilityServices.Tests.Services.Grounding;

public sealed class Issue48ProductionLegacyClickGateTests
{
    [Fact]
    public void Click_WhenLegacyEntryPointUsed_ShouldFailClosed()
    {
        IOcrGroundedClickService sut = new RejectingProductionGroundedClickService();
        var snapshot = new OcrGroundedSnapshot("snapshot", []);
        Action act = () => sut.Click(snapshot, "target");
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Trusted signed review approval is required*");
    }
}
