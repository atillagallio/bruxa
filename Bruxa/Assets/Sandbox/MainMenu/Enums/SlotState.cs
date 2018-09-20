using System;

namespace CharacterSelectionScreen
{
  [Flags]
  internal enum SlotState
  {
    None = 0,
    Free = 1 << 0,
    Joined = 1 << 1,
    Ready = 1 << 2,
    All = ~0,
  }
}