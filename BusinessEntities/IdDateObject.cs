using System;

namespace BusinessEntities
{
    public class IdDateObject : IdObject
    {
        private DateTime _modifiedOn;

        public DateTime CreatedOn => DateTime.Now;
        public DateTime ModifiedOn => _modifiedOn;

        public void SetModifiedDate(DateTime modifiedOn) =>
        _modifiedOn = modifiedOn >= CreatedOn
            ? modifiedOn
            : throw new ArgumentOutOfRangeException(nameof(modifiedOn), "Modified date must be on or after the created date.");
    }
}
