namespace System.Collections.Generic
{
    internal static class ExceptionResource
    {
        
        public const string Arg_InvalidArrayType="Target array type is not compatible with the type of items in the collection.";
        public const string Arg_RankMultiDimNotSupported="Only single dimensional arrays are supported for the requested action.";
        
        public const string ArgumentNull_Key = "Key cannot be null.";
        public const string Argument_AddingDuplicate = "An entry with the same key already exists.";
        public const string Argument_InvalidValue = "Argument {0} should be larger than {1}.";
        public const string ArgumentOutOfRange_NeedNonNegNum = "Index is less than zero.";
        public const string ArgumentOutOfRange_InvalidThreshold="The specified threshold for creating dictionary is out of range.";
        public const string InvalidOperation_EnumFailedVersion="Collection was modified after the enumerator was instantiated.";
        public const string InvalidOperation_EnumOpCantHappen="Enumerator is positioned before the first element or after the last element of the collection.";
        public const string Arg_MultiRank = "Multi dimension array is not supported on this operation.";
        public const string Arg_NonZeroLowerBound = "The lower bound of target array must be zero.";
        public const string Arg_InsufficientSpace="Insufficient space in the target location to copy the information.";
        public const string NotSupported_EnumeratorReset = "Reset is not supported on the Enumerator.";
        public const string Invalid_Array_Type="Target array type is not compatible with the type of items in the collection.";
        public const string Serialization_InvalidOnDeser="OnDeserialization method was called while the object was not being deserialized.";
        public const string Serialization_MissingValues = "The values for this collection are missing.";
        public const string Serialization_MismatchedCount="The serialized Count information doesn't match the number of items.";
        public const string ExternalLinkedListNode = "The LinkedList node does not belong to current LinkedList.";
        public const string LinkedListNodeIsAttached = "The LinkedList node already belongs to a LinkedList.";
        public const string LinkedListEmpty = "The LinkedList is empty.";
        public const string Arg_WrongType="The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.";
        public const string Argument_ItemNotExist = "The specified item does not exist in this KeyedCollection.";
        public const string Argument_ImplementIComparable = "At least one object must implement IComparable.";
        public const string InvalidOperation_EmptyCollection = "This operation is not valid on an empty collection.";
        public const string InvalidOperation_EmptyQueue = "Queue empty.";
        public const string InvalidOperation_EmptyStack = "Stack empty.";
        public const string InvalidOperation_CannotRemoveFromStackOrQueue = "Removal is an invalid operation for Stack or Queue.";
        public const string ArgumentOutOfRange_Index = "Index was out of range. Must be non-negative and less than the size of the collection.";
        public const string ArgumentOutOfRange_SmallCapacity = "capacity was less than the current size.";
        public const string Arg_ArrayPlusOffTooSmall = "Destination array is not long enough to copy all the items in the collection. Check array index and length.";
        public const string NotSupported_KeyCollectionSet = "Mutating a key collection derived from a dictionary is not allowed.";
        public const string NotSupported_ValueCollectionSet = "Mutating a value collection derived from a dictionary is not allowed.";
        public const string NotSupported_ReadOnlyCollection = "Collection is read-only.";
        public const string NotSupported_SortedListNestedWrite = "This operation is not supported on SortedList nested types because they require modifying the original SortedList.";
    }
}
