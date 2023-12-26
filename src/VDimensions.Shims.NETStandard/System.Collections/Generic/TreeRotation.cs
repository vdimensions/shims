#if SHIM_SORTED_SET
namespace System.Collections.Generic
{
    internal enum TreeRotation 
    {
        LeftRotation = 1,
        RightRotation = 2,
        RightLeftRotation = 3,
        LeftRightRotation = 4,
    }
}
#endif