using AutoFixture.Xunit2;

namespace UserTaskManagement.Tests.Utils;

public class InlineAutoMoqDataAttribute(params object?[] objects)
    : InlineAutoDataAttribute(new AutoMoqDataAttribute(), objects);
