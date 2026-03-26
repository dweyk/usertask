using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace UserTaskManagement.Tests.Utils;

public sealed class AutoMoqDataAttribute()
    : AutoDataAttribute(() => new Fixture().Customize(new AutoMoqCustomization()));

