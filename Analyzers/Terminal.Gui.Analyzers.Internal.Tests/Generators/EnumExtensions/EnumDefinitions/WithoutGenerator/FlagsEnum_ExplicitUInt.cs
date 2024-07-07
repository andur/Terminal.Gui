﻿namespace Terminal.Gui.Analyzers.Internal.Tests.Generators.EnumExtensions.EnumDefinitions;

/// <summary>
///     Flags enum with explicitly-defined backing type of uint and only a <see cref="FlagsAttribute"/> on the enum declaration No other attributes on the enum or its members..
/// </summary>
[Flags]
[SuppressMessage ("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Naming is intentional.")]
[SuppressMessage ("Roslynator", "RCS1154:Sort enum members", Justification = "Order is intentional.")]
public enum FlagsEnum_ExplicitUInt : uint
{
    Bit31 = 0b_10000000_00000000_00000000_00000000u,
    Bit30 = 0b_01000000_00000000_00000000_00000000u,
    Bit29 = 0b_00100000_00000000_00000000_00000000u,
    Bit28 = 0b_00010000_00000000_00000000_00000000u,
    Bit27 = 0b_00001000_00000000_00000000_00000000u,
    Bit26 = 0b_00000100_00000000_00000000_00000000u,
    Bit25 = 0b_00000010_00000000_00000000_00000000u,
    Bit24 = 0b_00000001_00000000_00000000_00000000u,
    Bit23 = 0b_00000000_10000000_00000000_00000000u,
    Bit22 = 0b_00000000_01000000_00000000_00000000u,
    Bit21 = 0b_00000000_00100000_00000000_00000000u,
    Bit20 = 0b_00000000_00010000_00000000_00000000u,
    Bit19 = 0b_00000000_00001000_00000000_00000000u,
    Bit18 = 0b_00000000_00000100_00000000_00000000u,
    Bit17 = 0b_00000000_00000010_00000000_00000000u,
    Bit16 = 0b_00000000_00000001_00000000_00000000u,
    Bit15 = 0b_00000000_00000000_10000000_00000000u,
    Bit14 = 0b_00000000_00000000_01000000_00000000u,
    Bit13 = 0b_00000000_00000000_00100000_00000000u,
    Bit12 = 0b_00000000_00000000_00010000_00000000u,
    Bit11 = 0b_00000000_00000000_00001000_00000000u,
    Bit10 = 0b_00000000_00000000_00000100_00000000u,
    Bit09 = 0b_00000000_00000000_00000010_00000000u,
    Bit08 = 0b_00000000_00000000_00000001_00000000u,
    Bit07 = 0b_00000000_00000000_00000000_10000000u,
    Bit06 = 0b_00000000_00000000_00000000_01000000u,
    Bit05 = 0b_00000000_00000000_00000000_00100000u,
    Bit04 = 0b_00000000_00000000_00000000_00010000u,
    Bit03 = 0b_00000000_00000000_00000000_00001000u,
    Bit02 = 0b_00000000_00000000_00000000_00000100u,
    Bit01 = 0b_00000000_00000000_00000000_00000010u,
    Bit00 = 0b_00000000_00000000_00000000_00000001u,
    All_0 = 0b_00000000_00000000_00000000_00000000u,
    All_1 = 0b_11111111_11111111_11111111_11111111u
}